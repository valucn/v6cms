using v6cms.entities.db_set;
using v6cms.entities.sys;
using v6cms.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace v6cms.web.Areas.v6admin.Controllers.sys
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "user_manager/index")]
    public class user_managerController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public user_managerController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task get_role()
        {
            var query = await _context.user_role.ToListAsync();
            var role_list = query.Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.role_name
            }).ToList();
            ViewData["role_list"] = role_list;
        }

        [admin_role_filter(authority_code = "user_manager/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.user.AsQueryable();
            var model = await query.Include(m => m.user_role).OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        [admin_role_filter(authority_code = "user_manager/create")]
        public async Task<IActionResult> create()
        {
            await get_role();
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var model = new user_entity
            {
                sort_rank = 50,
            };
            return View(model);
        }

        [admin_role_filter(authority_code = "user_manager/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(user_entity model)
        {
            await get_role();

            if (ModelState.IsValid)
            {
                model.password = V6.sha1(model.password);
                model.create_time = DateTime.Now;
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(index));
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        [admin_role_filter(authority_code = "user_manager/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            await get_role();

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.user.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "user_manager/edit")]
        public async Task<IActionResult> edit(int id, user_entity model)
        {
            await get_role();
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.user.Where(m => m.id == id).UpdateFromQueryAsync(m => new user_entity
                    {
                        role_id = model.role_id,
                        dept_id = model.dept_id,
                        username = model.username,
                        real_name = model.real_name,
                        avatar = model.avatar,
                        post = model.post,
                        intro = model.intro,
                        mobile = model.mobile,
                        card_id = model.card_id,
                        date_of_birth = model.date_of_birth,
                        is_lock = model.is_lock,
                        is_leader_mailbox = model.is_leader_mailbox,
                        sort_rank = model.sort_rank
                    });

                    //删除缓存
                    _cache.Remove("user_birthday_today_cache");
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
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "user_manager/edit_password")]
        public async Task<IActionResult> edit_password(int? id)
        {
            await get_role();

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.user.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            model.is_need_edit_password = true;

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 修改密码POST
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "user_manager/edit_password")]
        public async Task<IActionResult> edit_password(int id, user_entity model)
        {
            await get_role();
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string password = V6.sha1(model.password);
                    await _context.user.Where(m => m.id == id).UpdateFromQueryAsync(m => new user_entity
                    {
                        password = password,
                        is_need_edit_password = model.is_need_edit_password
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

        [admin_role_filter(authority_code = "user_manager/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.user.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.user.Any(m => m.id == id);
        }
    }
}
