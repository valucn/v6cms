using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using v6cms.models;
using v6cms.models.page_views;

namespace v6cms.web.Controllers
{
    public class homeController : v6baseController
    {
        public IActionResult index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult unauthorized(string message, string authority_code)
        {
            var model = new unauthorized_model
            {
                msg = message,
                authority_code = authority_code
            };
            return View(model);
        }
    }
}
