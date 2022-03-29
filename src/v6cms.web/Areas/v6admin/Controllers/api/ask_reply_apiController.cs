using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities.db_set;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    [ApiController]
    [Route("v6admin/api/[controller]/[action]")]
    public class ask_reply_apiController : Controller
    {
        private readonly db_context _context;

        public ask_reply_apiController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 删除答疑回复
        /// </summary>
        [admin_role_filter(authority_code = "ask_reply/delete", is_ajax = true)]
        public async Task<global_response> delete(int id)
        {
            await _context.ask_reply.Where(m => m.id == id).DeleteFromQueryAsync();
            var resp = new global_response
            {
                code = 200,
                msg = "删除成功"
            };
            return resp;
        }

    }
}
