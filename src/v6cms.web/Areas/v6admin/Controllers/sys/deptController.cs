using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using v6cms.entities.db_set;
using v6cms.entities.sys;

namespace v6cms.web.Areas.v6admin.Controllers.sys
{
    /// <summary>
    /// 部门控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "dept/index")]
    public class deptController : Controller
    {
        private readonly db_context _context;

        public deptController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取部门列表下拉框
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        private async Task get_dept_list(int parent_id = 0)
        {
            var list = await get_dept(parent_id);
            var dept_list = new SelectList(list, "id", "dept_name");
            ViewData["dept_list"] = dept_list;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        private async Task<IEnumerable<dept_entity>> get_dept(int parent_id = 0)
        {
            var list = await _context.dept.Where(m => m.parent_id == parent_id).ToListAsync();
            foreach (var item in list)
            {
                if (parent_id > 0)
                {
                    item.dept_name = $"——{item.dept_name}";
                }
                var li = await get_dept(item.id);
                list = list.Concat(li).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取祖级列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<string> get_ancestors_str(int id = 0)
        {
            string ancestors = await get_ancestors(id);
            return ancestors;
        }

        private async Task<string> get_ancestors(int id = 0)
        {
            var dept = await _context.dept.Where(m => m.id == id).FirstOrDefaultAsync();
            string ancestors = $"{dept.ancestors},{dept.id}";
            return ancestors;
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        [admin_role_filter(authority_code = "dept/index")]
        public IActionResult index(int page = 1)
        {
            //var query = _context.dept.AsQueryable();
            //var model = await query.OrderBy(m => m.order_num).ThenByDescending(m => m.id).ToPagedListAsync(page, 15);
            //return View(model);
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View();
        }

        /// <summary>
        /// AJAX获取列表
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "dept/index", is_ajax = true)]
        public async Task<IActionResult> get_json_list()
        {
            var depts = await _context.dept.OrderBy(d => d.sort_rank).ToListAsync();
            var response = new
            {
                code = 200,
                message = "success",
                data = depts
            };
            return Json(depts);
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        [admin_role_filter(authority_code = "dept/create")]
        public async Task<IActionResult> create()
        {
            await get_dept_list();

            var model = new dept_entity
            {
                sort_rank = 50
            };
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 添加部门POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "dept/create")]
        public async Task<IActionResult> create(dept_entity model)
        {
            if (ModelState.IsValid)
            {
                string user_real_name = HttpContext.User.FindFirstValue("user_real_name");
                model.ancestors = await get_ancestors_str(model.parent_id);
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
        /// 修改部门
        /// </summary>
        [admin_role_filter(authority_code = "dept/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.dept.FindAsync(id);
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
        /// 修改部门POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "dept/edit")]
        public async Task<IActionResult> edit(int id, dept_entity model)
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
        /// 设置栏目权限
        /// </summary>
        [admin_role_filter(authority_code = "dept/column_permissions")]
        public async Task<IActionResult> column_permissions(int id)
        {
            var model = await _context.dept.FindAsync(id);
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
        /// 设置栏目权限POST
        /// </summary>
        [HttpPost]
        [admin_role_filter(authority_code = "dept/column_permissions")]
        public async Task<IActionResult> column_permissions(int id, dept_entity model)
        {
            if (ModelState.IsValid)
            {
                await _context.dept.Where(m => m.id == id).UpdateFromQueryAsync(x => new dept_entity
                {
                    column_id_list = model.column_id_list
                });
                return RedirectToAction(nameof(index));
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View(model);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        [admin_role_filter(authority_code = "dept/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            if (id == 2)
            {
                return Redirect($"/notice/fail?message={HttpUtility.UrlEncode("根部门不能删除")}&jump_url=/dept");
            }
            if (count(id.Value) > 0)
            {
                return Redirect($"/notice/fail?message={HttpUtility.UrlEncode("该部门下有子部门，不能删除")}&jump_url=/dept");
            }
            await _context.dept.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private int count(int id)
        {
            return _context.dept.Where(m => m.parent_id == id).Count();
        }

        private bool exists(int id)
        {
            return _context.dept.Any(m => m.id == id);
        }
    }
}
