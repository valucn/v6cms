using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 值班控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "duty_manager/index")]
    public class duty_managerController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public duty_managerController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 值班列表
        /// </summary>
        [admin_role_filter(authority_code = "duty_manager/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.duty.AsQueryable();
            var model = await query.OrderByDescending(m => m.date).ThenBy(m => m.id).ToPagedListAsync(page, 15);

            //读取值班配置
            var configs = _context.duty_config;
            foreach (var item in configs)
            {
                ViewData[item.column_no] = item.display_name;
            }

            return View(model);
        }

        /// <summary>
        /// 添加值班
        /// </summary>
        [admin_role_filter(authority_code = "duty_manager/create")]
        public async Task<IActionResult> create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            //读取值班配置
            var configs = await _context.duty_config.ToListAsync();
            foreach (var item in configs)
            {
                ViewData[item.column_no] = item.display_name;
            }

            return View();
        }

        /// <summary>
        /// 添加值班POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "duty_manager/create")]
        public async Task<IActionResult> create(duty_entity model)
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

            //读取值班配置
            var configs = _context.duty_config;
            foreach (var item in configs)
            {
                ViewData[item.column_no] = item.display_name;
            }
            return View(model);
        }

        /// <summary>
        /// 修改值班
        /// </summary>
        [admin_role_filter(authority_code = "duty_manager/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.duty.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            //读取值班配置
            var configs = _context.duty_config;
            foreach (var item in configs)
            {
                ViewData[item.column_no] = item.display_name;
            }
            return View(model);
        }

        /// <summary>
        /// 修改值班POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "duty_manager/edit")]
        public async Task<IActionResult> edit(int id, duty_entity model)
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
                    //删除缓存
                    _cache.Remove("get_today_duty_list");
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

            //读取值班配置
            var configs = _context.duty_config;
            foreach (var item in configs)
            {
                ViewData[item.column_no] = item.display_name;
            }
            return View(model);
        }

        /// <summary>
        /// 删除值班
        /// </summary>
        [admin_role_filter(authority_code = "duty_manager/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.duty.Where(m => m.id == id).DeleteFromQueryAsync();
            //删除缓存
            _cache.Remove("get_today_duty_list");
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.duty.Any(m => m.id == id);
        }
    }
}
