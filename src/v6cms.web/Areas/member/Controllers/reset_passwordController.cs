using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;
using v6cms.utils;

namespace v6cms.web.Areas.member.Controllers
{
    /// <summary>
    /// 重置密码控制器
    /// </summary>
    [Area("member")]
    public class reset_passwordController : Controller
    {
        private readonly db_context _context;

        public reset_passwordController(db_context context)
        {
            _context = context;
        }

        public IActionResult index()
        {
            return View();
        }

        /// <summary>
        /// AJAX请求重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> index(member_entity model)
        {
            var resp = new global_response();
            var exists = await _context.member.AnyAsync(m => m.username == model.username);
            if (!exists)
            {
                resp.code = 200;
                resp.msg = "用户名不存在";
                resp.success = false;
                return Json(resp);
            }

            var exists2 = await _context.member.AnyAsync(m => m.username == model.username
                && m.member_question1 == model.member_question1 && m.member_answer1 == model.member_answer1);
            if (!exists2)
            {
                resp.code = 200;
                resp.msg = "密保问题和答案不匹配";
                resp.success = false;
                return Json(resp);
            }
            else
            {
                TempData["reset_password"] = model.username;
                //HttpContext.Session.SetString("reset_password", "true");

                resp.code = 200;
                resp.msg = "验证成功";
                resp.success = true;
                return Json(resp);
            }
        }

        /// <summary>
        /// 密保问题验证成功第2步
        /// </summary>
        /// <returns></returns>
        public IActionResult step2()
        {
            var reset_password = TempData["reset_password"];
            //var reset_password = HttpContext.Session.GetString("reset_password");
            if (reset_password == null)
            {
                return RedirectToAction("index");
            }
            else
            {
                TempData["username"] = reset_password.ToString();
            }

            return View();
        }

        /// <summary>
        /// 重置密码POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> step2(member_entity model)
        {
            var _username = TempData["username"];
            if (_username == null)
            {
                return RedirectToAction("index");
            }
            string username = _username.ToString();
            string password_sha1 = V6.sha1(model.password);
            await _context.member.Where(m => m.username == username).UpdateFromQueryAsync(x => new member_entity
            {
                password = password_sha1
            });
            var success = new global_response
            {
                code = 200,
                msg = "重新成功",
                success = true
            };
            return Json(success);
        }

    }
}
