using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v6cms.entities.db_set;
using v6cms.services;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 生日名单控制器
    /// </summary>
    public class birthday_listController : Controller
    {
        private readonly db_context _context;
        private readonly IDataService _service;

        public birthday_listController(db_context context, IDataService service)
        {
            _context = context;
            _service = service;
        }

        /// <summary>
        /// 获取js列表
        /// </summary>
        /// <returns></returns>
        public ActionResult get_js()
        {
            StringBuilder html = new StringBuilder();
            var model = _service.get_today_birthday();
            string users = string.Join("，", model.Select(m => m.real_name));

            html.AppendLine($"var birthday_count = {model.Count()};");
            html.AppendLine($"document.write('{users}');");

            return Content(html.ToString());
        }
    }
}
