using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.blls;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.services;
using v6cms.utils;
using Webdiyer.AspNetCore;
using Yitter.IdGenerator;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 文章管理控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "article/index")]
    public class articleController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;
        private readonly IDataService _service;

        public articleController(IMemoryCache cache, db_context context, IDataService service)
        {
            _cache = cache;
            _context = context;
            _service = service;
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        [admin_role_filter(authority_code = "article/index")]
        public async Task<IActionResult> index(int? column_id, string keyword, int page = 1)
        {
            ViewData["keyword"] = keyword;

            //获取上次访问页面
            string last_visit_url = Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            string user_id_str = HttpContext.User.FindFirstValue("user_id");
            int user_id = int.Parse(user_id_str);
            var user = _context.user.Where(m => m.id == user_id).FirstOrDefault();
            var role = _context.user_role.Where(m => m.id == user.role_id).FirstOrDefault();
            if (role.authority_code_list.Contains("article/review"))
            {
                ViewData["review"] = "1";
            }

            var query = _context.article.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                //query = query.Where(m => m.title.Contains(keyword) || m.content_nohtml.Contains(keyword));
                var parameters = new object[] { keyword };
                keyword = keyword.Trim().Replace(" ", "%");
                keyword = V6.ChkSql(keyword);
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = _context.article.FromSqlRaw($"SELECT * FROM T_article WHERE title LIKE '%{keyword}%' OR CONTAINS(content_nohtml, '{keyword}')", parameters);
                }
            }
            if (column_id.HasValue)
            {
                ViewData["column_id"] = column_id;
                query = query.Where(m => m.column_id == column_id || m.sub_column.Contains($",{column_id.ToString()},"));
            }
            if (role.data_range == data_range_enum.仅本人数据权限)
            {
                query = query.Where(m => m.user_id == user_id);
            }
            else if (role.data_range == data_range_enum.本部门数据权限 || role.data_range == data_range_enum.本部门及以下数据权限)
            {
                query = query.Where(m => m.dept_id == role.id);
            }
            var model = await query.OrderByDescending(m => m.publish_time).Include(m => m.column).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        [admin_role_filter(authority_code = "article/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            var article_snow_id = YitIdHelper.NextId();

            var model = new article_entity
            {
                article_snow_id = article_snow_id,
                publish_time = DateTime.Now,
                views = 1
            };
            return View(model);
        }

        /// <summary>
        /// 添加文章POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "article/create")]
        public async Task<IActionResult> create(article_entity model, string content)
        {
            if (ModelState.IsValid)
            {
                string user_id_str = HttpContext.User.FindFirstValue("user_id");
                string user_dept_id_str = HttpContext.User.FindFirstValue("user_dept_id");
                string user_role_id_str = HttpContext.User.FindFirstValue("user_role_id");
                int user_id = int.Parse(user_id_str);
                int user_role_id = int.Parse(user_role_id_str);
                int user_dept_id = int.Parse(user_dept_id_str);
                var role = await _context.user_role.FirstOrDefaultAsync(m => m.id == user_role_id);

                bool is_review = false;//默认不审核文档
                if (!role.is_need_review)
                {
                    //如果用户组发布文章需要审核=false，则默认审核
                    is_review = true;
                }
                var column = await _context.column.FirstOrDefaultAsync(m => m.id == model.column_id);
                if (!column.is_need_review)
                {
                    //如果选择的栏目需要审核=false，则默认审核
                    is_review = true;
                }

                model.title = model.title.Trim();
                model.article_snow_id = model.article_snow_id;
                model.user_id = user_id;
                model.dept_id = user_dept_id;
                model.content_nohtml = V6.NoHTML(content).Trim();
                model.is_review = is_review;
                model.create_time = DateTime.Now;
                _context.Add(model);
                //保存得到文章主键id
                await _context.SaveChangesAsync();
                //保存文章内容
                _context.article_content.Add(new article_content_entity
                {
                    article_id = model.id,
                    content = content
                });
                await _context.SaveChangesAsync();

                //删除缓存
                //_cache.Remove($"column_details_{model.column_id}_cache");
                //_cache.Remove($"article_list_{model.column_id}_cache");
                //_cache.Remove($"article_list_{model.column_id}_true_true_cache");
                //_cache.Remove($"article_list_{model.column_id}_true_false_cache");
                //_cache.Remove($"article_list_{model.column_id}_false_true_cache");
                //_cache.Remove($"article_list_{model.column_id}_false_false_cache");
                //_cache.Remove($"pic_article_list_cache");
                //_cache.Remove($"pic_article_list_{model.column_id}_cache");
                clear_cache(model.column_id, model.id);
                if (!string.IsNullOrEmpty(model.sub_column))
                {
                    foreach (var item in model.sub_column.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //_cache.Remove($"column_details_{item}_cache");
                            //_cache.Remove($"article_list_{item}_cache");
                            //_cache.Remove($"pic_article_list_{item}_cache");
                            clear_cache(int.Parse(item), model.id);
                        }
                    }
                }

                //生成html
                if (column.list_option != list_option_enum.使用动态页)
                {
                    string html_template = column.html_template;
                    string html_path_rule = column.html_path_rule;
                    create_html_bll chtml = new create_html_bll(_context);
                    chtml.create_html(model.id, html_template, html_path_rule);
                }
                return RedirectToAction(nameof(index));
            }

            return View(model);
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        [admin_role_filter(authority_code = "article/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            string last_visit_url = Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.article.Where(m => m.id == id).FirstOrDefaultAsync();
            if (model == null)
            {
                return NotFound();
            }
            var article_content = await _context.article_content.Where(m => m.article_id == model.id).FirstOrDefaultAsync();
            ViewData["content"] = article_content.content;

            var pics = html_helper.GetHtmlImageUrlList(article_content.content);
            ViewData["pics"] = pics;

            return View(model);
        }

        /// <summary>
        /// 修改文章POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "article/edit")]
        public async Task<IActionResult> edit(int id, article_entity model, string content)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string user_role_id_str = HttpContext.User.FindFirstValue("user_role_id");
                    int user_role_id = int.Parse(user_role_id_str);
                    var role = _context.user_role.Where(m => m.id == user_role_id).FirstOrDefault();

                    bool is_review = false;//默认不审核文档
                    if (!role.is_need_review)
                    {
                        //如果用户组发布文章需要审核=false，则默认审核
                        is_review = true;
                    }
                    var column = await _context.column.FirstOrDefaultAsync(m => m.id == model.column_id);
                    if (!column.is_need_review)
                    {
                        //如果选择的栏目需要审核=false，则默认审核
                        is_review = true;
                    }

                    string content_nohtml = V6.NoHTML(content).Trim();
                    //保存文章主表
                    await _context.article.Where(m => m.id == id).UpdateFromQueryAsync(x => new article_entity
                    {
                        article_snow_id = model.article_snow_id,
                        column_id = model.column_id,
                        sub_column = model.sub_column,
                        title = model.title.Trim(),
                        author = model.author,
                        source = model.source,
                        summary = model.summary,
                        is_review = is_review,
                        is_slide = model.is_slide,
                        is_top = model.is_top,
                        is_best = model.is_best,
                        is_recommend = model.is_recommend,
                        is_sr = model.is_sr,
                        is_hot = model.is_hot,
                        is_pic = model.is_pic,
                        is_limit_ip = model.is_limit_ip,
                        use_gab = model.use_gab,
                        use_province = model.use_province,
                        use_city = model.use_city,
                        use_branch = model.use_branch,
                        content_nohtml = content_nohtml,
                        pic = model.pic,
                        video = model.video,
                        publish_time = model.publish_time,
                        update_time = DateTime.Now
                    });
                    //保存文章内容
                    await _context.article_content.Where(m => m.article_id == id).UpdateFromQueryAsync(x => new article_content_entity
                    {
                        content = content
                    });

                    //删除缓存
                    //_cache.Remove($"column_details_{model.column_id}_cache");
                    //_cache.Remove($"article_list_{model.column_id}_cache");
                    //_cache.Remove($"article_list_{model.column_id}_true_true_cache");
                    //_cache.Remove($"article_list_{model.column_id}_true_false_cache");
                    //_cache.Remove($"article_list_{model.column_id}_false_true_cache");
                    //_cache.Remove($"article_list_{model.column_id}_false_false_cache");
                    //_cache.Remove($"pic_article_list_cache");
                    //_cache.Remove($"pic_article_list_{model.column_id}_cache");
                    //_cache.Remove($"article_details_{id}_cache");
                    clear_cache(model.column_id, id);
                    if (!string.IsNullOrEmpty(model.sub_column))
                    {
                        foreach (var item in model.sub_column.Split(','))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                //_cache.Remove($"column_details_{item}_cache");
                                //_cache.Remove($"article_list_{item}_cache");
                                //_cache.Remove($"pic_article_list_{item}_cache");
                                clear_cache(int.Parse(item), id);
                            }
                        }
                    }

                    //生成html
                    if (column.list_option != list_option_enum.使用动态页)
                    {
                        string html_template = column.html_template;
                        string html_path_rule = column.html_path_rule;

                        create_html_bll chtml = new create_html_bll(_context);
                        chtml.create_html(id, html_template, html_path_rule);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!exists(model.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(index));
            }

            return View(model);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        [admin_role_filter(authority_code = "article/delete")]
        public async Task<IActionResult> delete(int id)
        {
            var model = await _context.article.FirstOrDefaultAsync(m => m.id == id);
            //删除缓存
            //_cache.Remove($"column_details_{model.column_id}_cache");
            //_cache.Remove($"article_list_{model.column_id}_cache");
            //_cache.Remove($"article_list_{model.column_id}_true_true_cache");
            //_cache.Remove($"article_list_{model.column_id}_true_false_cache");
            //_cache.Remove($"article_list_{model.column_id}_false_true_cache");
            //_cache.Remove($"article_list_{model.column_id}_false_false_cache");
            //_cache.Remove($"pic_article_list_cache");
            //_cache.Remove($"pic_article_list_{model.column_id}_cache");
            clear_cache(model.column_id, id);
            if (!string.IsNullOrEmpty(model.sub_column))
            {
                foreach (var item in model.sub_column.Split(','))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //_cache.Remove($"column_details_{item}_cache");
                        //_cache.Remove($"article_list_{item}_cache");
                        //_cache.Remove($"pic_article_list_{item}_cache");
                        clear_cache(int.Parse(item), id);
                    }
                }
            }
            _cache.Remove($"article_details_{id}_cache");
            //删除文章
            await _context.article.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.article.Any(m => m.id == id);
        }

        private void clear_cache(int column_id, int article_id)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            if (cacheItems != null)
            {
                foreach (DictionaryEntry cacheItem in cacheItems)
                {
                    string key = cacheItem.Key.ToString();
                    if (key.StartsWith($"article_list_{column_id}_")
                        || key.StartsWith($"article_details_{article_id}_")
                        || key.StartsWith($"column_child_list_{column_id}_")
                        || key.StartsWith($"column_details_{column_id}_")
                        || (key.Contains("article_list_") && key.Contains(column_id.ToString())))
                    {
                        _cache.Remove(cacheItem.Key);
                    }
                }
            }
        }
    }
}
