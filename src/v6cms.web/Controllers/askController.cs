using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.web.Areas.member.Controllers;
using Webdiyer.AspNetCore;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 答疑控制器
    /// </summary>
    public class askController : member_baseController
    {
        private readonly db_context _context;

        public askController(db_context context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 答疑列表
        /// </summary>
        public async Task<IActionResult> index(int page = 1)
        {
            var query = _context.ask.Where(m => m.is_review).AsQueryable();
            var model = await query.Include(m => m.member).Include(m => m.reply).ThenInclude(m => m.user).OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 提问
        /// </summary>
        [member_role_filter]
        public async Task<IActionResult> new_question()
        {
            string member_id_str = HttpContext.User.FindFirstValue("member_id");
            int member_id = int.Parse(member_id_str);
            var member = await _context.member.Where(m => m.id == member_id).FirstOrDefaultAsync();
            if (member.member_level < member_level_enum.审核会员)
            {
                ViewData["msg"] = "请等待工作人员审核后才能提问";
                return View("_ask_notice");
            }
            return View();
        }

        /// <summary>
        /// 提问POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [member_role_filter]
        public async Task<IActionResult> create(ask_entity model)
        {
            if (ModelState.IsValid)
            {
                string member_id_str = HttpContext.User.FindFirstValue("member_id");
                int member_id = int.Parse(member_id_str);

                HttpContextAccessor context = new HttpContextAccessor();
                string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();

                model.member_id = member_id;
                model.create_time = DateTime.Now;
                model.ip = ip;
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "my_ask", new { area = "member" });
            }
            return View(model);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> details(int id)
        {
            ViewData["ask_id"] = id;

            var model = await _context.ask.Where(m => m.id == id && m.is_review).Include(m => m.member).Include(m => m.reply).ThenInclude(m => m.user).FirstOrDefaultAsync();

            var comment_list = _context.comment.Where(m => m.source_id == id).AsQueryable();
            ViewData["comment_count"] = await comment_list.CountAsync();
            ViewData["comment_list"] = await comment_list.Include(m => m.member).ToListAsync();
            ViewData["up_count"] = await _context.ask_up.CountAsync(m => m.source_id == id);

            ViewData["member_id"] = current_member_id;
            ViewData["member_level"] = 0;


            if (current_member_id > 0)
            {
                var member = await _context.member.Where(m => m.id == current_member_id).FirstOrDefaultAsync();
                if (member != null)
                {
                    ViewData["member_level"] = member.member_level;
                }

                await _context.ask.Where(m => m.id == id).UpdateFromQueryAsync(x => new ask_entity
                {
                    reply_status = reply_status_enum.回复已读
                });
            }

            return View(model);
        }

        /// <summary>
        /// 问答评论
        /// </summary>
        /// <param name="ask_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [member_role_filter]
        public async Task<IActionResult> comment(int ask_id, comment_entity model)
        {
            string member_id_str = HttpContext.User.FindFirstValue("member_id");
            int member_id = int.Parse(member_id_str);
            string member_nick_name = HttpContext.User.FindFirstValue("member_nick_name");

            HttpContextAccessor context = new HttpContextAccessor();
            string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();

            _context.comment.Add(new comment_entity
            {
                module = "ask",
                source_id = ask_id,
                member_id = member_id,
                comment_name = member_nick_name,
                comment_content = model.comment_content,
                create_time = DateTime.Now,
                ip = ip
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("details", new { id = ask_id });
        }
    }
}
