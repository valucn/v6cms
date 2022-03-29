using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models;
using v6cms.utils;

namespace v6cms.blls
{
    public class site_stat_bll
    {
        private readonly ICookie _cookie;
        private readonly db_context _context;
        public site_stat_bll(ICookie cookie, db_context context)
        {
            _cookie = cookie;
            _context = context;
        }

        public async Task<site_stat_model> update()
        {
            HttpContextAccessor context = new HttpContextAccessor();
            string visitor_ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            string cookie_ip = _cookie.GetCookies("site_visitor");
            if (string.IsNullOrEmpty(cookie_ip))
            {
                _cookie.SetCookies("site_visitor", visitor_ip);
            }
            int ip_stat = 1;
            if (visitor_ip == cookie_ip)
            {
                ip_stat = 0;
            }
            if (_context.site_stat.Any(m => m.date == DateTime.Today))
            {
                await _context.site_stat.Where(m => m.date == DateTime.Today).UpdateFromQueryAsync(x => new site_stat_entity
                {
                    ip = x.ip + ip_stat,
                    pv = x.pv + 1
                });
            }
            else
            {
                _context.site_stat.Add(new site_stat_entity
                {
                    date = DateTime.Today,
                    ip = 1,
                    pv = 1,
                });
                await _context.SaveChangesAsync();
            }

            var today_stat = await _context.site_stat.Where(m => m.date == DateTime.Today).FirstOrDefaultAsync();
            var stat = _context.site_stat;
            var resp = new site_stat_model
            {
                today_ip = today_stat.ip,
                today_pv = today_stat.pv,
                total_ip = stat.Sum(m => m.ip),
                total_pv = stat.Sum(m => m.pv)
            };
            return resp;
        }
    }
}
