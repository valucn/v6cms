using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using v6cms.entities.db_set;
using v6cms.models.api;
using v6cms.utils;

namespace v6cms.blls
{
    public class create_html_bll
    {
        private readonly db_context _context;

        public create_html_bll(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 生成html
        /// </summary>
        /// <param name="article_id"></param>
        /// <param name="html_template"></param>
        /// <param name="html_path_rule"></param>
        /// <returns></returns>
        public global_response create_html(int article_id, string html_template, string html_path_rule)
        {
            var resp = new global_response();

            var site_config = _context.site_config.FirstOrDefault();
            var article = _context.article.Where(m => m.id == article_id).Include(m => m.column).FirstOrDefault();
            if (article == null)
            {
                resp.code = 400;
                resp.msg = "文章不存在";
                return resp;
            }
            var article_content = _context.article_content.Where(m => m.article_id == article.id).FirstOrDefault();

            string template = File.ReadAllText(Path.Combine("wwwroot", html_template));

            //当前位置
            string column_parent = "";
            if (article.column.parent_id > 0)
            {
                var parent = _context.column.Where(m => m.id == article.column.parent_id).FirstOrDefault();
                if (parent != null)
                {
                    column_parent = $"/column/article_list/{parent.id}";
                    if (!string.IsNullOrEmpty(parent.route_value))
                    {
                        column_parent = $"<a href='/{parent.route_value}'>{parent.column_name}</a> &gt; ";
                    }
                }
            }
            string column_href = $"/column/article_list/{article.column_id}";
            if (!string.IsNullOrEmpty(article.column.route_value))
            {
                column_href = $"/{article.column.route_value}";
            }
            string position = $"<a href='/'>首页</a> &gt; {column_parent}<a href='{column_href}'>{article.column.column_name}</a>";
            template = template.Replace("[field:position/]", position);

            //循环导航
            var navs = _context.column.Where(m => m.is_show_nav).OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToList();
            var nav_mats = Regex.Matches(template, @"{vtl:nav-list}([\w\W]*?){/vtl:nav-list}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match mat in nav_mats)
            {
                string nav_line_value = "";
                string nav_line_0 = mat.Groups[0].Value;
                string line_template = mat.Groups[1].Value;
                foreach (var item in navs)
                {
                    string url = $"/column/article_list/{item.id}";
                    if (!string.IsNullOrEmpty(item.route_value))
                    {
                        url = $"/{item.route_value}";
                    }
                    string line_temp = line_template;
                    line_temp = line_temp.Replace("[field:url/]", url);
                    line_temp = line_temp.Replace("[field:target/]", item.target);
                    line_temp = line_temp.Replace("[field:column_name/]", item.column_name);
                    nav_line_value += line_temp;
                }
                template = template.Replace(nav_line_0, nav_line_value);
            }

            //循环链接
            var link_mats = Regex.Matches(template, @"{vtl:link category_id='(\d+)'}([\w\W]*?){/vtl:link}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match mat in link_mats)
            {
                string link_line_value = "";
                string link_line_0 = mat.Groups[0].Value;
                int category_id = int.Parse(mat.Groups[1].Value);
                string line_template = mat.Groups[2].Value;

                var link_list = _context.link.Where(m => m.category_id == category_id);
                foreach (var item in link_list)
                {
                    string line_temp = line_template;
                    line_temp = line_temp.Replace("[field:url/]", item.url);
                    line_temp = line_temp.Replace("[field:link_title/]", item.title);
                    link_line_value += line_temp;
                }
                template = template.Replace(link_line_0, link_line_value);
            }

            //循环文章
            var article_lists = Regex.Matches(template, @"{vtl:article-list(?<attributes>.*?)}(?<line_template>[\w\W]*?){/vtl:article-list}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match mat in article_lists)
            {
                string link_line_value = "";
                string link_line_0 = mat.Groups[0].Value;
                string attributes = mat.Groups["attributes"].Value;
                attributes = attributes.Replace("\"", "'");

                var mat_column_id = Regex.Match(attributes, "column_id='(.*?)'");
                string column_id_str = mat_column_id.Groups[1].Value;

                var mat_flag = Regex.Match(attributes, "flag='(.*?)'");
                string flag = mat_flag.Groups[1].Value;

                int skip = 0;
                var mat_skip = Regex.Match(attributes, "skip='(.*?)'");
                string skip_str = mat_skip.Groups[1].Value;
                if (!string.IsNullOrEmpty(skip_str))
                {
                    int.TryParse(skip_str, out skip);
                }

                int take = 10;
                var mat_take = Regex.Match(attributes, "take='(.*?)'");
                string take_str = mat_take.Groups[1].Value;
                if (!string.IsNullOrEmpty(take_str))
                {
                    int.TryParse(take_str, out take);
                }
                //Console.WriteLine($"take={take}");

                int titlelen = 18;
                var mat_titlelen = Regex.Match(attributes, "titlelen='(.*?)'");
                string titlelen_str = mat_titlelen.Groups[1].Value;
                if (!string.IsNullOrEmpty(titlelen_str))
                {
                    int.TryParse(titlelen_str, out titlelen);
                }

                string line_template = mat.Groups["line_template"].Value;

                var article_list = _context.article.AsQueryable();
                if (!string.IsNullOrEmpty(column_id_str))
                {
                    if (column_id_str != "all")
                    {
                        int.TryParse(column_id_str, out int column_id);
                        article_list = article_list.Where(m => m.column_id == column_id);
                    }
                }
                else
                {
                    article_list = article_list.Where(m => m.column_id == article.column_id);
                }

                if (!string.IsNullOrEmpty(flag))
                {
                    if (flag.Contains("recommend"))
                    {
                        article_list = article_list.Where(m => m.is_recommend);
                    }
                    if (flag.Contains("hot"))
                    {
                        article_list = article_list.Where(m => m.is_hot);
                    }
                    if (flag.Contains("best"))
                    {
                        article_list = article_list.Where(m => m.is_best);
                    }
                    if (flag.Contains("nopic"))
                    {
                        article_list = article_list.Where(m => !m.is_pic);
                    }
                    else if (flag.Contains("pic"))
                    {
                        article_list = article_list.Where(m => m.is_pic && !string.IsNullOrEmpty(m.pic));
                    }
                }
                article_list = article_list.Include(m => m.column).OrderByDescending(m => m.publish_time);
                if (skip > 0)
                {
                    article_list = article_list.Skip(skip);
                }
                article_list = article_list.Take(take);
                foreach (var item in article_list)
                {
                    string line_temp = line_template;

                    string article_url = item.column.html_path_rule.Replace("{yyyy}", item.publish_time.ToString("yyyy"));
                    article_url = article_url.Replace("{MM}", item.publish_time.ToString("MM"));
                    article_url = article_url.Replace("{dd}", item.publish_time.ToString("dd"));
                    article_url = article_url.Replace("{id}", item.id.ToString());
                    article_url = article_url.Replace("{article_snow_id}", item.article_snow_id.ToString());

                    line_temp = line_temp.Replace("[field:column_url/]", item.column_id.to_route_url(item.column.route_value));
                    line_temp = line_temp.Replace("[field:column_name/]", item.column.column_name);
                    line_temp = line_temp.Replace("[field:pic/]", item.pic);
                    line_temp = line_temp.Replace("[field:url/]", $"/{article_url}");
                    line_temp = line_temp.Replace("[field:fulltitle/]", item.title);
                    string title = item.title.CutStr(titlelen);
                    line_temp = line_temp.Replace("[field:title/]", title);
                    line_temp = line_temp.Replace("[field:summary/]", item.summary);
                    link_line_value += line_temp;
                }
                template = template.Replace(link_line_0, link_line_value);
            }

            template = template.Replace("[field:article_id/]", article.id.ToString());
            template = template.Replace("[field:column_id/]", article.column_id.ToString());
            template = template.Replace("[field:column_url/]", article.column_id.to_route_url(article.column.route_value));
            template = template.Replace("[field:column_name/]", article.column.column_name);
            template = template.Replace("[field:article_title/]", article.title);
            template = template.Replace("[field:author/]", article.author);
            template = template.Replace("[field:source/]", article.source);
            template = template.Replace("[field:summary/]", article.summary);
            template = template.Replace("[field:content/]", article_content.content);

            //上一篇
            var pre = _context.article.Where(m => m.column_id == article.column_id && m.id < article.id).OrderByDescending(m => m.id).FirstOrDefault();
            if (pre == null)
            {
                template = template.Replace("[field:pre/]", "无");
            }
            else
            {
                string pre_url = html_path_rule.Replace("{yyyy}", pre.publish_time.ToString("yyyy"));
                pre_url = pre_url.Replace("{MM}", pre.publish_time.ToString("MM"));
                pre_url = pre_url.Replace("{dd}", pre.publish_time.ToString("dd"));
                pre_url = pre_url.Replace("{id}", pre.id.ToString());
                pre_url = pre_url.Replace("{article_snow_id}", pre.article_snow_id.ToString());
                template = template.Replace("[field:pre/]", $"<a href='/{pre_url}'>{pre.title}</a>");
            }
            //下一篇
            var next = _context.article.Where(m => m.column_id == article.column_id && m.id > article.id).OrderBy(m => m.id).FirstOrDefault();
            if (next == null)
            {
                template = template.Replace("[field:next/]", "无");
            }
            else
            {
                string next_url = html_path_rule.Replace("{yyyy}", next.publish_time.ToString("yyyy"));
                next_url = next_url.Replace("{MM}", next.publish_time.ToString("MM"));
                next_url = next_url.Replace("{dd}", next.publish_time.ToString("dd"));
                next_url = next_url.Replace("{id}", next.id.ToString());
                next_url = next_url.Replace("{article_snow_id}", next.article_snow_id.ToString());
                template = template.Replace("[field:next/]", $"<a href='/{next_url}'>{next.title}</a>");
            }

            var math = Regex.Match(template, "\\[field:create_time format=\"(.*?)\" /\\]");
            template = Regex.Replace(template, "\\[field:create_time format=\"(.*?)\" /\\]", article.create_time.ToString(math.Groups[1].Value));

            template = template.Replace("[field:site_name/]", site_config.site_name);
            template = template.Replace("[field:copyright/]", site_config.copyright);
            template = template.Replace("[field:icp/]", site_config.icp);
            template = template.Replace("[field:count_code/]", site_config.count_code);

            string contents = template;

            html_path_rule = html_path_rule.Replace("{yyyy}", article.publish_time.ToString("yyyy"));
            html_path_rule = html_path_rule.Replace("{MM}", article.publish_time.ToString("MM"));
            html_path_rule = html_path_rule.Replace("{dd}", article.publish_time.ToString("dd"));
            html_path_rule = html_path_rule.Replace("{id}", article.id.ToString());
            html_path_rule = html_path_rule.Replace("{article_snow_id}", article.article_snow_id.ToString());
            string file_name = Path.Combine("wwwroot", html_path_rule);
            string dir = Path.GetDirectoryName(file_name);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(file_name, contents);

            resp.code = 200;
            resp.msg = "生成成功";
            return resp;
        }

        /// <summary>
        /// 生成html
        /// </summary>
        /// <param name="article_id"></param>
        /// <param name="html_template"></param>
        /// <param name="html_path_rule"></param>
        /// <returns></returns>
        public async Task<global_response> create_html_asyc(int article_id, string html_template, string html_path_rule)
        {
            var site_config = await _context.site_config.FirstOrDefaultAsync();
            var navs = await _context.column.Where(m => m.is_show_nav).ToListAsync();
            var article = await _context.article.Where(m => m.id == article_id).Include(m => m.column).FirstOrDefaultAsync();
            var article_content = await _context.article_content.Where(m => m.article_id == article.id).FirstOrDefaultAsync();

            string template = await File.ReadAllTextAsync(Path.Combine("wwwroot", html_template));

            var nav_mats = Regex.Matches(template, @"{vtl:nav-list}([\w\W]*?){/vtl:nav-list}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match mat in nav_mats)
            {
                string nav_line_value = "";
                string nav_line_0 = mat.Groups[0].Value;
                string line_template = mat.Groups[1].Value;
                foreach (var item in navs)
                {
                    string url = $"/column/article_list/{item.id}";
                    if (!string.IsNullOrEmpty(item.route_value))
                    {
                        url = $"/{item.route_value}";
                    }
                    string line_temp = line_template;
                    line_temp = line_temp.Replace("[field:url/]", url);
                    line_temp = line_temp.Replace("[field:target/]", item.target);
                    line_temp = line_temp.Replace("[field:column_name/]", item.column_name);
                    nav_line_value += line_temp;
                }
                template = template.Replace(nav_line_0, nav_line_value);
            }

            var link_mats = Regex.Matches(template, @"{vtl:link category_id='(\d+)'}([\w\W]*?){/vtl:link}", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match mat in link_mats)
            {
                string link_line_value = "";
                string link_line_0 = mat.Groups[0].Value;
                int category_id = int.Parse(mat.Groups[1].Value);
                string line_template = mat.Groups[2].Value;

                var link_list = _context.link.Where(m => m.category_id == category_id);
                foreach (var item in link_list)
                {
                    string line_temp = line_template;
                    line_temp = line_temp.Replace("[field:url/]", item.url);
                    line_temp = line_temp.Replace("[field:link_title/]", item.title);
                    link_line_value += line_temp;
                }
                template = template.Replace(link_line_0, link_line_value);
            }

            template = template.Replace("[field:article_id/]", article.id.ToString());
            template = template.Replace("[field:column_id/]", article.column_id.ToString());
            template = template.Replace("[field:column_name/]", article.column.column_name);
            template = template.Replace("[field:article_title/]", article.title);
            template = template.Replace("[field:content/]", article_content.content);

            var math = Regex.Match(template, "\\[field:create_time format=\"(.*?)\" /\\]");
            template = Regex.Replace(template, "\\[field:create_time format=\"(.*?)\" /\\]", article.create_time.ToString(math.Groups[1].Value));

            template = template.Replace("[field:site_name/]", site_config.site_name);
            template = template.Replace("[field:copyright/]", site_config.copyright);
            template = template.Replace("[field:icp/]", site_config.icp);
            template = template.Replace("[field:count_code/]", site_config.count_code);

            string contents = template;
            html_path_rule = html_path_rule.Replace("{id}", article.id.ToString());
            html_path_rule = html_path_rule.Replace("{article_snow_id}", article.article_snow_id.ToString());
            html_path_rule = html_path_rule.Replace("{yyyy}", article.publish_time.ToString("yyyy"));
            html_path_rule = html_path_rule.Replace("{MM}", article.publish_time.ToString("MM"));
            html_path_rule = html_path_rule.Replace("{dd}", article.publish_time.ToString("dd"));
            string file_name = Path.Combine("wwwroot", html_path_rule);
            if (!Directory.Exists(Path.GetDirectoryName(file_name)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file_name));
            }
            await File.WriteAllTextAsync(file_name, contents);

            var resp = new global_response { code = 200, msg = "生成成功" };
            return resp;
        }
    }
}
