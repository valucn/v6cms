using v6cms.entities.db_set;
using v6cms.entities.sys;
using v6cms.utils;
using v6cms.utils.premission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6admin.Controllers.sys
{
    /// <summary>
    /// 用户角色控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "user_role/index")]
    public class user_roleController : Controller
    {
        private readonly db_context _context;

        public user_roleController(db_context context)
        {
            _context = context;
        }

        [admin_role_filter(authority_code = "user_role/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            var query = _context.user_role.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        [admin_role_filter(authority_code = "user_role/create")]
        public IActionResult create()
        {
            var model = new user_role_entity
            {
                data_range = entities.enums.data_range_enum.全部数据权限
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "user_role/create")]
        public async Task<IActionResult> create(user_role_entity model)
        {
            if (ModelState.IsValid)
            {
                model.authority_code_list = " ";
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(index));
            }
            return View(model);
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "user_role/set_permissions")]
        public async Task<IActionResult> set_permissions(int? id)
        {
            var authority_code_list = authority_util.init();
            ViewData["authority_code_list"] = authority_code_list;

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.user_role.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// 设置权限POST
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="authority_code"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "user_role/set_permissions")]
        public async Task<IActionResult> set_permissions(int id, user_role_entity model, string[] authority_code)
        {
            var authority_code_list = authority_util.init();
            ViewData["authority_code_list"] = authority_code_list;

            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string code_list = V6.list2string(authority_code);
                    //Console.WriteLine($"code_list={code_list}");
                    await _context.user_role.Where(m => m.id == id).UpdateFromQueryAsync(x => new user_role_entity
                    {
                        authority_code_list = code_list
                    });
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

            return View(model);
        }

        [admin_role_filter(authority_code = "user_role/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.user_role.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "user_role/edit")]
        public async Task<IActionResult> edit(int id, user_role_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.user_role.Where(m => m.id == id).UpdateFromQueryAsync(x => new user_role_entity
                    {
                        role_name = model.role_name,
                        data_range = model.data_range,
                        is_need_review = model.is_need_review
                    });
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

            return View(model);
        }

        [admin_role_filter(authority_code = "user_role/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.user_role.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.user_role.Any(m => m.id == id);
        }
    }
}
