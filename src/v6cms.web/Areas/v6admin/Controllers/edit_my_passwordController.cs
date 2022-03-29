using v6cms.entities.db_set;
using v6cms.entities.sys;
using v6cms.models.page_views;
using v6cms.utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 修改我的密码
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "edit_my_password/index")]
    public class edit_my_passwordController : Controller
    {
        private readonly db_context _context;

        public edit_my_passwordController(db_context context)
        {
            _context = context;
        }

        public IActionResult index()
        {
            return View();
        }

        /// <summary>
        /// 修改密码AJAX请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> index(edit_password_model model)
        {
            string username = User.Identity.Name;
            string old_password = V6.sha1(model.old_password);
            var user = _context.user.Where(m => m.username == username && m.password == old_password).FirstOrDefault();
            if (user == null)
            {
                var fail = new
                {
                    code = 200,
                    message = "当前密码错误",
                    success = false
                };
                return Json(fail);
            }
            string password = V6.sha1(model.password);
            await _context.user.Where(m => m.username == username).UpdateFromQueryAsync(x => new user_entity
            {
                password = password,
                is_need_edit_password = false
            });
            var result = new
            {
                code = 200,
                message = "密码修改成功",
                success = true
            };
            return Json(result);
        }
    }
}
