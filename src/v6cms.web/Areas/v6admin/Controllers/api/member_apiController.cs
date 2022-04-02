using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    /// <summary>
    /// 会员管理接口控制器
    /// </summary>
    [ApiController]
    [Route("v6admin/api/[controller]/[action]")]
    public class member_apiController : ControllerBase
    {
        private readonly db_context _context;

        public member_apiController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="member_id"></param>
        /// <returns></returns>
        [HttpGet]
        [admin_role_filter(authority_code = "member_manager/edit", is_ajax = true)]
        public async Task<object> review(int member_id)
        {
            await _context.member.Where(m => m.id == member_id).UpdateFromQueryAsync(x => new member_entity
            {
                member_level = member_level_enum.审核会员
            });
            var resp = new global_response
            {
                code = 200,
                msg = "审核成功"
            };
            return resp;
        }
    }
}
