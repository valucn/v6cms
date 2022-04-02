using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities.db_set;
using v6cms.entities.enums;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 网站栏目控制器
    /// </summary>
    public class columnController : v6baseController
    {
        private readonly db_context _context;

        public columnController(db_context context)
        {
            _context = context;
        }

        public async Task<ActionResult> article_list(int id)
        {
            var column = await _context.column.Where(m => m.id == id).FirstOrDefaultAsync();
            if (column == null)
            {
                ViewData["msg"] = "分类不存在";
                return View("_notice");
            }
            if (!string.IsNullOrEmpty(column.list_view_path))
            {
                return View(column.list_view_path);
            }
            else
            {
                if (column.column_attribute == column_attribute_enum.栏目封面)
                {
                    return View("column_index");
                }
                else if (column.column_attribute == column_attribute_enum.频道封面)
                {
                    return View("channel_index");
                }
            }
            return View();
        }
    }
}
