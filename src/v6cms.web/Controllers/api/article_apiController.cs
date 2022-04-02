using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;

namespace v6cms.web.Controllers.api
{
    /// <summary>
    /// 文章接口控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class article_apiController : ControllerBase
    {
        private readonly db_context _context;

        public article_apiController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取查看次数并更新
        /// </summary>
        /// <param name="column_id">栏目id</param>
        /// <param name="article_id">文章id</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 3, VaryByQueryKeys = new string[] { "article_id" })]
        public async Task<int> update_views(int article_id)
        {
            //更新查看次数
            await _context.article.Where(m => m.id == article_id).UpdateFromQueryAsync(x => new article_entity
            {
                views = x.views + 1
            });

            //获取查看次数
            var article = await _context.article.Where(m => m.id == article_id).FirstOrDefaultAsync();
            int views = article.views;
            return views;
        }

    }
}
