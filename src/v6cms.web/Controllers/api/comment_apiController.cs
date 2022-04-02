using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
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
        /// 统计评论和点赞
        /// </summary>
        /// <param name="source_ids">资源id集合</param>
        /// <returns></returns>
        public async Task<count_response> count(string source_ids)
        {
            var resp = new count_response();
            var data = new List<count_response.data_model>();

            string[] arr = source_ids.Split(',');
            foreach (var item in arr)
            {
                int source_id = int.Parse(item);
                data.Add(new count_response.data_model
                {
                    ask_id = source_id,
                    comment_count = await count_comment(source_id),
                    up_count = await count_up(source_id),
                });
            }
            resp.data = data;
            return resp;
        }

        /// <summary>
        /// 统计评论
        /// </summary>
        /// <param name="source_id"></param>
        /// <returns></returns>
        private async Task<int> count_comment(int source_id)
        {
            return await _context.comment.CountAsync(m => m.source_id == source_id);
        }

        /// <summary>
        /// 统计点赞
        /// </summary>
        /// <param name="source_id"></param>
        /// <returns></returns>
        private async Task<int> count_up(int source_id)
        {
            return await _context.ask_up.CountAsync(m => m.source_id == source_id);
        }

        /// <summary>
        /// POST提交评论内容
        /// </summary>
        /// <param name="request">评论实体类</param>
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
            if (_context.comment.Any(m => m.source_id == request.source_id && m.user_id == user.id))
            {
                rows = await _context.comment.Where(m => m.source_id == request.source_id && m.user_id == user.id).UpdateFromQueryAsync(x => new comment_entity
                {
                    comment_name = user.real_name,
                    comment_content = request.comment_content
                });
                msg = "更新评论成功";
            }
            else
            {
                _context.comment.Add(new comment_entity
                {
                    module = request.module,
                    user_id = user.id,
                    source_id = request.source_id,
                    comment_name = user.real_name,
                    comment_content = request.comment_content,
                    create_time = DateTime.Now
                });
                rows = await _context.SaveChangesAsync();
                msg = "发表评论成功";
            }
            if (rows > 0)
            {
                //删除缓存
                _cache.Remove($"comment_list_{request.source_id}_{request.module}_cache");

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
