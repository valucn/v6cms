using v6cms.entities.db_set;
using v6cms.utils;
using v6cms.utils.premission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace v6cms.web
{
    public class admin_role_filter : ActionFilterAttribute
    {
        /// <summary>
        /// 角色权限编码
        /// </summary>
        public string authority_code { get; set; }

        /// <summary>
        /// 是否ajax请求
        /// </summary>
        public bool is_ajax { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool IsAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            string user_id = filterContext.HttpContext.User.FindFirstValue("user_id");
            //Console.WriteLine($"user_id={user_id}");
            string user_role_id = filterContext.HttpContext.User.FindFirstValue("user_role_id");
            if (!IsAuthenticated || string.IsNullOrEmpty(user_id))
            {
                if (is_ajax)
                {
                    filterContext.Result = new JsonResult(new { code = 4031, msg = "请登录" });
                }
                else
                {
                    filterContext.Result = new RedirectResult($"/v6admin/login?return_url={filterContext.HttpContext.Request.Path}");
                }
                return;
            }

            var _context = filterContext.HttpContext.RequestServices.GetService(typeof(db_context)) as db_context;

            string controller = filterContext.HttpContext.Request.RouteValues["controller"].ToString();
            //Console.WriteLine($"controller={controller}");
            if (controller != "edit_my_password")
            {
                //如果请求的不是修改自己密码的控制器则提醒用户必须修改密码
                var user = _context.user.Where(m => m.id.ToString() == user_id).FirstOrDefault();
                //Console.WriteLine($"is_need_edit_password={user.is_need_edit_password}");
                if (user.is_need_edit_password)
                {
                    if (is_ajax)
                    {
                        filterContext.Result = new JsonResult(new { code = 4033, msg = $"必须先修改密码" });
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult($"/v6admin/notice/fail?message={HttpUtility.UrlEncode("必须先修改密码")}&jump_url=/v6admin/edit_my_password");
                    }
                    return;
                }
            }

            //Console.WriteLine($"authority_cod={authority_code}");
            if (!string.IsNullOrEmpty(authority_code))
            {
                var user_role = _context.user_role.Where(m => m.id.ToString() == user_role_id).FirstOrDefault();
                //Console.WriteLine($"user_role.authority_code_list={user_role.authority_code_list}");
                var authority_code_list = string_util.string2list(user_role.authority_code_list);
                if (!auth_premissions.has_permissions(authority_code_list, authority_code))
                {
                    if (is_ajax)
                    {
                        filterContext.Result = new JsonResult(new { code = 4032, msg = $"无权限：{authority_code}" });
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult($"/v6admin/notice/fail?message={HttpUtility.UrlEncode("无权限")}&authority_code={authority_code}");
                    }
                    return;
                }
            }
        }
    }
}
