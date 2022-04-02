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
    /// 我的密保问题控制器
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class question_answerController : member_baseController
    {
        private readonly db_context _context;

        public question_answerController(db_context context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 修改我的密保问题
        /// </summary>
        public async Task<IActionResult> index()
        {
            var model = await _context.member.Where(m => m.id == current_member_id).FirstOrDefaultAsync();
            return View(model);
        }

        /// <summary>
        /// 保存我的密保问题
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> index(member_entity model)
        {
            if (ModelState.IsValid)
            {
                await _context.member.Where(m => m.id == current_member_id).UpdateFromQueryAsync(x => new member_entity
                {
                    member_question1 = model.member_question1,
                    member_answer1 = model.member_answer1,
                    member_question2 = model.member_question2,
                    member_answer2 = model.member_answer2,
                    member_question3 = model.member_question3,
                    member_answer3 = model.member_answer3,
                });
                ViewData["msg"] = "保存成功";
            }
            return View(model);
        }

    }
}
