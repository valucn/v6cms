using v6cms.entities;
using v6cms.entities.db_set;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// IP地址
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "ip_address/index")]
    public class ip_addressController : Controller
    {
        private readonly db_context _context;

        /// <summary>
        /// IP地址
        /// </summary>
        /// <param name="context"></param>
        public ip_addressController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "ip_address/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            int page_size = 15;
            return View(await _context.ip_address.OrderByDescending(m => m.id).ToPagedListAsync(page, page_size));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "ip_address/create")]
        public IActionResult create()
        {
            var model = new ip_address_entity
            {
                ip_type = entities.enums.ip_type_enum.IP白名单
            };

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 提交创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "ip_address/create")]
        public async Task<IActionResult> create(ip_address_entity model)
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
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "ip_address/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ip_address_entity = await _context.ip_address.FindAsync(id);
            if (ip_address_entity == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(ip_address_entity);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "ip_address/edit")]
        public async Task<IActionResult> edit(int id, ip_address_entity model)
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
                    if (!ip_address_entityExists(model.id))
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

        [admin_role_filter(authority_code = "ip_address/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            var ip_address_entity = await _context.ip_address.FindAsync(id);
            _context.ip_address.Remove(ip_address_entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(index));
        }

        private bool ip_address_entityExists(int id)
        {
            return _context.ip_address.Any(e => e.id == id);
        }
    }
}
