using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities.db_set;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    [Route("v6admin/api/[controller]/[action]")]
    [ApiController]
    public class user_apiController : ControllerBase
    {
        private readonly db_context _context;

        public user_apiController(db_context context)
        {
            _context = context;
        }

        [HttpGet]
        [admin_role_filter(is_ajax = true)]
        public async Task<bool> check_username(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            var reslt = await _context.user.AnyAsync(m => m.username == username);
            return !reslt;
        }
    }
}
