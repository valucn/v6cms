using v6cms.entities;
using v6cms.entities.db_set;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Http.Extensions;
using v6cms.entities.enums;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 网站栏目控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "column/index")]
    public class columnController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public columnController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 网站栏目列表
        /// </summary>
        [admin_role_filter(authority_code = "column/index")]
        public IActionResult index()
        {
            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            return View();
        }

        /// <summary>
        /// 搜索栏目
        /// </summary>
        [admin_role_filter(authority_code = "column/index")]
        public async Task<IActionResult> search(string keyword, int page = 1)
        {
            ViewData["keyword"] = keyword;

            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());
            var query = _context.column.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(m => m.column_name.Contains(keyword) || m.column_name_abbr.Contains(keyword)
                || m.intro.Contains(keyword) || m.route_value.Contains(keyword) || m.html_path_rule.Contains(keyword)
                || m.html_template.Contains(keyword) || m.external_link.Contains(keyword));
            }
            int page_size = 15;
            var model = await query.OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToPagedListAsync(page, page_size);
            return View(model);
        }

        /// <summary>
        /// 网站栏目列表
        /// </summary>
        [admin_role_filter(authority_code = "column/index", is_ajax = true)]
        public async Task<JsonResult> get_json_list()
        {
            var query = _context.column.AsQueryable();
            var model = await query.OrderBy(m => m.sort_rank).ThenByDescending(m => m.id).ToListAsync();
            return Json(model);
        }

        /// <summary>
        /// 添加网站栏目
        /// </summary>
        [admin_role_filter(authority_code = "column/create")]
        public async Task<IActionResult> create(int parent_id)
        {
            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            var model = new column_entity
            {
                parent_id = parent_id,
                sort_rank = 50,
                list_option = list_option_enum.使用动态页,
                column_attribute = column_attribute_enum.最终列表栏目,
                target = ""
            };

            var parent = await _context.column.Where(m => m.id == parent_id).FirstOrDefaultAsync();
            if (parent != null)
            {
                ViewData["parent_column_name"] = parent.column_name;
            }
            else
            {
                ViewData["parent_column_name"] = "根栏目";
            }
            return View(model);
        }

        /// <summary>
        /// 添加网站栏目POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "column/create")]
        public async Task<IActionResult> create(column_entity model)
        {
            if (ModelState.IsValid)
            {
                if (model.parent_id == 0)
                {
                    model.level = 1;
                }
                else
                {
                    model.level = 2;
                }
                model.column_name = model.column_name.Trim();
                model.column_name_abbr = string.IsNullOrEmpty(model.column_name_abbr) ? "" : model.column_name_abbr.Trim();
                model.create_time = DateTime.Now;
                _context.Add(model);
                await _context.SaveChangesAsync();

                //删除相关缓存
                _cache.Remove("column_nav_cache");
                _cache.Remove("sys_api_column_list_json_True_True");
                _cache.Remove("sys_api_column_list_json_True_False");
                _cache.Remove("sys_api_column_list_json_False_True");
                _cache.Remove("sys_api_column_list_json_False_False");
                _cache.Remove($"column_child_list_{model.parent_id}_cache");
                return RedirectToAction(nameof(index));
            }

            //获取上次访问页面
            string last_visit_url = Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 修改网站栏目
        /// </summary>
        [admin_role_filter(authority_code = "column/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.column.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            var parent = _context.column.Where(m => m.id == model.parent_id).FirstOrDefault();
            if (parent != null)
            {
                ViewData["parent_column_name"] = parent.column_name;
            }
            else
            {
                ViewData["parent_column_name"] = "根栏目";
            }
            return View(model);
        }

        /// <summary>
        /// 修改网站栏目POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "column/edit")]
        public async Task<IActionResult> edit(int id, column_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.parent_id == 0)
                    {
                        model.level = 1;
                    }
                    else
                    {
                        model.level = 2;
                    }
                    await _context.column.Where(m => m.id == id).UpdateFromQueryAsync(x => new column_entity
                    {
                        parent_id = model.parent_id,
                        level = model.level,
                        column_name = model.column_name.Trim(),
                        column_name_abbr = string.IsNullOrEmpty(model.column_name_abbr) ? "" : model.column_name_abbr.Trim(),
                        intro = model.intro,
                        content = model.content,
                        pic = model.pic,
                        route_value = model.route_value,
                        article_route = model.article_route,
                        list_view_path = model.list_view_path,
                        details_view_path = model.details_view_path,
                        html_template = model.html_template,
                        html_path_rule = model.html_path_rule,
                        sort_rank = model.sort_rank,
                        score = model.score,
                        score_gab = model.score_gab,
                        score_province = model.score_province,
                        score_city = model.score_city,
                        score_branch = model.score_branch,
                        is_recommend = model.is_recommend,
                        is_show_nav = model.is_show_nav,
                        is_need_review = model.is_need_review,
                        is_limit_ip = model.is_limit_ip,
                        list_option = model.list_option,
                        column_attribute = model.column_attribute,
                        external_link = model.external_link,
                        target = model.target,
                        update_time = DateTime.Now
                    });
                    //删除相关缓存
                    _cache.Remove("column_nav_cache");
                    _cache.Remove("sys_api_column_list_json_True_True");
                    _cache.Remove("sys_api_column_list_json_True_False");
                    _cache.Remove("sys_api_column_list_json_False_True");
                    _cache.Remove("sys_api_column_list_json_False_False");
                    _cache.Remove($"column_child_list_{model.parent_id}_cache");
                    _cache.Remove($"column_details_{id}_cache");
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
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            var parent = _context.column.Where(m => m.id == model.parent_id).FirstOrDefault();
            if (parent != null)
            {
                ViewData["parent_column_name"] = parent.column_name;
            }
            else
            {
                ViewData["parent_column_name"] = "根栏目";
            }

            return View(model);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        [admin_role_filter(authority_code = "column/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            var model = await _context.column.Where(m => m.id == id).FirstOrDefaultAsync();
            //删除相关缓存
            _cache.Remove("column_nav_cache");
            _cache.Remove("sys_api_column_list_json_True_True");
            _cache.Remove("sys_api_column_list_json_True_False");
            _cache.Remove("sys_api_column_list_json_False_True");
            _cache.Remove("sys_api_column_list_json_False_False");
            _cache.Remove($"column_details_{id}_cache");
            _cache.Remove($"column_child_list_{model.parent_id}_cache");

            //删除记录
            await _context.column.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.column.Any(m => m.id == id);
        }
    }
}
