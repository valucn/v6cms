using v6cms.entities;
using v6cms.entities.db_set;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 生日名单控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "birthday_list/index")]
    public class birthday_listController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public birthday_listController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 生日名单列表
        /// </summary>
        [admin_role_filter(authority_code = "birthday_list/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.birthday_list.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加生日名单
        /// </summary>
        [admin_role_filter(authority_code = "birthday_list/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View();
        }

        /// <summary>
        /// 添加生日名单POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "birthday_list/create")]
        public async Task<IActionResult> create(birthday_list_entity model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                _cache.Remove("today_birthday_cache");//删除缓存
                return RedirectToAction(nameof(index));
            }
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 修改生日名单
        /// </summary>
        [admin_role_filter(authority_code = "birthday_list/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.birthday_list.FindAsync(id);
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
        /// 修改生日名单POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "birthday_list/edit")]
        public async Task<IActionResult> edit(int id, birthday_list_entity model)
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

                    _cache.Remove("today_birthday_cache");//删除缓存
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
        /// 删除生日名单
        /// </summary>
        [admin_role_filter(authority_code = "birthday_list/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.birthday_list.Where(m => m.id == id).DeleteFromQueryAsync();
            _cache.Remove("today_birthday_cache");//删除缓存

            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.birthday_list.Any(m => m.id == id);
        }
    }
}
