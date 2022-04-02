using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using Webdiyer.AspNetCore;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 问答评论控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "comment/index")]
    public class ask_commentController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public ask_commentController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        [admin_role_filter(authority_code = "comment/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            //获取上次访问页面
            string last_visit_url = HttpContext.Request.GetDisplayUrl();
            HttpContext.Response.Cookies.Append("last_visit_url", last_visit_url);

            var query = _context.comment.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).Include(m => m.ask).ToPagedListAsync(page, 15);
            return View(model);
        }

        /// <summary>
        /// 修改评论
        /// </summary>
        [admin_role_filter(authority_code = "comment/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            HttpContext.Response.Cookies.Append("last_visit_url", Request.GetDisplayUrl());

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.comment.Where(m => m.id == id).Include(m => m.ask).FirstOrDefaultAsync();
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// 修改评论POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "comment/edit")]
        public async Task<IActionResult> edit(int id, comment_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.comment.Where(m => m.id == id).UpdateFromQueryAsync(x => new comment_entity
                    {
                        comment_name = model.comment_name,
                        comment_content = model.comment_content
                    });

                    //删除缓存
                    _cache.Remove($"article_details_{id}_cache");
                    _cache.Remove($"comment_list_{model.source_id}_ask_cache");
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

            return View(model);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        [admin_role_filter(authority_code = "comment/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            //查询评论
            var model = await _context.comment.Where(m => m.id == id).FirstOrDefaultAsync();
            //删除评论
            await _context.comment.Where(m => m.id == id).DeleteFromQueryAsync();

            //删除缓存
            _cache.Remove($"article_details_{id}_cache");
            _cache.Remove($"comment_list_{model.source_id}_ask_cache");
            return RedirectToAction(nameof(index));
        }

        private bool exists(int id)
        {
            return _context.article.Any(m => m.id == id);
        }
    }
}
