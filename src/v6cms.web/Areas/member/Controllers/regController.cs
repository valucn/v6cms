using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
    /// 会员注册
    /// </summary>
    [Area("member")]
    public class regController : Controller
    {
        private readonly db_context _context;

        public regController(db_context context)
        {
            _context = context;
        }

        public IActionResult index(string return_url)
        {
            ViewData["return_url"] = return_url;
            return View();
        }

        /// <summary>
        /// AJAX请求注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> index(member_entity model)
        {
            var resp = new global_response();
            string password = V6.sha1(model.password);
            if (await _context.member.AnyAsync(m => m.username == model.username))
            {
                resp.code = 200;
                resp.msg = "用户名已存在";
                resp.success = false;
                return Json(resp);
            }

            HttpContextAccessor context = new HttpContextAccessor();
            string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            var member = new member_entity
            {
                username = model.username,
                password = password,
                nick_name = model.nick_name,
                company = model.company,
                mobile = model.mobile,
                reg_time = DateTime.Now,
                reg_ip = ip
            };
            _context.member.Add(member);
            await _context.SaveChangesAsync();
            //这里的scheme一定要和注入服务的scheme一样
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.username));
            identity.AddClaim(new Claim("member_id", member.id.ToString()));
            //identity.AddClaim(new Claim("member_real_name", member.real_name));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            var ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(45);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,//表示 cookie 是否是持久化的以便它在不同的 request 之间传送。设置了ExpireTimeSpan或ExpiresUtc是必须的。
                AllowRefresh = true,//Refreshing the authentication session should be allowed.
                ExpiresUtc = ExpiresUtc
            };
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            resp.code = 200;
            resp.msg = "注册成功";
            resp.success = true;
            return Json(resp);
        }
    }
}
