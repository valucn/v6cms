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
    /// 值班表配置控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "duty_config/index")]
    public class duty_configController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public duty_configController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 值班表配置列表
        /// </summary>
        [admin_role_filter(authority_code = "duty_config/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.duty_config.AsQueryable();
            var model = await query.ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加值班表配置
        /// </summary>
        [admin_role_filter(authority_code = "duty_config/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View();
        }

        /// <summary>
        /// 添加值班表配置POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "duty_config/create")]
        public async Task<IActionResult> create(duty_config_entity model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                //删除缓存
                _cache.Remove("get_today_duty_list");
                return RedirectToAction(nameof(index));
            }
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 修改值班表配置
        /// </summary>
        [admin_role_filter(authority_code = "duty_config/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.duty_config.FindAsync(id);
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
        /// 修改值班表配置POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "duty_config/edit")]
        public async Task<IActionResult> edit(int id, duty_config_entity model)
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
                    //删除缓存
                    _cache.Remove("get_today_duty_list");
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
        /// 删除值班表配置
        /// </summary>
        [admin_role_filter(authority_code = "duty_config/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.duty_config.Where(m => m.id == id).DeleteFromQueryAsync();
            //删除缓存
            _cache.Remove("get_today_duty_list");
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.duty_config.Any(m => m.id == id);
        }
    }
}
