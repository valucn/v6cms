using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.utils;

namespace v6cms.web.Controllers.api
{
    /// <summary>
    /// 文章接口控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class article_apiController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public article_apiController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
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
        public async Task<int> update_views(int column_id, int article_id)
        {
            //更新查看次数
            await _context.article.Where(m => m.id == article_id).UpdateFromQueryAsync(x => new article_entity
            {
                views = x.views + 1
            });

            //获取查看次数
            var article = _context.article.Where(m => m.id == article_id).FirstOrDefault();
            int views = article.views;
            return views;
        }

    }
}
