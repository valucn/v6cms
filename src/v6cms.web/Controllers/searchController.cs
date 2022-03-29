using Microsoft.AspNetCore.Mvc;

namespace v6cms.web.Controllers
{
    public class searchController : v6baseController
    {
        public IActionResult index()
        {
            return View();
        }
    }
}
