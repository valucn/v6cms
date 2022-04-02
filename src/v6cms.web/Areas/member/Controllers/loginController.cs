using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;
using v6cms.utils;

namespace v6cms.web.Areas.member.Controllers
{
    /// <summary>
    /// 会员登录
    /// </summary>
    [Area("member")]
    public class loginController : Controller
    {
        private readonly db_context _context;

        public loginController(db_context context)
        {
            _context = context;
        }

        public IActionResult index(string return_url)
        {
            ViewData["return_url"] = return_url;
            return View();
        }

        /// <summary>
        /// AJAX请求登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="remember"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> index(member_entity model, string remember)
        {
            var resp = new global_response();
            string password = V6.sha1(model.password);
            var member = await _context.member.FirstOrDefaultAsync(m => m.username == model.username && m.password == password);
            if (member == null)
            {
                resp.code = 200;
                resp.msg = "用户名或密码错误";
                resp.success = false;
                return Json(resp);
            }
            if (member.is_lock)
            {
                resp.code = 200;
                resp.msg = "用户被锁定";
                resp.success = false;
                return Json(resp);
            }
            //这里的scheme一定要和注入服务的scheme一样
            var identity = new ClaimsIdentity("v6cms_member");
            identity.AddClaim(new Claim(ClaimTypes.Name, model.username));
            identity.AddClaim(new Claim("member_id", member.id.ToString()));
            //identity.AddClaim(new Claim("member_nick_name", member.nick_name));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            var ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(45);
            if (remember == "on")
            {
                //记住密码7天
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7);
            }
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,//表示 cookie 是否是持久化的以便它在不同的 request 之间传送。设置了ExpireTimeSpan或ExpiresUtc是必须的。
                AllowRefresh = true,//Refreshing the authentication session should be allowed.
                ExpiresUtc = ExpiresUtc
            };
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            resp.code = 200;
            resp.msg = "登录成功";
            resp.success = true;
            return Json(resp);
        }
    }
}
