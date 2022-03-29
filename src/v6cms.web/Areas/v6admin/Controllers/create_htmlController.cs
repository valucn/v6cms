using v6cms.entities;
using v6cms.entities.db_set;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.RegularExpressions;
using System.IO;
using v6cms.blls;
using v6cms.entities.enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 生成html控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter]
    public class create_htmlController : Controller
    {
        private readonly db_context _context;

        /// <summary>
        /// 生成html
        /// </summary>
        /// <param name="context"></param>
        public create_htmlController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 生成文章html
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "create_html/article")]
        public async Task<IActionResult> article()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            int end_id = await _context.article.MaxAsync(m => m.id);
            ViewData["end_id"] = end_id;

            //var column_list = await _context.column.Where(m => m.list_option != list_option_enum.使用动态页).ToListAsync();
            //ViewBag.column_list = column_list.Select(x => new SelectListItem { Value = x.id.ToString(), Text = x.column_name });

            return View();
        }

        /// <summary>
        /// GET生成文章html
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "create_html/article")]
        public async Task<IActionResult> article_creater(int column_id, int start_id, int end_id, int page_size)
        {
            var query = _context.article.AsQueryable();
            if (column_id > 0)
            {
                //查询栏目
                var column = await _context.column.Where(m => m.id == column_id).FirstOrDefaultAsync();
                if (column == null)
                {
                    ViewData["msg"] = "栏目不存在";
                    return View();
                }
                //查询子栏目
                var child = await _context.column.Where(m => m.parent_id == column.id).ToListAsync();
                if (child.Count() > 0)
                {
                    query = query.Where(m => child.Select(m => m.id).Contains(m.column_id));
                }
                else
                {
                    query = query.Where(m => m.column_id == column_id);
                }
                var max_id = await query.MaxAsync(m => (int?)m.id);
                if (max_id.HasValue)
                {
                    end_id = max_id.Value;
                }
                else
                {
                    ViewData["msg"] = "没有文章需要生成";
                    return View();
                }
            }
            string msg;
            var article = await query.Where(m => m.id >= start_id).Include(m => m.column).FirstOrDefaultAsync();
            if (article == null)
            {
                msg = "文章不存在";
            }
            else
            {
                start_id = article.id;
                if (string.IsNullOrEmpty(article.column.html_template) || string.IsNullOrEmpty(article.column.html_path_rule))
                {
                    msg = "模板和生成规则为空";
                }
                else
                {
                    if (article.column.list_option != list_option_enum.使用动态页)
                    {
                        var chtml = new create_html_bll(_context);
                        var result = chtml.create_html(article.id, article.column.html_template, article.column.html_path_rule);
                        msg = $"正在生成，id={article.id}，title={article.title}\r\n<br />{result.msg}";
                    }
                    else
                    {
                        msg = "动态页无需生成";
                    }
                }
            }
            msg += "，准备生成下一条....";

            string script = "";
            if (start_id < end_id)
            {
                int new_start_id = start_id + 1;
                script = "<script>\r\n";
                script += "    parent.document.getElementById('start_id').value = '" + new_start_id + "';\r\n";
                script += "    setTimeout(\"location.href = 'article_creater?column_id=" + column_id + "&start_id="
                        + new_start_id + "&end_id=" + end_id + "&page_size=" + page_size + "'\", 200);\r\n";
                script += "</script>\r\n";
            }
            else
            {
                msg = "全部生成完成。";
            }

            ViewData["msg"] = msg;
            ViewData["column_id"] = column_id;
            ViewData["script"] = script;
            return View();
        }
    }
}
