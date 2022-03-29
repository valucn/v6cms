using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Web;
using v6cms.entities.db_set;

namespace v6cms.web
{
    /// <summary>
    /// 会员过滤器
    /// </summary>
    public class member_role_filter : ActionFilterAttribute
    {
        /// <summary>
        /// 是否ajax请求
        /// </summary>
        public bool is_ajax { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string member_id = filterContext.HttpContext.User.FindFirstValue("member_id");
            if (string.IsNullOrEmpty(member_id))
            {
                if (is_ajax)
                {
                    filterContext.Result = new JsonResult(new { code = 4031, msg = "请登录" });
                }
                else
                {
                    filterContext.Result = new RedirectResult($"/member/login?return_url={filterContext.HttpContext.Request.Path}");
                }
                return;
            }

        }
    }
}
