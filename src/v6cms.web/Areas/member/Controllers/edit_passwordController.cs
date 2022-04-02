using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.utils;
using v6cms.web.Areas.member.Controllers;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 修改密码控制器
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class edit_passwordController : member_baseController
    {
        private readonly db_context _context;

        public edit_passwordController(db_context context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public async Task<IActionResult> index()
        {
            return View();
        }

        /// <summary>
        /// 保存密码
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> index(member_entity model)
        {
            if (ModelState.IsValid)
            {
                string password_sha1 = V6.sha1(model.password);
                await _context.member.Where(m => m.id == current_member_id).UpdateFromQueryAsync(x => new member_entity
                {
                    password = password_sha1
                });
                ViewData["msg"] = "保存成功";
            }
            return View(model);
        }

    }
}
