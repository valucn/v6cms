using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 链接分类管理
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "link_category/index")]
    public class link_categoryController : Controller
    {
        private readonly db_context _context;

        /// <summary>
        /// 链接分类管理
        /// </summary>
        /// <param name="context"></param>
        public link_categoryController(db_context context)
        {
            _context = context;
        }

        [admin_role_filter(authority_code = "link_category/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            int page_size = 15;
            return View(await _context.link_category.OrderBy(m => m.sort_rank).ToPagedListAsync(page, page_size));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "link_category/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var model = new link_category_entity
            {
                sort_rank = 50
            };
            return View(model);
        }

        /// <summary>
        /// 提交创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "link_category/create")]
        public async Task<IActionResult> create(link_category_entity model)
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

        [admin_role_filter(authority_code = "link_category/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link_category_entity = await _context.link_category.FindAsync(id);
            if (link_category_entity == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(link_category_entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "link_category/edit")]
        public async Task<IActionResult> edit(int id, link_category_entity model)
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
                    if (!link_category_entityExists(model.id))
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

        [admin_role_filter(authority_code = "link_category/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            var model = await _context.link_category.FindAsync(id);
            _context.link_category.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(index));
        }

        private bool link_category_entityExists(int id)
        {
            return _context.link_category.Any(e => e.id == id);
        }
    }
}
