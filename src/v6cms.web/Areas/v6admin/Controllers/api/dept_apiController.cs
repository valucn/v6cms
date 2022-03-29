using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v6cms.entities.db_set;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    [Route("v6admin/api/[controller]/[action]")]
    [ApiController]
    public class dept_apiController : ControllerBase
    {
        private readonly db_context _context;

        public dept_apiController(db_context context)
        {
            _context = context;
        }

        [HttpGet]
        [admin_role_filter(authority_code = "dept/index", is_ajax = true)]
        public async Task<List<xm_select_response>> get_list()
        {
            return await get_dept_list();
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        private async Task<List<xm_select_response>> get_dept_list(int parent_id = 0)
        {
            var list = new List<xm_select_response>();
            if (parent_id == 0)
            {
                list.Add(new xm_select_response
                {
                    name = "不属于任何部门",
                    value = 0,
                });
            }

            var data_list = await _context.dept.Where(m => m.parent_id == parent_id).ToListAsync();
            foreach (var item in data_list)
            {
                var children = new List<xm_select_response>();
                int child_count = await column_count_child(item.id);
                if (child_count > 0)
                {
                    var children_list = await get_dept_list(item.id);
                    foreach (var child in children_list)
                    {
                        children.Add(new xm_select_response
                        {
                            name = child.name,
                            value = child.value
                        });
                    }
                }
                list.Add(new xm_select_response
                {
                    name = item.dept_name,
                    value = item.id,
                    children = children
                });
            }
            return list;
        }

        /// <summary>
        /// 统计子部门个数
        /// </summary>
        /// <param name="column_id"></param>
        /// <returns></returns>
        private async Task<int> column_count_child(int column_id)
        {
            return await _context.dept.CountAsync(m => m.parent_id == column_id);
        }
    }
}
