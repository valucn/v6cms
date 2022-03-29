using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers
{
    /// <summary>
    /// 附件上传控制器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter]
    public class uploadController : Controller
    {
        private readonly db_context _context;

        public uploadController(db_context context)
        {
            _context = context;
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="file">文件对象</param>
        /// <param name="file_type">文件类型</param>
        /// <param name="batch">批量上传</param>
        /// <param name="source_id">资源id</param>
        /// <returns></returns>
        public async Task<IActionResult> index(string module, IFormFile file, string file_type = "image", string batch = "", int source_id = 0)
        {
            var resp = new global_response();
            //foreach (var file in files)
            //{
            if (file.Length > 0)
            {
                FileInfo fi = new FileInfo(file.FileName);
                string ext = fi.Extension;
                var newFileName = Guid.NewGuid().ToString("N") + ext;
                string dir = $"uploads/{module}-{file_type}/{DateTime.Now.ToString("yyyy-MM/dd")}";
                string org_dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", dir);
                if (!Directory.Exists(org_dir))
                {
                    Directory.CreateDirectory(org_dir);
                }
                var filePath = Path.Combine(dir, newFileName);
                var org_filePath = Path.Combine(org_dir, newFileName);
                using (var stream = new FileStream(org_filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                string file_path = "/" + filePath.Replace("\\", "/");

                //获取用户id
                string user_id_str = HttpContext.User.FindFirstValue("user_id");
                int user_id = int.Parse(user_id_str);
                //图片插入到数据库
                _context.attachment.Add(new attachment_entity
                {
                    user_id = user_id,
                    module = module,
                    source_id = source_id,
                    org_name = file.FileName,
                    file_type = file_type,
                    file_path = file_path,
                    upload_time = DateTime.Now
                });

                //如果是模块广告 且 批量上传是广告 且 广告id大于0
                if (module == "advertisement" && batch == "ad" && source_id > 0)
                {
                    _context.advertisement_pic_list.Add(new advertisement_pic_list_entity
                    {
                        ad_id = source_id,
                        pic = file_path
                    });
                }
                await _context.SaveChangesAsync();

                resp.code = 200;
                resp.msg = "上传成功";
                resp.data = new { path = file_path };
            }
            else
            {
                resp.code = 500;
                resp.msg = "上传文件出错";
            }
            //}
            return Json(resp);
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="file_path">文件路径</param>
        /// <param name="source_id">资源id</param>
        /// <returns></returns>
        public async Task<IActionResult> delete(string module, string file_path, int source_id)
        {
            var resp = new global_response();

            var org_file_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file_path.TrimStart('/'));
            if (System.IO.File.Exists(org_file_path))
            {
                System.IO.File.Delete(org_file_path);
            }

            //删除数据库文件
            await _context.attachment.Where(m => m.module == module & m.file_path == file_path).DeleteFromQueryAsync();

            resp.code = 200;
            resp.msg = "删除成功";

            return Json(resp);
        }
    }
}
