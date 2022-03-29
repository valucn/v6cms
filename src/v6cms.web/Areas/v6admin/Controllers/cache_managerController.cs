using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using v6cms.models.api;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 缓存管理控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "cache_manager/index")]
    public class cache_managerController : Controller
    {
        private readonly IMemoryCache _cache;
        public cache_managerController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IActionResult> index(int page = 1)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems != null)
            {
                foreach (DictionaryEntry cacheItem in cacheItems)
                {
                    keys.Add(cacheItem.Key.ToString() + "|" + cacheItem.Value.ToString());
                }
            }
            keys.Sort();
            int page_size = 15;
            var model = await keys.ToPagedListAsync(page, page_size);

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        [admin_role_filter(authority_code = "cache_manager/delete")]
        public IActionResult delete(string key)
        {
            _cache.Remove(key);
            return RedirectToAction(nameof(index));
        }

        /// <summary>
        /// 清空所有缓存（index.cshtml调用）
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "cache_manager/delete", is_ajax = true)]
        public JsonResult clear_all()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            if (cacheItems != null)
            {
                foreach (DictionaryEntry cacheItem in cacheItems)
                {
                    _cache.Remove(cacheItem.Key);
                }
            }
            var resp = new global_response
            {
                code = 200,
                msg = "清空成功"
            };
            return Json(resp);
        }
    }
}
