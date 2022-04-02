using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;

namespace v6cms.web.Controllers.api
{
    /// <summary>
    /// 问答点赞接口控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ask_up_apiController : Controller
    {
        private readonly db_context _context;

        public ask_up_apiController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 点赞
        /// </summary>
        public async Task<global_response> up(int ask_id)
        {
            var resp = new global_response();

            HttpContextAccessor context = new HttpContextAccessor();
            string ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            int member_id = 0;
            string member_id_str = HttpContext.User.FindFirstValue("member_id");
            int.TryParse(member_id_str, out member_id);

            string connection_id = HttpContext.Connection.Id;
            //Console.WriteLine($"connection_id={connection_id}");

            var query = _context.ask_up.Where(m => m.source_id == ask_id).AsQueryable();
            if (member_id == 0)
            {
                query = query.Where(m => m.connection_id == connection_id);
            }
            else
            {
                query = query.Where(m => m.member_id == member_id);
            }
            bool exist = await query.AnyAsync();
            if (!exist)
            {
                _context.ask_up.Add(new ask_up_entity
                {
                    source_id = ask_id,
                    member_id = member_id,
                    connection_id = connection_id,
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
    }
}
