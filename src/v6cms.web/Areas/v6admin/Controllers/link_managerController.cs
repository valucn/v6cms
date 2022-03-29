using v6cms.entities;
using v6cms.entities.db_set;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 链接管理
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "link_manager/index")]
    public class link_managerController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        /// <summary>
        /// 链接管理
        /// </summary>
        /// <param name="context"></param>
        public link_managerController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        private async Task get_category()
        {
            var list = await _context.link_category.ToListAsync();
            ViewData["category_id"] = new SelectList(list, "id", "category_name");
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "link_manager/index")]
        public async Task<IActionResult> index(string keyword, int? category_id, int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            ViewData["keyword"] = keyword;
            ViewData["category_id"] = category_id;
            await get_category();

            var query = _context.link.AsQueryable();
            if (category_id.HasValue)
            {
                query = query.Where(m => m.category_id == category_id);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(m => m.title.Contains(keyword) || m.logo.Contains(keyword) || m.url.Contains(keyword));
            }
            var model = await query.OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加链接
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "link_manager/create")]
        public async Task<IActionResult> create()
        {
            var model = new link_entity
            {
                font_weight = "",
                sort_rank = 50
            };
            await get_category();

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 添加链接POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "link_manager/create")]
        public async Task<IActionResult> create(link_entity model)
        {
            if (ModelState.IsValid)
            {
                if (model.font_color != null && model.font_color.Equals("#000000"))
                {
                    model.font_color = "";
                }
                if (model.font_weight == null)
                {
                    model.font_weight = "";
                }
                _context.Add(model);
                await _context.SaveChangesAsync();

                //删除缓存
                _cache.Remove($"link_list_{model.category_id}_cache");
                return RedirectToAction(nameof(index));
            }
            await get_category();

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "link_manager/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link_entity = await _context.link.FindAsync(id);
            if (link_entity == null)
            {
                return NotFound();
            }
            await get_category();

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(link_entity);
        }

        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "link_manager/edit")]
        public async Task<IActionResult> edit(int id, link_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.font_color != null && model.font_color.Equals("#000000"))
                    {
                        model.font_color = "";
                    }
                    if (model.font_weight == null)
                    {
                        model.font_weight = "";
                    }
                    await _context.link.Where(m => m.id == id).UpdateFromQueryAsync(x => new link_entity
                    {
                        category_id = model.category_id,
                        title = model.title,
                        logo = model.logo,
                        url = model.url,
                        sort_rank = model.sort_rank,
                        font_weight = model.font_weight,
                        font_color = model.font_color
                    });
                    await _context.SaveChangesAsync();
                    //删除缓存
                    _cache.Remove($"link_list_{model.category_id}_cache");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!link_entityExists(model.id))
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
            await get_category();

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "link_manager/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            var model = await _context.link.FindAsync(id);
            _context.link.Remove(model);
            await _context.SaveChangesAsync();
            //删除缓存
            _cache.Remove($"link_list_{model.category_id}_cache");
            return RedirectToAction(nameof(index));
        }

        private bool link_entityExists(int id)
        {
            return _context.link.Any(e => e.id == id);
        }
    }
}
