using v6cms.entities.db_set;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using v6cms.entities;
using Microsoft.EntityFrameworkCore;
using Webdiyer.AspNetCore;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 领导信箱控制器
    /// </summary>
    public class leader_mailboxController : v6baseController
    {
        private readonly db_context _context;

        public leader_mailboxController(db_context context)
        {
            _context = context;
        }

        public async Task<ActionResult> index()
        {
            var model = await _context.user.Where(m => m.is_leader_mailbox).OrderBy(m => m.sort_rank)
                .ThenByDescending(m => m.id).ToListAsync();
            return View(model);
        }

        /// <summary>
        /// 信件列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> mail_list(int id, int page = 1)
        {
            int page_size = 15;
            var model = await _context.leader_mailbox.Where(m => m.user_id == id).OrderByDescending(m => m.id).ToPagedListAsync(page, page_size);
            return View(model);
        }

        /// <summary>
        /// 写信
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> write(int id)
        {
            var user = await _context.user.FirstOrDefaultAsync(m => m.is_leader_mailbox && m.id == id);
            if (user == null)
            {
                ViewData["message"] = "领导信箱不存在";
                return View("_notice");
            }
            return View();
        }

        /// <summary>
        /// 写信POST
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> write(int id, leader_mailbox_entity model)
        {
            if (ModelState.IsValid)
            {
                _context.leader_mailbox.Add(new leader_mailbox_entity
                {
                    user_id = id,
                    title = model.title,
                    guest_name = model.guest_name,
                    guest_unit = model.guest_unit,
                    guest_email = model.guest_email,
                    guest_mobile = model.guest_mobile,
                    mail_type = model.mail_type,
                    reply_type = model.reply_type,
                    content = model.content,
                    create_time = DateTime.Now
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("mail_list", new { id = id });
            }
            var user = _context.user.FirstOrDefault(m => m.is_leader_mailbox && m.id == id);
            if (user == null)
            {
                ViewData["message"] = "领导信箱不存在";
                return View("_notice");
            }
            return View();
        }
    }
}
