using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 答疑回复控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "ask_reply/index")]
    public class ask_replyController : Controller
    {
        private readonly db_context _context;

        public ask_replyController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 添加答疑回复
        /// </summary>
        [admin_role_filter(authority_code = "ask_reply/create")]
        public async Task<IActionResult> create(int ask_id, string ask_title)
        {
            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            ViewData["ask_title"] = ask_title;
            return View();
        }

        /// <summary>
        /// 添加答疑回复POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "ask_reply/create")]
        public async Task<IActionResult> create(ask_reply_entity model)
        {
            if (ModelState.IsValid)
            {
                HttpContextAccessor context = new HttpContextAccessor();
                string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
                string user_id_str = HttpContext.User.FindFirstValue("user_id");
                int user_id = int.Parse(user_id_str);

                model.user_id = user_id;
                model.reply_time = DateTime.Now;
                model.ip = ip;
                _context.Add(model);
                await _context.SaveChangesAsync();

                // 自动审核提问
                await _context.ask.Where(m => m.id == model.ask_id).UpdateFromQueryAsync(x => new ask_entity
                {
                    is_review = true,
                    reply_status = entities.enums.reply_status_enum.新回复
                });
                //跳转到提问列表页
                return RedirectToAction("index", "ask");
            }

            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());
            return View(model);
        }

        /// <summary>
        /// 修改答疑回复
        /// </summary>
        [admin_role_filter(authority_code = "ask_reply/edit")]
        public async Task<IActionResult> edit(int? id, string ask_title)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.ask_reply.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            ViewData["ask_title"] = ask_title;
            return View(model);
        }

        /// <summary>
        /// 修改答疑回复POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "ask_reply/edit")]
        public async Task<IActionResult> edit(int id, ask_reply_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.ask_reply.Where(m => m.id == id).UpdateFromQueryAsync(x => new ask_reply_entity
                    {
                        content = model.content,
                        reply_time = DateTime.Now
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!exists(model.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //跳转到提问列表页
                return RedirectToAction("index", "ask");
            }

            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());
            return View(model);
        }

        private bool exists(int id)
        {
            return _context.ask_reply.Any(m => m.id == id);
        }
    }
}
