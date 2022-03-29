using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;

namespace v6cms.web.Controllers.plug
{
    /// <summary>
    /// SSCMS外挂-文章内容
    /// </summary>
    public class sscms_contentController : Controller
    {
        private readonly sscms_db_context _sscms_context;

        public sscms_contentController(sscms_db_context sscms_context)
        {
            _sscms_context = sscms_context;
        }

        /// <summary>
        /// 季度
        /// </summary>
        int quarter = 0;

        /// <summary>
        /// 调用浏览次数js
        /// </summary>
        /// <param name="ChannelId">栏目id</param>
        /// <param name="ContentId">文章内容id</param>
        /// <returns></returns>
        [EnableCors("any")] //支持跨域
        [ResponseCache(Duration = 3, VaryByQueryKeys = new string[] { "ChannelId", "ContentId" })]
        public async Task<IActionResult> hits_js(int ContentId, int ChannelId)
        {
            await _sscms_context.sscms_content.Where(m => m.ChannelId == ChannelId && m.Id == ContentId).UpdateFromQueryAsync(x => new sscms_content_entity
            {
                Hits = x.Hits + 1
            });
            var content = await _sscms_context.sscms_content.FirstOrDefaultAsync(m => m.ChannelId == ChannelId && m.Id == ContentId);
            return Json(content.Hits);
        }

        /// <summary>
        /// 信息统计js
        /// </summary>
        /// <param name="ChannelIds">栏目id，如1,2,4,9,15</param>
        /// <returns></returns>
        public async Task<IActionResult> count_js(string ChannelId_list)
        {
            var now = DateTime.Now;

            StringBuilder html = new StringBuilder();

            var admins = new Dictionary<int, string>();
            admins.Add(5, "一大队");
            admins.Add(6, "三大队");
            admins.Add(7, "四大队");
            admins.Add(8, "涉众大队");
            admins.Add(9, "情报大队");
            admins.Add(10, "基础工作大队");
            admins.Add(11, "法制大队");
            admins.Add(12, "政治处");
            admins.Add(14, "指挥室");
            //admins.Add(15, "指挥室");
            admins.Add(17, "纪检监督室");

            var list = new List<count_model>();
            foreach (var item in admins)
            {
                var query = _sscms_context.sscms_content.Where(m => m.Checked);
                if (!string.IsNullOrEmpty(ChannelId_list))
                {
                    var ChannelIds = ChannelId_list.Split(',').Select(x => int.Parse(x)).ToArray(); //注意逗号是半角的。
                    query = query.Where(m => ChannelIds.Contains(m.ChannelId));
                }
                if (item.Key == 14)
                {
                    //指挥室两个账号
                    query = query.Where(m => m.adminId == item.Key || m.adminId == 15);
                }
                else
                {
                    query = query.Where(m => m.adminId == item.Key);
                }
                if (now.Month == 1 || now.Month == 2 || now.Month == 3)
                {
                    quarter = 1;
                    query = query.Where(m => m.AddDate.Month == 1 || m.AddDate.Month == 2 || m.AddDate.Month == 3);
                }
                else if (now.Month == 4 || now.Month == 5 || now.Month == 6)
                {
                    quarter = 2;
                    query = query.Where(m => m.AddDate.Month == 4 || m.AddDate.Month == 5 || m.AddDate.Month == 6);
                }
                else if (now.Month == 7 || now.Month == 8 || now.Month == 9)
                {
                    quarter = 3;
                    query = query.Where(m => m.AddDate.Month == 7 || m.AddDate.Month == 8 || m.AddDate.Month == 9);
                }
                else if (now.Month == 10 || now.Month == 11 || now.Month == 12)
                {
                    quarter = 4;
                    query = query.Where(m => m.AddDate.Month == 10 || m.AddDate.Month == 11 || m.AddDate.Month == 12);
                }
                var count = await query.CountAsync();
                var sum = await query.Include(m => m.sscms_Channel).SumAsync(m => m.sscms_Channel.score);
                Console.WriteLine($"count={count}");
                Console.WriteLine($"sum={sum}");
                list.Add(new count_model
                {
                    unit = item.Value,
                    count = count,
                    sum = sum,
                    quarter = quarter
                });
            }
            list = list.OrderByDescending(m => m.sum).ToList();
            foreach (var item in list)
            {
                html.AppendLine("document.write('<li>');");
                html.AppendLine($"document.write('    <span class=\"w1\">{item.unit}</span>');");
                html.AppendLine($"document.write('    <span class=\"w2\">{item.count}</span>');");
                html.AppendLine($"document.write('    <span class=\"w3\">{item.sum.ToString("F1")}</span>');");
                html.AppendLine($"document.write('    <!--第{item.quarter}季度-->');");
                html.AppendLine("document.write('</li>');");
            }

            return Content(html.ToString());
        }

        public class count_model
        {
            public string unit { get; set; }
            public int count { get; set; }
            public decimal sum { get; set; }
            public int quarter { get; set; }
        }
    }
}
