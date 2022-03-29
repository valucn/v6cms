using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 领导信箱控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "leader_mailbox/index")]
    public class leader_mailboxController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public leader_mailboxController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 领导信箱列表
        /// </summary>
        [admin_role_filter(authority_code = "leader_mailbox/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.leader_mailbox.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).Include(m => m.user).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加领导信箱
        /// </summary>
        [admin_role_filter(authority_code = "leader_mailbox/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View();
        }

        /// <summary>
        /// 添加领导信箱POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "leader_mailbox/create")]
        public async Task<IActionResult> create(leader_mailbox_entity model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(index));
            }
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 修改领导信箱
        /// </summary>
        [admin_role_filter(authority_code = "leader_mailbox/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.leader_mailbox.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 修改领导信箱POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "leader_mailbox/edit")]
        public async Task<IActionResult> edit(int id, leader_mailbox_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(index));
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 回复领导信箱
        /// </summary>
        [admin_role_filter(authority_code = "leader_mailbox/reply")]
        public async Task<IActionResult> reply(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.leader_mailbox.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 回复领导信箱POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "leader_mailbox/reply")]
        public async Task<IActionResult> reply(int id, leader_mailbox_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.leader_mailbox.Where(m => m.id == id).UpdateFromQueryAsync(x => new leader_mailbox_entity
                    {
                        reply_content = model.reply_content,
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
                return RedirectToAction(nameof(index));
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 删除领导信箱
        /// </summary>
        [admin_role_filter(authority_code = "leader_mailbox/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.leader_mailbox.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.leader_mailbox.Any(m => m.id == id);
        }
    }
}
