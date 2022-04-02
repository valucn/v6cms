using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using v6cms.entities.db_set;
using v6cms.entities.enums;

namespace v6cms.web.Areas.member.Controllers
{
    public class member_baseController : Controller
    {
        /// <summary>
        /// 当前会员id
        /// </summary>
        public int current_member_id { get; set; }

        private readonly db_context _context;
        public member_baseController(db_context context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int member_id;
            string member_id_str = HttpContext.User.FindFirstValue("member_id");
            string member_nick_name = HttpContext.User.FindFirstValue("member_nick_name");
            int.TryParse(member_id_str, out member_id);
            current_member_id = member_id;

            var new_reply = _context.ask.Where(m => m.member_id == member_id && m.reply_status == reply_status_enum.新回复).Count();

            ViewData["member_id"] = member_id;
            ViewData["member_nick_name"] = member_nick_name;
            ViewData["new_reply"] = new_reply;
        }
    }
}
