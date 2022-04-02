using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.web.Areas.member.Controllers;
using Webdiyer.AspNetCore;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 我赞过的问答控制器
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class my_upController : member_baseController
    {
        private readonly db_context _context;

        public my_upController(db_context context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 我赞过的问答列表
        /// </summary>
        public async Task<IActionResult> index(int page = 1)
        {
            var source_ids = _context.ask_up.Where(m => m.member_id == current_member_id).Select(m => m.source_id);

            var query = _context.ask.Where(m => source_ids.Contains(m.id)).AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

    }
}
