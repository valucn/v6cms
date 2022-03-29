using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using v6cms.entities.db_set;
using v6cms.services;

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
        public ActionResult details(long id)
        {
            long article_snow_id = id;
            var article = _service.get_article_details(article_snow_id);
            if (article == null)
            {
                ViewData["msg"] = "文章不存在";
                return View("_notice");
            }
            if (!string.IsNullOrEmpty(article.details_view_path))
            {
                return View(article.details_view_path);
            }

            var column = _service.get_column_details(article.column_id);
            if (column == null)
            {
                ViewData["msg"] = "栏目不存在";
                return View("_notice");
            }
            if (!string.IsNullOrEmpty(column.details_view_path))
            {
                return View(column.details_view_path);
            }
            return View();
        }
    }
}
