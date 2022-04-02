using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.blls;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.services;
using v6cms.utils;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 文章控制器
    /// </summary>
    public class articleController : v6baseController
    {
        private readonly db_context _context;
        private readonly IDataService _service;

        public articleController(db_context context, IDataService service)
        {
            _context = context;
            _service = service;
        }

        /// <summary>
        /// 文章详情页
        /// </summary>
        [ResponseCache(Duration = 3, VaryByQueryKeys = new string[] { "id" })]
        public async Task<ActionResult> details(long id)
        {
            long article_snow_id = id;
            var article = await _context.article.Where(m => m.article_snow_id == article_snow_id).Include(m => m.column).FirstOrDefaultAsync();
            //var article = _service.get_article_details(article_snow_id);
            if (article == null)
            {
                ViewData["msg"] = "文章不存在";
                return View("_notice");
            }
            if (article == null)
            {
                ViewData["msg"] = "文章未审核";
                return View("_notice");
            }
            if (!string.IsNullOrEmpty(article.details_view_path))
            {
                return View(article.details_view_path);
            }
            //如果栏目是动态浏览，且HTML模板不为空，且路径生成规则不为空，则生成HTML页面并跳转
            if (article.column.list_option != list_option_enum.使用动态页 && !string.IsNullOrEmpty(article.column.html_template) && !string.IsNullOrEmpty(article.column.html_path_rule))
            {
                var chtml = new create_html_bll(_context);
                chtml.create_html(article.id, article.column.html_template, article.column.html_path_rule);
                return Redirect(article.to_html_url());
            }

            //如果文章所属栏目内容页视图不为空则显示自己定义视图
            if (!string.IsNullOrEmpty(article.column.details_view_path))
            {
                return View(article.column.details_view_path);
            }
            return View();
        }
    }
}
