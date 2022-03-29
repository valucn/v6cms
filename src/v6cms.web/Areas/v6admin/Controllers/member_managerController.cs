using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.utils;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 会员管理控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "member_manager/index")]
    public class member_managerController : Controller
    {
        private readonly db_context _context;

        public member_managerController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 会员列表
        /// </summary>
        [admin_role_filter(authority_code = "member_manager/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.member.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 修改会员
        /// </summary>
        [admin_role_filter(authority_code = "member_manager/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.member.FindAsync(id);
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
        /// 修改会员POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "member_manager/edit")]
        public async Task<IActionResult> edit(int id, member_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.member.Where(m => m.id == id).UpdateFromQueryAsync(x => new member_entity
                    {
                        nick_name = model.nick_name,
                        company = model.company,
                        avatar = model.avatar,
                        mobile = model.mobile,
                        card_id = model.card_id,
                        is_lock = model.is_lock,
                        reg_ip = model.reg_ip,
                        member_level = model.member_level
                    });
                    //如果密码不为空则修改密码
                    if (!string.IsNullOrEmpty(model.password))
                    {
                        string password = V6.sha1(model.password);
                        await _context.member.Where(m => m.id == id).UpdateFromQueryAsync(x => new member_entity
                        {
                            password = password
                        });
                    }
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
        /// 删除会员
        /// </summary>
        [admin_role_filter(authority_code = "member_manager/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.member.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.member.Any(m => m.id == id);
        }
    }
}
