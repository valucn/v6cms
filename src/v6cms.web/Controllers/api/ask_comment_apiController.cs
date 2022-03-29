using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;

namespace v6cms.web.Controllers.api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ask_comment_apiController : Controller
    {
        private readonly db_context _context;

        public ask_comment_apiController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 统计评论和点赞
        /// </summary>
        /// <param name="ask_id_list"></param>
        /// <returns></returns>
        public async Task<count_response> count(string ask_id_list)
        {
            var resp = new count_response();
            var data = new List<count_response.data_model>();

            string[] arr = ask_id_list.Split(',');
            foreach (var item in arr)
            {
                int ask_id = int.Parse(item);
                data.Add(new count_response.data_model
                {
                    ask_id = ask_id,
                    comment_count = await count_comment(ask_id),
                    up_count = await count_up(ask_id),
                });
            }
            resp.data = data;
            return resp;
        }

        private async Task<int> count_comment(int ask_id)
        {
            return await _context.ask_comment.CountAsync(m => m.ask_id == ask_id && m.comment_type == "comment");
        }

        private async Task<int> count_up(int ask_id)
        {
            return await _context.ask_comment.CountAsync(m => m.ask_id == ask_id && m.comment_type == "up");
        }

        /// <summary>
        /// 点赞
        /// </summary>
        [member_role_filter(is_ajax = true)]
        public async Task<global_response> up(int ask_id)
        {
            var resp = new global_response();

            HttpContextAccessor context = new HttpContextAccessor();
            string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            string member_id_str = HttpContext.User.FindFirstValue("member_id");
            int member_id = int.Parse(member_id_str);

            bool exist = await _context.ask_comment.AnyAsync(m => m.ask_id == ask_id && m.member_id == member_id && m.comment_type == "up");
            if (!exist)
            {
                _context.ask_comment.Add(new ask_comment_entity
                {
                    ask_id = ask_id,
                    member_id = member_id,
                    comment_type = "up",
                    create_time = DateTime.Now,
                    ip = ip
                });
                await _context.SaveChangesAsync();

                resp.code = 200;
                resp.msg = "点赞成功";
                return resp;
            }
            resp.code = 600;
            resp.msg = "您已经点赞";
            return resp;
        }

        /// <summary>
        /// 点赞
        /// </summary>
        [HttpPost]
        [member_role_filter(is_ajax = true)]
        public async Task<global_response> comment(ask_comment_request request)
        {
            var resp = new global_response();

            HttpContextAccessor context = new HttpContextAccessor();
            string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            string member_id_str = HttpContext.User.FindFirstValue("member_id");
            int member_id = int.Parse(member_id_str);

            bool exist = await _context.ask_comment.AnyAsync(m => m.ask_id == request.ask_id && m.member_id == member_id && m.comment_type == "comment");
            if (!exist)
            {
                _context.ask_comment.Add(new ask_comment_entity
                {
                    ask_id = request.ask_id,
                    member_id = member_id,
                    comment_type = "comment",
                    create_time = DateTime.Now,
                    ip = ip
                });
                await _context.SaveChangesAsync();

                resp.code = 200;
                resp.msg = "评论成功";
                return resp;
            }
            resp.code = 600;
            resp.msg = "您已经评论";
            return resp;
        }
    }
}
