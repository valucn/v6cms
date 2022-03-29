using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 广告控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "advertisement/index")]
    public class advertisementController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public advertisementController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 广告列表
        /// </summary>
        [admin_role_filter(authority_code = "advertisement/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.advertisement.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).Include(m => m.pic_list).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加广告
        /// </summary>
        [admin_role_filter(authority_code = "advertisement/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View();
        }

        /// <summary>
        /// 添加广告POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "advertisement/create")]
        public async Task<IActionResult> create(advertisement_entity model)
        {
            if (ModelState.IsValid)
            {
                model.create_time = DateTime.Now;
                _context.Add(model);
                await _context.SaveChangesAsync();

                //删除缓存
                _cache.Remove($"advertisement_details_{model.id}_cache");
                _cache.Remove($"advertisement_list_{model.ad_type}_cache");
                return RedirectToAction(nameof(index));
            }
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 修改广告
        /// </summary>
        [admin_role_filter(authority_code = "advertisement/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.advertisement.Where(m => m.id == id).Include(m => m.pic_list).FirstOrDefaultAsync();
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
        /// 修改广告POST
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "advertisement/edit")]
        public async Task<IActionResult> edit(int id, advertisement_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.advertisement.Where(m => m.id == id).UpdateFromQueryAsync(x => new advertisement_entity
                    {
                        ad_name = model.ad_name,
                        ad_type = model.ad_type,
                        view_time_limit = model.view_time_limit,
                        text = model.text,
                        url = model.url,
                        pic = model.pic,
                        end_time = model.end_time
                    });

                    //删除缓存
                    _cache.Remove($"advertisement_details_{model.id}_cache");
                    _cache.Remove($"advertisement_list_{model.ad_type}_cache");
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
        /// 删除广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "advertisement/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            var model = await _context.advertisement.FirstOrDefaultAsync(m => m.id == id);
            //删除缓存
            _cache.Remove($"advertisement_details_{model.id}_cache");
            _cache.Remove($"advertisement_list_{model.ad_type}_cache");

            await _context.advertisement.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        /// <summary>
        /// 删除广告图片集合
        /// </summary>
        /// <param name="pic_list_id"></param>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "advertisement/delete", is_ajax = true)]
        public async Task<JsonResult> delete_pic_list(int pic_list_id, int ad_id)
        {
            var model = await _context.advertisement.FirstOrDefaultAsync(m => m.id == ad_id);
            //删除缓存
            _cache.Remove($"advertisement_details_{model.id}_cache");
            _cache.Remove($"advertisement_list_{model.ad_type}_cache");

            await _context.advertisement_pic_list.Where(m => m.id == pic_list_id && m.ad_id == ad_id).DeleteFromQueryAsync();
            var resp = new global_response
            {
                code = 200,
                msg = "删除成功"
            };
            return Json(resp);
        }

        private bool exists(int id)
        {
            return _context.advertisement.Any(m => m.id == id);
        }
    }
}
