using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;

namespace v6cms.web.Controllers.plug
{
    /// <summary>
    /// SSCMS外挂-领导阅评
    /// </summary>
    public class sscms_commentsController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly sscms_db_context _sscms_context;

        public sscms_commentsController(IConfiguration configuration, sscms_db_context sscms_context)
        {
            Configuration = configuration;
            _sscms_context = sscms_context;
        }

        /// <summary>
        /// 显示评字
        /// </summary>
        /// <param name="ContentId">内容id主键</param>
        /// <returns></returns>
        public async Task<IActionResult> show_ping(int ContentId)
        {
            StringBuilder html = new StringBuilder();

            var comments = await _sscms_context.comments.FirstOrDefaultAsync(m => m.ContentId == ContentId);
            if (comments != null)
            {
                html.AppendLine("document.write('<i class=\"ping\">评</i>');");
            }

            //取消15天前领导评论的置顶
            int rows = await _sscms_context.sscms_content.Where(m => m.Top && m.comment_time < DateTime.Now.AddDays(-15)).UpdateFromQueryAsync(x => new sscms_content_entity
            {
                Top = false
            });
            Console.WriteLine($"rows={rows}");
            return Content(html.ToString());
        }

        /// <summary>
        /// 获取评论内容
        /// </summary>
        /// <param name="ContentId">内容id主键</param>
        /// <param name="ChannelId">栏目id主键</param>
        /// <returns></returns>
        public async Task<IActionResult> get_content(int ContentId, int ChannelId)
        {
            StringBuilder html = new StringBuilder();
            //获取当前页面
            string post_url = HttpContext.Request.GetDisplayUrl();
            var comments = await _sscms_context.comments.Where(m => m.ContentId == ContentId).ToListAsync();
            if (comments.Count() == 0)
            {
                html.AppendLine("var is_comment = false;//未评论");

                html.AppendLine($"document.write('<form id=\"comment_form\" method=\"POST\" action=\"{post_url}\">');");
                html.AppendLine("document.write('    <input type=\"hidden\" id=\"ChannelId\" name=\"ChannelId\" value=" + ChannelId + ">');");
                html.AppendLine("document.write('    <table class=\"comment-table\">');");
                html.AppendLine("document.write('        <tr>');");
                html.AppendLine("document.write('            <th>');");
                html.AppendLine("document.write('                姓名');");
                html.AppendLine("document.write('            </th>');");
                html.AppendLine("document.write('            <td>');");
                html.AppendLine("document.write('                <input id=\"CommentPwd\" name=\"CommentPwd\" autocomplete=\"off\" />');");
                html.AppendLine("document.write('            </td>');");
                html.AppendLine("document.write('        </tr>');");
                html.AppendLine("document.write('        <tr>');");
                html.AppendLine("document.write('            <th>');");
                html.AppendLine("document.write('                评论内容');");
                html.AppendLine("document.write('            </th>');");
                html.AppendLine("document.write('            <td>');");
                html.AppendLine("document.write('                <textarea id=\"CommentContent\" name=\"CommentContent\"></textarea>');");
                html.AppendLine("document.write('            </td>');");
                html.AppendLine("document.write('        </tr>');");
                html.AppendLine("document.write('        <tr>');");
                html.AppendLine("document.write('            <th>');");
                html.AppendLine("document.write('            </th>');");
                html.AppendLine("document.write('            <td>');");
                html.AppendLine("document.write('                <button type=\"submit\" class=\"comment-btn\">提交</button>');");
                html.AppendLine("document.write('            </td>');");
                html.AppendLine("document.write('        </tr>');");
                html.AppendLine("document.write('    </table>');");
                html.AppendLine("document.write('</form>');");
            }
            else
            {
                html.AppendLine("var is_comment = true;//已评论");

                html.AppendLine($"document.write('<form id=\"comment_form\" method=\"POST\" action=\"{post_url}\">');");
                html.AppendLine("document.write('    <input type=\"hidden\" id=\"ChannelId\" name=\"ChannelId\" value=" + ChannelId + ">');");
                html.AppendLine("document.write('    <table class=\"comment-table\">');");
                html.AppendLine("document.write('        <tr>');");
                html.AppendLine("document.write('            <th>');");
                html.AppendLine("document.write('                姓名');");
                html.AppendLine("document.write('            </th>');");
                html.AppendLine("document.write('            <td>');");
                html.AppendLine("document.write('                <input id=\"CommentPwd\" name=\"CommentPwd\" autocomplete=\"off\" />');");
                html.AppendLine("document.write('            </td>');");
                html.AppendLine("document.write('        </tr>');");
                html.AppendLine("document.write('        <tr>');");
                html.AppendLine("document.write('            <th>');");
                html.AppendLine("document.write('                评论内容');");
                html.AppendLine("document.write('            </th>');");
                html.AppendLine("document.write('            <td>');");
                html.AppendLine("document.write('                <textarea id=\"CommentContent\" name=\"CommentContent\"></textarea>');");
                html.AppendLine("document.write('            </td>');");
                html.AppendLine("document.write('        </tr>');");
                html.AppendLine("document.write('        <tr>');");
                html.AppendLine("document.write('            <th>');");
                html.AppendLine("document.write('            </th>');");
                html.AppendLine("document.write('            <td>');");
                html.AppendLine("document.write('                <button type=\"submit\" class=\"comment-btn\">保存</button>');");
                html.AppendLine("document.write('                <button id=\"v6cms-delete-comment\" type=\"button\" class=\"comment-btn\">删除</button>');");
                html.AppendLine("document.write('            </td>');");
                html.AppendLine("document.write('        </tr>');");
                html.AppendLine("document.write('    </table>');");
                html.AppendLine("document.write('</form>');");
            }
            foreach (var item in comments)
            {
                string content = "";
                if (!string.IsNullOrEmpty(item.Content))
                {
                    content = item.Content.Replace("\r", "<br />").Replace("\n", "<br />").Replace("\r\n", "<br />");
                }
                html.AppendLine("document.write('<div class=\"pyxx_style\">');");
                html.AppendLine("document.write('    <img src=\"/images/ldyp_top.jpg\">');");
                html.AppendLine("document.write('    <h3 class=\"h3_plnr\">领导阅评</h3>');");
                html.AppendLine($"document.write('    <div class=\"Content\">{content}</div>');");
                html.AppendLine($"document.write('    <span class=\"shijian\">{item.CreatedDate.GetValueOrDefault().ToString("yyyy年M月d日")}</span>');");
                html.AppendLine($"document.write('    <span class=\"update-del\">');");
                html.AppendLine($"document.write('        <button class=\"v6cms-modify-comment\" type=\"button\" data-content=\"{content}\">修改/删除</button>');");
                html.AppendLine($"document.write('    </span>');");
                html.AppendLine($"document.write('    <span class=\"xingming\">{item.leader_name}</span>');");
                html.AppendLine("document.write('    <img src=\"/images/ldyp_bottom.jpg\">');");
                html.AppendLine("document.write('</div>');");
            }
            return Content(html.ToString());
        }

        /// <summary>
        /// POST提交评论内容
        /// </summary>
        /// <param name="ContentId">内容id主键</param>
        /// <param name="ChannelId">栏目id主键</param>
        /// <param name="CommentPwd">评论密码</param>
        /// <param name="Content">评论内容</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")] //支持跨域
        public async Task<IActionResult> get_content(int ContentId, int ChannelId, string CommentPwd, string CommentContent)
        {
            var user = _sscms_context.siteserver_User.FirstOrDefault(m => m.ExtendValues.Contains($"\"commentPwd\":\"{CommentPwd}\""));
            if (user == null)
            {
                var resp = new { code = 500, message = "姓名不对" };
                return Json(resp);
            }
            else
            {
                var now = DateTime.Now;
                var comments = _sscms_context.comments.FirstOrDefault(m => m.ContentId == ContentId && m.leader_name == user.DisplayName);
                if (comments == null)
                {
                    //更新内容表置顶和评论时间字段
                    int content_rows = await _sscms_context.sscms_content.Where(m => m.Id == ContentId).UpdateFromQueryAsync(x => new sscms_content_entity
                    {
                        Top = true,
                        comment_time = now
                    });
                    //将评论插入表
                    _sscms_context.comments.Add(new sscms_comments_entity
                    {
                        Guid = Guid.NewGuid().ToString(),
                        CreatedDate = now,
                        SiteId = 1,
                        ChannelId = ChannelId,
                        ContentId = ContentId,
                        Content = CommentContent,
                        Status = "Approved",
                        leader_name = user.DisplayName
                    });
                    int rows = await _sscms_context.SaveChangesAsync();
                    //重新生成sscms首页html
                    await recreate_html();
                    if ((rows + content_rows) > 1)
                    {
                        var resp = new { code = 200, message = "评论成功" };
                        return Json(resp);
                    }
                    else
                    {
                        var resp = new { code = 600, message = "评论失败" };
                        return Json(resp);
                    }
                }
                else
                {
                    //更新内容表置顶和评论时间字段
                    int content_rows = await _sscms_context.sscms_content.Where(m => m.Id == ContentId).UpdateFromQueryAsync(x => new sscms_content_entity
                    {
                        Top = true,
                        comment_time = now
                    });
                    //更新评论内容
                    int rows = await _sscms_context.comments.Where(m => m.ContentId == ContentId && m.leader_name == user.DisplayName).UpdateFromQueryAsync(x => new sscms_comments_entity
                    {
                        LastModifiedDate = now,
                        Content = CommentContent
                    });
                    //重新生成sscms首页html
                    await recreate_html();
                    if ((rows + content_rows) > 1)
                    {
                        var resp = new { code = 200, message = "评论更新成功" };
                        return Json(resp);
                    }
                    else
                    {
                        var resp = new { code = 600, message = "评论更新失败" };
                        return Json(resp);
                    }
                }
            }
        }
        
        /// <summary>
        /// 删除评论内容
        /// </summary>
        /// <param name="ContentId">内容id主键</param>
        /// <param name="ChannelId">栏目id主键</param>
        /// <param name="CommentPwd">评论密码</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("any")] //支持跨域
        public async Task<IActionResult> delete(int ContentId, int ChannelId, string CommentPwd)
        {
            var user = _sscms_context.siteserver_User.FirstOrDefault(m => m.ExtendValues.Contains($"\"commentPwd\":\"{CommentPwd}\""));
            if (user == null)
            {
                var resp = new { code = 500, message = "姓名不对" };
                return Json(resp);
            }
            else
            {
                Console.WriteLine("删除评论");
                //删除评论
                int rows = await _sscms_context.comments.Where(m => m.ContentId == ContentId && m.leader_name == user.DisplayName).DeleteFromQueryAsync();
                Console.WriteLine("更新内容表置顶和评论时间字段");
                //更新内容表置顶和评论时间字段
                int content_rows = await _sscms_context.sscms_content.Where(m => m.Id == ContentId).UpdateFromQueryAsync(x => new sscms_content_entity
                {
                    Top = false,
                    comment_time = null
                });
                //重新生成sscms首页html
                await recreate_html();
                if ((rows + content_rows) > 1)
                {
                    var resp = new { code = 200, message = "删除评论成功" };
                    return Json(resp);
                }
                else
                {
                    var resp = new { code = 600, message = "删除评论失败" };
                    return Json(resp);
                }
            }
        }
        
        /// <summary>
        /// 重新生成sscms首页html
        /// </summary>
        /// <returns></returns>
        private async Task<bool> recreate_html()
        {
            string root_url = Configuration.GetSection("site_config:sscms_url").Value;
            string sscms_account = Configuration.GetSection("site_config:sscms_account").Value;
            string sscms_password = Configuration.GetSection("site_config:sscms_password").Value;
            string url = $"{root_url}/api/v1/administrators/actions/login";

            var post_data_obj = new
            {
                account = sscms_account,
                password = sscms_password
            };
            string post_data = JsonConvert.SerializeObject(post_data_obj);
            HttpContent httpContent = new StringContent(post_data);
            var client = new HttpClient();
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";
            var http_response = await client.PostAsync(url, httpContent);
            string src = "";
            if (http_response.IsSuccessStatusCode)
            {
                src = await http_response.Content.ReadAsStringAsync();
            }
            var resp = JsonConvert.DeserializeObject<dynamic>(src);
            string accessToken = resp.accessToken;

            string create_url = $"{root_url}/api/admin/cms/create/createPage";
            var create_post_data_obj = new
            {
                siteId = 1,
                type = "Index",
                channelIdList = new int[] { 1 },
                isAllChecked = false,
                isDescendent = false,
                isChannelPage = true,
                isContentPage = false
            };
            string create_post_data = JsonConvert.SerializeObject(create_post_data_obj);
            HttpContent createContent = new StringContent(create_post_data);
            createContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            createContent.Headers.ContentType.CharSet = "utf-8";
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var create_response = await client.PostAsync(create_url, createContent);
            string create_src = "";
            if (create_response.IsSuccessStatusCode)
            {
                create_src = await create_response.Content.ReadAsStringAsync();
            }
            var create_resp = JsonConvert.DeserializeObject<dynamic>(src);
            return true;// create_resp.value;
        }
    }
}
