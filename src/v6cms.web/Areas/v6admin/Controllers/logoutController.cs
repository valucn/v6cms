using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 管理员退出登录
    /// </summary>
    [Area("v6admin")]
    public class logoutController : Controller
    {
        public async Task<IActionResult> index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "home");
        }
    }
}
