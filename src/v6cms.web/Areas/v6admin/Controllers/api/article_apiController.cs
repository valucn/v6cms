using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    /// <summary>
    /// 文章管理接口控制器
    /// </summary>
    [ApiController]
    [Route("v6admin/api/[controller]/[action]")]
    public class article_apiController : ControllerBase
    {
        private readonly db_context _context;

        public article_apiController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        [HttpGet]
        [admin_role_filter(authority_code = "article/review", is_ajax = true)]
        public async Task<object> review(int article_id)
        {
            await _context.article.Where(m => m.id == article_id).UpdateFromQueryAsync(x => new article_entity
            {
                is_review = true
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
