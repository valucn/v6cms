using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using v6cms.blls;
using v6cms.entities.db_set;
using v6cms.utils;

namespace v6cms.web.Controllers
{
    public class site_statController : Controller
    {
        private readonly ICookie _cookie;
        private readonly db_context _context;

        public site_statController(ICookie cookie, db_context context)
        {
            _cookie = cookie;
            _context = context;
        }

        public async Task<IActionResult> get_js(string show_type = "ip")
        {
            var ssb = new site_stat_bll(_cookie, _context);
            var stat = await ssb.update();
            int total_stat, today_stat;
            if (show_type == "pv")
            {
                total_stat = stat.total_pv;
                today_stat = stat.today_pv;
            }
            else
            {
                total_stat = stat.total_ip;
                today_stat = stat.today_ip;
            }

            StringBuilder html = new StringBuilder();
            html.AppendLine($"document.write('<span>访问总量：{total_stat}人次</span>');");
            html.AppendLine($"document.write('<span>今天访问：{today_stat}人次</span>');");
            return Content(html.ToString());
        }
    }
}
