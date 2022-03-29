using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using v6cms.blls;
using v6cms.entities.db_set;
using v6cms.utils;

namespace v6cms.web.Controllers
{
    public abstract class v6baseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 获取母版页中数据赋值到ViewBag中
            var controller = context.Controller as Controller;

            controller.ViewData["current_url"] = HttpContext.Request.GetDisplayUrl();
        }
    }
}
