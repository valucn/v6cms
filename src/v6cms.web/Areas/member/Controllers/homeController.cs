using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities.db_set;

namespace v6cms.web.Areas.member.Controllers
{
    /// <summary>
    /// 会员中心控制器
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class homeController : member_baseController
    {
        private readonly db_context _context;
        public homeController(db_context context) : base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> index(string return_url)
        {
            if (!string.IsNullOrEmpty(return_url))
            {
                return Redirect(return_url);
            }

            var model = await _context.member.Where(m => m.id == current_member_id).FirstOrDefaultAsync();
            return View(model);
        }
    }
}
