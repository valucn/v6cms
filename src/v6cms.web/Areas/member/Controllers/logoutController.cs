using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace v6cms.web.Areas.member.Controllers
{
    /// <summary>
    /// 会员退出登录
    /// </summary>
    [Area("member")]
    public class logoutController : Controller
    {
        public async Task<IActionResult> index()
        {
            await HttpContext.SignOutAsync("v6cms_member");
            return RedirectToAction("index", "home");
        }
    }
}
