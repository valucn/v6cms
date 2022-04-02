using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.web.Areas.member.Controllers;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 修改个人信息控制器
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class edit_infoController : member_baseController
    {
        private readonly db_context _context;

        public edit_infoController(db_context context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        public async Task<IActionResult> index()
        {
            var model = await _context.member.Where(m => m.id == current_member_id).FirstOrDefaultAsync();
            return View(model);
        }

        /// <summary>
        /// 保存修改个人信息
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> index(member_entity model)
        {
            if (ModelState.IsValid)
            {
                await _context.member.Where(m => m.id == current_member_id).UpdateFromQueryAsync(x => new member_entity
                {
                    nick_name = model.nick_name,
                    real_name = model.real_name,
                    company = model.company,
                    mobile = model.mobile,
                });
                ViewData["msg"] = "保存成功";
                return RedirectToAction("index", "home");
            }
            return View(model);
        }

    }
}
