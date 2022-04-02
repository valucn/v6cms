using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.entities.enums;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    [Route("v6admin/api/[controller]/[action]")]
    [ApiController]
    public class column_apiController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;

        public column_apiController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /*[HttpGet]
        [admin_role_filter(authority_code = "column/index", is_ajax = true)]
        public async Task<List<xm_select_response>> get_list_old(bool show_root = false, bool show_all = false)
        {
            var resp = await get_column_list(0, show_root, show_all);
            return resp;
        }

        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        private async Task<List<xm_select_response>> get_column_list(int parent_id = 0, bool show_root = false, bool show_all = false)
        {
            var list = new List<xm_select_response>();
            if (parent_id == 0 && show_root)
            {
                list.Add(new xm_select_response
                {
                    value = 0,
                    name = "根栏目"
                });
            }
            if (parent_id == 0 && show_all)
            {
                list.Add(new xm_select_response
                {
                    value = 0,
                    name = "所有栏目"
                });
            }

            string user_id_str = HttpContext.User.FindFirstValue("user_id");
            int user_id = int.Parse(user_id_str);
            var user = _context.user.Where(m => m.id == user_id).FirstOrDefault();
            var column_id_list = new List<int>();
            if (user.dept_id > 0)
            {
                var dept = _context.dept.Where(m => m.id == user.dept_id).FirstOrDefault();
                foreach (var item in dept.column_id_list.Split(','))
                {
                    column_id_list.Add(int.Parse(item));
                }
            }

            var query = _context.column.Where(m => m.parent_id == parent_id).AsQueryable();
            //if (parent_id > 0 && !show_root)
            //{
            //    query = query.Where(m => m.column_attribute == column_attribute_enum.最终列表栏目);
            //}
            if (parent_id > 0 && column_id_list.Count() > 0)
            {
                query = query.Where(m => column_id_list.Contains(m.id));
            }
            var column_list = await query.ToListAsync();
            foreach (var item in column_list)
            {
                //if (item.column_name == "特色专科")
                //{
                //    int test = 0;
                //}
                var children = new List<xm_select_response>();
                int child_count = await column_count_child_async(item.id);
                if (child_count > 0)
                {
                    var children_list = await get_column_list(item.id, show_root, show_all);
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
                    name = item.column_name,
                    value = item.id,
                    children = children
                });
            }
            return list;
        }*/

        [HttpGet]
        [admin_role_filter(authority_code = "column/index", is_ajax = true)]
        public async Task<List<xm_select_response>> get_list(bool show_root = false, bool show_all = false)
        {
            string cache_key = $"sys_api_column_list_json_{show_root}_{show_all}";
            var cache_list = _cache.Get<List<xm_select_response>>(cache_key);
            if (cache_list != null)
            {
                return cache_list;
            }

            var data = await _context.column.ToListAsync();
            var list = GetData(data);
            if (show_root)
            {
                list.Insert(0, new xm_select_response
                {
                    value = 0,
                    name = "根栏目"
                });
            }
            if (show_all)
            {
                list.Insert(0, new xm_select_response
                {
                    value = 0,
                    name = "所有栏目"
                });
            }
            _cache.Set(cache_key, list, TimeSpan.FromSeconds(6000));
            return list;
        }

        public List<xm_select_response> GetData(List<column_entity> data)
        {
            var nodes = data.Where(x => x.parent_id == 0).Select(x => new xm_select_response
            {
                id = x.id,
                name = x.column_name,
                value = x.id
            }).ToList();
            foreach (var item in nodes)
            {
                var count = column_count_child(item.id);
                if (count > 0)
                {
                    item.children = GetChildrens(item, data);
                }
            }
            return nodes;
        }

        private List<xm_select_response> GetChildrens(xm_select_response item, List<column_entity> data)
        {
            var children = data.Where(x => x.parent_id == item.id).Select(x => new xm_select_response
            {
                id = x.id,
                name = x.column_name,
                value = x.id
            }).ToList();
            foreach (var child in children)
            {
                var count = column_count_child(child.id);
                if (count > 0)
                {
                    child.children = GetChildrens(child, data);
                }
            }
            return children;
        }

        /// <summary>
        /// 统计子栏目个数
        /// </summary>
        /// <param name="column_id"></param>
        /// <returns></returns>
        private int column_count_child(int column_id)
        {
            return _context.column.Count(m => m.parent_id == column_id);
        }

        /// <summary>
        /// 统计子栏目个数
        /// </summary>
        /// <param name="column_id"></param>
        /// <returns></returns>
        private async Task<int> column_count_child_async(int column_id)
        {
            return await _context.column.CountAsync(m => m.parent_id == column_id);
        }
    }
}
