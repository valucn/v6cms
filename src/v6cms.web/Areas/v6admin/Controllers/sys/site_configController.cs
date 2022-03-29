using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities.db_set;
using v6cms.entities.sys;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6amin.Controllers.sys
{
    /// <summary>
    /// 网站配置控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "site_config/index")]
    public class site_configController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public site_configController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 查看配置
        /// </summary>
        [admin_role_filter(authority_code = "site_config/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.site_config.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 修改配置
        /// </summary>
        [admin_role_filter(authority_code = "site_config/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.site_config.FindAsync(id);
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
        /// 修改配置POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "site_config/edit")]
        public async Task<IActionResult> edit(int id, site_config_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.site_config.Where(m => m.id == id).UpdateFromQueryAsync(x => new site_config_entity
                    {
                        site_name = model.site_name,
                        keywords = model.keywords,
                        description = model.description,
                        site_url = model.site_url,
                        site_color = model.site_color,
                        copyright = model.copyright,
                        icp = model.icp,
                        count_code = model.count_code,
                        comment_top_days = model.comment_top_days
                    });
                    _cache.Remove("site_config");
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

        private bool exists(int id)
        {
            return _context.site_config.Any(m => m.id == id);
        }
    }
}
