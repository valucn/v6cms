using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6amin.Controllers
{
    /// <summary>
    /// 答疑控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "ask/index")]
    public class askController : Controller
    {
        private readonly db_context _context;

        public askController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 答疑列表
        /// </summary>
        [admin_role_filter(authority_code = "ask/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.ask.Include(m => m.member).Include(m => m.reply).ThenInclude(m => m.user).AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 添加答疑
        /// </summary>
        [admin_role_filter(authority_code = "ask/create")]
        public IActionResult create()
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            return View();
        }

        /// <summary>
        /// 添加答疑POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "ask/create")]
        public async Task<IActionResult> create(ask_entity model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(index));
            }
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 修改答疑
        /// </summary>
        [admin_role_filter(authority_code = "ask/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.ask.Where(m => m.id == id).Include(m => m.member).FirstOrDefaultAsync();
            if (model == null)
            {
                return NotFound();
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 修改答疑POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "ask/edit")]
        public async Task<IActionResult> edit(int id, ask_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.ask.Where(m => m.id == id).UpdateFromQueryAsync(x => new ask_entity
                    {
                        title = model.title,
                        content = model.content,
                        is_review = model.is_review
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!exists(model.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(index));
            }

            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);
            return View(model);
        }

        /// <summary>
        /// 删除提问
        /// </summary>
        [admin_role_filter(authority_code = "ask/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            // 删除答疑
            await _context.ask_reply.Where(m => m.ask_id == id).DeleteFromQueryAsync();
            
            // 删除提问
            await _context.ask.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.ask.Any(m => m.id == id);
        }
    }
}
