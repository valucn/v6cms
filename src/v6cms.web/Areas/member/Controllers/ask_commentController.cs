using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using v6cms.entities.db_set;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.member.Controllers
{
    /// <summary>
    /// 我的问答评论
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class ask_commentController : member_baseController
    {
        private readonly db_context _context;
        public ask_commentController(db_context context) : base(context)
        {
            _context = context;
        }

        public IActionResult index(int page)
        {
            int page_size = 15;
            var model = _context.comment.Where(m => m.module == "ask" && m.member_id == current_member_id).Include(m => m.ask).OrderByDescending(m => m.id).ToPagedList(page, page_size);
            return View(model);
        }
    }
}
