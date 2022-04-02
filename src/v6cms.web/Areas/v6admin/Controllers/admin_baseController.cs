using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using v6cms.entities.db_set;
using v6cms.services;

namespace v6cms.web.Areas.v6admin.Controllers
{
    public class admin_baseController : Controller
    {
        private readonly db_context _context;
        private readonly IDataService _service;

        public admin_baseController(db_context context, IDataService service)
        {
            _context = context;
            _service = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 获取母版页中数据赋值到ViewBag中
            //string last_visit_url = HttpContext.Request.GetDisplayUrl();
            //HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            var config = _service.get_site_config();
            ViewData["site_name"] = config.site_name;
        }
    }
}
