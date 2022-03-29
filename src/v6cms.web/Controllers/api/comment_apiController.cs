using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;
using v6cms.utils;

namespace v6cms.web.Controllers.api
{
    /// <summary>
    /// 评论接口控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class comment_apiController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public comment_apiController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// POST提交评论内容
        /// </summary>
        /// <param name="article_id">文章id</param>
        /// <param name="pwd">用户密码</param>
        /// <param name="comment_content">评论内容</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<global_response> post([FromBody] comment_request request)
        {
            var resp = new global_response();
            string password_sha1 = V6.sha1(request.password);
            var user = _context.user.FirstOrDefault(m => m.username == request.username && m.password == password_sha1 && !m.is_lock);
            if (user == null)
            {
                resp.code = 400;
                resp.msg = "用户名密码不对";
                return resp;
            }
            int rows = 0;
            string msg = "";
            if (_context.comment.Any(m => m.article_id == request.article_id && m.user_id == user.id))
            {
                rows = await _context.comment.Where(m => m.article_id == request.article_id && m.user_id == user.id).UpdateFromQueryAsync(x => new comment_entity
                {
                    name = user.real_name,
                    comment_content = request.comment_content
                });
                msg = "更新评论成功";
            }
            else
            {
                _context.comment.Add(new comment_entity
                {
                    user_id = user.id,
                    article_id = request.article_id,
                    name = user.real_name,
                    comment_content = request.comment_content,
                    comment_time = DateTime.Now
                });
                rows = await _context.SaveChangesAsync();
                msg = "发表评论成功";
            }
            if (rows > 0)
            {
                //删除缓存
                string cache_key = $"comment_list_{request.article_id}_cache";
                _cache.Remove(cache_key);

                resp.code = 200;
                resp.msg = msg;
                return resp;
            }
            else
            {
                resp.code = 500;
                resp.msg = "评论失败";
                return resp;
            }
        }
    }
}
