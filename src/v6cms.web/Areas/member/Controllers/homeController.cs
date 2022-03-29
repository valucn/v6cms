using Microsoft.AspNetCore.Mvc;

namespace v6cms.web.Areas.member.Controllers
{
    /// <summary>
    /// 会员中心
    /// </summary>
    [Area("member")]
    [member_role_filter]
    public class homeController : Controller
    {
        public IActionResult index(string return_url)
        {
            if (!string.IsNullOrEmpty(return_url))
            {
                return Redirect(return_url);
            }
            return View();
        }
    }
}
