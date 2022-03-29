using v6cms.models.page_views;
using Microsoft.AspNetCore.Mvc;

namespace v6cms.web.Areas.v6admin.Controllers
{
    [Area("v6admin")]
    public class noticeController : Controller
    {
        public IActionResult fail(string message, string authority_code, string jump_url)
        {
            var model = new unauthorized_model
            {
                msg = message,
                authority_code = authority_code,
                jump_url = jump_url
            };
            return View(model);
        }
    }
}
