using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v6cms.entities.db_set;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 值班表控制器
    /// </summary>
    public class dutyController : v6baseController
    {
        private readonly db_context _context;

        public dutyController(db_context context)
        {
            _context = context;
        }

        public async Task<ActionResult> index(DateTime? month)
        {
            var _date = DateTime.Today;
            if (month.HasValue)
            {
                _date = month.Value;
            }
            var start = new DateTime(_date.Year, _date.Month, 1);
            var next_month = _date.AddMonths(1);
            var end = new DateTime(next_month.Year, next_month.Month, 1);
            var model = await _context.duty.Where(m => m.date >= start && m.date < end).OrderBy(m => m.date).ToListAsync();

            var configs = _context.duty_config;
            foreach (var item in configs)
            {
                ViewData[item.column_no] = item.display_name;
            }
            return View(model);
        }

        /// <summary>
        /// 获取js列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> get_js()
        {
            var today = DateTime.Today;
            StringBuilder html = new StringBuilder();
            var model = await _context.duty.FirstOrDefaultAsync(m => m.date == today);
            if (model == null)
            {
                html.AppendLine($"document.write('无值班信息');");
                return Content(html.ToString());
            }

            string line_str = "";
            var configs = await _context.duty_config.ToListAsync();
            foreach (var item in configs)
            {
                if (!string.IsNullOrEmpty(item.display_name))
                {
                    //如果表头显示名称不为空
                    line_str += $"<span class=\"duty-header\">{item.display_name}：</span>";

                    string column_value = "";
                    var column_obj = model.GetType().GetProperty(item.column_no).GetValue(model, null);
                    if (column_obj != null)
                    {
                        //如果表格值不为空
                        column_value = column_obj.ToString();
                    }
                    else
                    {
                        column_value = "无";
                    }
                    line_str += column_value + "，";
                }
            }
            if (line_str.EndsWith("，"))
            {
                line_str = line_str.Substring(0, line_str.Length - 1);
            }
            html.AppendLine($"document.write('{line_str}');");

            return Content(html.ToString());
        }
    }
}
