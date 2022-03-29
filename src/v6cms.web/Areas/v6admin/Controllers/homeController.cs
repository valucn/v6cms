using v6cms.entities.db_set;
using v6cms.models.page_views;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using v6cms.utils;
using System.Security.Claims;
using v6cms.utils.premission;
using v6cms.services;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter]
    public class homeController : Controller
    {
        private readonly db_context _context;
        private readonly IDataService _service;

        public homeController(db_context context, IDataService service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult index(string return_url)
        {
            var config = _service.get_site_config();
            ViewData["site_name"] = config.site_name;

            string last_visit_url;
            HttpContext.Request.Cookies.TryGetValue("last_visit_url", out last_visit_url);

            if (string.IsNullOrWhiteSpace(return_url))
            {
                return_url = "/v6admin/home/console";
            }
            if (return_url.EndsWith("/v6admin/home"))
            {
                return_url = "/v6admin/home/console";
            }
            if (return_url.EndsWith("/v6admin"))
            {
                return_url = "/v6admin/home/console";
            }
            if (!string.IsNullOrEmpty(last_visit_url))
            {
                return_url = last_visit_url;
            }
            ViewData["return_url"] = return_url;

            string user_role_id = User.FindFirstValue("user_role_id");
            var current_user_role = _context.user_role.Where(m => m.id.ToString() == user_role_id).FirstOrDefault();
            var authority_code_list = string_util.string2list(current_user_role.authority_code_list);

            bool column = auth_premissions.has_permissions(authority_code_list, "column/index");
            bool article = auth_premissions.has_permissions(authority_code_list, "article/index");
            bool comment = auth_premissions.has_permissions(authority_code_list, "comment/index");
            ViewData["cms"] = column || article || comment;
            ViewData["column"] = column;
            ViewData["article"] = article;
            ViewData["comment"] = comment;

            bool create_html_article = auth_premissions.has_permissions(authority_code_list, "create_html/article");
            ViewData["create_html"] = create_html_article;
            ViewData["create_html/article"] = create_html_article;

            bool link_category = auth_premissions.has_permissions(authority_code_list, "link_category/index");
            bool link_manager = auth_premissions.has_permissions(authority_code_list, "link_manager/index");
            ViewData["link"] = link_category || link_manager;
            ViewData["link_category"] = link_category;
            ViewData["link_manager"] = link_manager;

            bool member_manager = auth_premissions.has_permissions(authority_code_list, "member_manager/index");
            bool ask = auth_premissions.has_permissions(authority_code_list, "ask/index");
            bool ask_reply = auth_premissions.has_permissions(authority_code_list, "ask_reply/index");
            ViewData["member"] = member_manager || ask || ask_reply;
            ViewData["member_manager"] = member_manager;
            ViewData["ask"] = ask;
            ViewData["ask_reply"] = ask_reply;

            ViewData["ip_address"] = auth_premissions.has_permissions(authority_code_list, "ip_address/index");

            bool code_generate = auth_premissions.has_permissions(authority_code_list, "code_generate/index");
            bool advertisement = auth_premissions.has_permissions(authority_code_list, "advertisement/index");
            bool leader_mailbox = auth_premissions.has_permissions(authority_code_list, "leader_mailbox/index");
            bool duty_config = auth_premissions.has_permissions(authority_code_list, "duty_config/index");
            bool duty_manager = auth_premissions.has_permissions(authority_code_list, "duty_manager/index");
            bool birthday_list = auth_premissions.has_permissions(authority_code_list, "birthday_list/index");
            ViewData["app"] = code_generate || advertisement || leader_mailbox || duty_config || duty_manager | birthday_list;
            ViewData["code_generate"] = code_generate;
            ViewData["advertisement"] = advertisement;
            ViewData["leader_mailbox"] = leader_mailbox;
            ViewData["duty_config"] = duty_config;
            ViewData["duty_manager"] = duty_manager;
            ViewData["birthday_list"] = birthday_list;

            bool user_manager = auth_premissions.has_permissions(authority_code_list, "user_manager/index");
            bool user_role = auth_premissions.has_permissions(authority_code_list, "user_role/index");
            bool dept = auth_premissions.has_permissions(authority_code_list, "dept/index");
            ViewData["user"] = user_manager || user_role || dept;
            ViewData["user_manager"] = user_manager;
            ViewData["user_role"] = user_role;
            ViewData["dept"] = dept;

            bool cache_manager = auth_premissions.has_permissions(authority_code_list, "cache_manager/index");
            bool site_config = auth_premissions.has_permissions(authority_code_list, "site_config/index");
            ViewData["set"] = cache_manager || site_config;
            ViewData["cache_manager"] = cache_manager;
            ViewData["site_config"] = site_config;

            return View();
        }

        public IActionResult console()
        {
            return View();
        }

        public IActionResult unauthorized(string message, string authority_code)
        {
            var model = new unauthorized_model
            {
                msg = message,
                authority_code = authority_code
            };
            return View(model);
        }
    }
}
