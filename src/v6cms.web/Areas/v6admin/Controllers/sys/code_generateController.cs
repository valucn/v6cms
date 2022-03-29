using v6cms.entities.db_set;
using v6cms.entities.sys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.AspNetCore;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.sys
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    [Area("v6admin")]
    [admin_role_filter(authority_code = "code_generate/index")]
    public class code_generateController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly db_context _context;

        public code_generateController(db_context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [admin_role_filter(authority_code = "code_generate/index")]
        public async Task<IActionResult> index(int page = 1)
        {
            int page_size = 15;
            var query = _context.code_generate.AsQueryable();
            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, page_size);
            return View(model);
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "code_generate/generate", is_ajax = true)]
        public async Task<IActionResult> generate(int id)
        {
            var model = await _context.code_generate.FirstOrDefaultAsync(m => m.id == id);
            if (model == null)
            {
                return NotFound();
            }
            string controller_dir = "wwwroot/uploads/code/Controllers";
            if (!System.IO.Directory.Exists(controller_dir))
            {
                System.IO.Directory.CreateDirectory(controller_dir);
            }
            string views_dir = $"wwwroot/uploads/code/Views/{model.business_name}";
            if (!System.IO.Directory.Exists(views_dir))
            {
                System.IO.Directory.CreateDirectory(views_dir);
            }

            var sb_index_thead = new StringBuilder();
            var sb_index_tbody = new StringBuilder();
            var sb_form_item = new StringBuilder();

            //生成表实体类代码
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine("using v6cms.entities.enums;");
            sb.AppendLine("");
            sb.AppendLine("namespace v6cms.entities");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {model.table_desc}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    [Table(\"{model.table_name}\")]");
            sb.AppendLine($"    public class {model.model_name}");
            sb.AppendLine("    {");
            var columns = await _context.table_column.Where(m => m.code_generate_id == model.id).ToListAsync();
            foreach (var item in columns)
            {
                //index.cshtml
                sb_index_thead.AppendLine("                    <th>");
                sb_index_thead.AppendLine($"                        @Html.DisplayNameFor(model => model.{item.column_name})");
                sb_index_thead.AppendLine("                    </th>");
                sb_index_tbody.AppendLine("                        <td>");
                sb_index_tbody.AppendLine($"                            @Html.DisplayFor(modelItem => item.{item.column_name})");
                sb_index_tbody.AppendLine("                        </td>");

                //实体类
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {item.column_desc}");
                sb.AppendLine("        /// </summary>");
                if (item.is_pk)
                {
                    sb.AppendLine("        [Key]");
                }
                else
                {
                    sb.AppendLine($"        [Display(Name = \"{item.column_display}\")]");

                    //create/edit.cshtml
                    sb_form_item.AppendLine("            <div class=\"layui-form-item\">");
                    sb_form_item.AppendLine("                <div class=\"layui-inline\">");
                    sb_form_item.AppendLine($"                    <label asp-for=\"{item.column_name}\" class=\"layui-form-label\">{item.column_display}</label>");
                    sb_form_item.AppendLine("                    <div class=\"layui-input-inline\">");
                    sb_form_item.AppendLine($"                        <input asp-for=\"{item.column_name}\" class=\"layui-input\" />");
                    sb_form_item.AppendLine($"                        <span asp-validation-for=\"{item.column_name}\" class=\"text-danger\"></span>");
                    sb_form_item.AppendLine("                    </div>");
                    sb_form_item.AppendLine("                </div>");
                    sb_form_item.AppendLine("            </div>");
                }
                if (item.data_type == "decimal")
                {
                    sb.AppendLine("        [Column(TypeName = \"decimal(16,2)\")]");
                }
                string dotnet_type = item.dotnet_type;
                if (dotnet_type != "string" && item.is_nullable)
                {
                    dotnet_type += "?";
                }
                sb.AppendLine("        public " + dotnet_type + " " + item.column_name + " { get; set; }");
                sb.AppendLine("");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            string entity_code = sb.ToString();
            sb.Clear();

            //生成控制器代码
            sb.AppendLine("using v6cms.entities;");
            sb.AppendLine("using v6cms.entities.db_set;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc.Rendering;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using Webdiyer.AspNetCore;");
            sb.AppendLine("");
            sb.AppendLine("namespace v6cms.web.Areas.v6amin.Controllers");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {model.function_name}控制器");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    [Area(\"v6admin\")]");
            sb.AppendLine($"    [admin_role_filter(authority_code = \"{model.business_name}/index\")]");
            sb.AppendLine($"    public class {model.business_name}Controller : Controller");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly db_context _context;");
            sb.AppendLine("");
            sb.AppendLine($"        public {model.business_name}Controller(db_context context)");
            sb.AppendLine("        {");
            sb.AppendLine("            _context = context;");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// {model.function_name}列表");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine($"        [admin_role_filter(authority_code = \"{model.business_name}/index\")]");
            sb.AppendLine("        public async Task<IActionResult> index(int page = 1)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var query = _context.{model.business_name}.AsQueryable();");
            sb.AppendLine("            var model = await query.OrderByDescending(m => m.id).ToPagedListAsync(page, 15);");
            sb.AppendLine("            return View(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// 添加{model.function_name}");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine($"        [admin_role_filter(authority_code = \"{model.business_name}/create\")]");
            sb.AppendLine("        public async Task<IActionResult> create()");
            sb.AppendLine("        {");
            sb.AppendLine("            return View();");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// 添加{model.function_name}POST");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine($"        [admin_role_filter(authority_code = \"{model.business_name}/create\")]");
            sb.AppendLine($"        public async Task<IActionResult> create({model.model_name} model)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (ModelState.IsValid)");
            sb.AppendLine("            {");
            sb.AppendLine("                _context.Add(model);");
            sb.AppendLine("                await _context.SaveChangesAsync();");
            sb.AppendLine("                return RedirectToAction(nameof(index));");
            sb.AppendLine("            }");
            sb.AppendLine("            return View(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// 修改{model.function_name}");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine($"        [admin_role_filter(authority_code = \"{model.business_name}/edit\")]");
            sb.AppendLine("        public async Task<IActionResult> edit(int? id)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (id == null)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NotFound();");
            sb.AppendLine("            }");
            sb.AppendLine("");
            sb.AppendLine($"            var model = await _context.{model.business_name}.FindAsync(id);");
            sb.AppendLine("            if (model == null)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NotFound();");
            sb.AppendLine("            }");
            sb.AppendLine("");
            sb.AppendLine("            return View(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// 修改{model.function_name}POST");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine($"        [admin_role_filter(authority_code = \"{model.business_name}/edit\")]");
            sb.AppendLine($"        public async Task<IActionResult> edit(int id, {model.model_name} model)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (id != model.id)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NotFound();");
            sb.AppendLine("            }");
            sb.AppendLine("");
            sb.AppendLine("            if (ModelState.IsValid)");
            sb.AppendLine("            {");
            sb.AppendLine("                try");
            sb.AppendLine("                {");
            sb.AppendLine("                    _context.Update(model);");
            sb.AppendLine("                    await _context.SaveChangesAsync();");
            sb.AppendLine("                }");
            sb.AppendLine("                catch (DbUpdateConcurrencyException)");
            sb.AppendLine("                {");
            sb.AppendLine("                    if (!exists(model.id))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        return NotFound();");
            sb.AppendLine("                    }");
            sb.AppendLine("                    else");
            sb.AppendLine("                    {");
            sb.AppendLine("                        throw;");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("                return RedirectToAction(nameof(index));");
            sb.AppendLine("            }");
            sb.AppendLine("");
            sb.AppendLine("            return View(model);");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// 删除{model.function_name}");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine($"        [admin_role_filter(authority_code = \"{model.business_name}/delete\")]");
            sb.AppendLine("        public async Task<IActionResult> delete(int? id)");
            sb.AppendLine("        {");
            sb.AppendLine($"            await _context.{model.business_name}.Where(m => m.id == id).DeleteFromQueryAsync();");
            sb.AppendLine("            return RedirectToAction(nameof(index));");
            sb.AppendLine("        }");
            sb.AppendLine("");
            sb.AppendLine("        private bool exists(int id)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return _context.{model.business_name}.Any(m => m.id == id);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            string controller_code = sb.ToString();//控制器代码
            sb.Clear();

            //生成列表页面index.cshtml
            sb.AppendLine($"@model PagedList<{model.model_name}>");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"{model.function_name}管理\";");
            sb.AppendLine("}");
            sb.AppendLine("<div class=\"layui-card\">");
            sb.AppendLine("    <div class=\"layui-card-header\">");
            sb.AppendLine("        @ViewData[\"Title\"]");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"layui-card-body\">");
            sb.AppendLine($"        <a asp-action=\"create\" class=\"layui-btn layui-btn-normal\">添加{model.function_name}</a>");
            sb.AppendLine("        <table class=\"layui-table\">");
            sb.AppendLine("            <thead>");
            sb.AppendLine("                <tr>");
            sb.Append(sb_index_thead);
            sb.AppendLine("                    <th></th>");
            sb.AppendLine("                </tr>");
            sb.AppendLine("            </thead>");
            sb.AppendLine("            <tbody>");
            sb.AppendLine("                @if (Model.Count() == 0)");
            sb.AppendLine("                {");
            sb.AppendLine("                    <tr>");
            sb.AppendLine($"                        <td colspan=\"{(columns.Count() + 1)}\" style=\"text-align: center;\">");
            sb.AppendLine($"                            <div class=\"layui-inline\">--没有记录--</div>");
            sb.AppendLine($"                        </td>");
            sb.AppendLine("                    </tr>");
            sb.AppendLine("                }");
            sb.AppendLine("                @foreach (var item in Model)");
            sb.AppendLine("                {");
            sb.AppendLine("                    <tr>");
            sb.Append(sb_index_tbody);
            sb.AppendLine("                        <td style=\"text-align: right;\">");
            sb.AppendLine("                            <a asp-action=\"edit\" asp-route-id=\"@item.id\">修改</a> |");
            sb.AppendLine("                            <a asp-action=\"delete\" asp-route-id=\"@item.id\" onclick=\"return confirm('确认删除吗？');\">删除</a>");
            sb.AppendLine("                        </td>");
            sb.AppendLine("                    </tr>");
            sb.AppendLine("                }");
            sb.AppendLine("            </tbody>");
            sb.AppendLine("        </table>");
            sb.AppendLine("        <div style=\"text-align: center;padding: 16px 0 12px 0;\">");
            sb.AppendLine("            <div class=\"layui-inline\">");
            sb.AppendLine("            @Html.Pager(Model, new PagerOptions");
            sb.AppendLine("            {");
            sb.AppendLine("                PageIndexParameterName = \"page\",");
            sb.AppendLine("                TagName = \"ul\",");
            sb.AppendLine("                CssClass = \"pagination\",");
            sb.AppendLine("                CurrentPagerItemTemplate = \"<li class=\\\"page-item active\\\"><a  class=\\\"page-link\\\" href=\\\"javascript:void(0);\\\">{0}</a></li>\",");
            sb.AppendLine("                DisabledPagerItemTemplate = \"<li class=\\\"page-item disabled\\\"><a class=\\\"page-link\\\">{0}</a></li>\",");
            sb.AppendLine("                PagerItemTemplate = \"<li class=\\\"page-item\\\">{0}</li>\",");
            sb.AppendLine("                PagerItemCssClass = \"page-link\",");
            sb.AppendLine("                Id = \"bootstrappager\"");
            sb.AppendLine("            })");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");
            string index_code = sb.ToString();
            sb.Clear();

            //生成添加页面create.cshtml
            sb.AppendLine($"@model {model.model_name}");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"添加{model.function_name}\";");
            sb.AppendLine("}");
            sb.AppendLine("<div class=\"layui-card\">");
            sb.AppendLine("    <div class=\"layui-card-header\">@ViewData[\"Title\"]</div>");
            sb.AppendLine("    <div class=\"layui-card-body\" style=\"padding: 15px;\">");
            sb.AppendLine("        <form asp-action=\"create\" class=\"layui-form\">");
            sb.AppendLine("            <div asp-validation-summary=\"ModelOnly\" class=\"text-danger\"></div>");
            sb.Append(sb_form_item);
            sb.AppendLine("            <div class=\"layui-form-item layui-layout-admin\">");
            sb.AppendLine("                <div class=\"layui-input-block\">");
            sb.AppendLine("                    <div class=\"layui-footer\" style=\"left: 0;\">");
            sb.AppendLine("                        <button class=\"layui-btn\" lay-submit=\"\" lay-filter=\"component-form-demo1\">添加</button>");
            sb.AppendLine("                        <button type=\"reset\" class=\"layui-btn layui-btn-primary\">重置</button>");
            sb.AppendLine("                    </div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </form>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");
            string create_code = sb.ToString();
            sb.Clear();

            //生成修改页面edit.cshtml
            sb.AppendLine($"@model {model.model_name}");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"修改{model.function_name}\";");
            sb.AppendLine("}");
            sb.AppendLine("<div class=\"layui-card\">");
            sb.AppendLine("    <div class=\"layui-card-header\">@ViewData[\"Title\"]</div>");
            sb.AppendLine("    <div class=\"layui-card-body\" style=\"padding: 15px;\">");
            sb.AppendLine("        <form asp-action=\"edit\" class=\"layui-form\">");
            sb.AppendLine("            <div asp-validation-summary=\"ModelOnly\" class=\"text-danger\"></div>");
            sb.AppendLine("            <input type=\"hidden\" asp-for=\"id\" />");
            sb.Append(sb_form_item);
            sb.AppendLine("            <div class=\"layui-form-item layui-layout-admin\">");
            sb.AppendLine("                <div class=\"layui-input-block\">");
            sb.AppendLine("                    <div class=\"layui-footer\" style=\"left: 0;\">");
            sb.AppendLine("                        <button class=\"layui-btn\" lay-submit=\"\" lay-filter=\"component-form-demo1\">保存</button>");
            sb.AppendLine("                        <button type=\"reset\" class=\"layui-btn layui-btn-primary\">重置</button>");
            sb.AppendLine("                    </div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </form>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");
            string edit_code = sb.ToString();
            sb.Clear();

            //说明
            sb.AppendLine("/v6cms.entities/db_set/db_context.cs 文件添加以下代码");
            sb.AppendLine("");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// {model.function_name}");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        public DbSet<" + model.model_name + "> " + model.business_name + " { get; set; }");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("/v6cms.utils/premission/authority_util.cs 文件添加以下代码");
            sb.AppendLine($"            //{model.function_name}管理");
            sb.AppendLine($"            var {model.business_name} = new List<authority_role_model.authority_model>();");
            sb.AppendLine("            " + model.business_name + ".Add(new authority_role_model.authority_model { key = \"" + model.business_name + "/index\", name = \"" + model.function_name + "列表\" });");
            sb.AppendLine("            " + model.business_name + ".Add(new authority_role_model.authority_model { key = \"" + model.business_name + "/create\", name = \"添加" + model.function_name + "\" });");
            sb.AppendLine("            " + model.business_name + ".Add(new authority_role_model.authority_model { key = \"" + model.business_name + "/edit\", name = \"修改" + model.function_name + "\" });");
            sb.AppendLine("            " + model.business_name + ".Add(new authority_role_model.authority_model { key = \"" + model.business_name + "/delete\", name = \"删除" + model.function_name + "\" });");
            sb.AppendLine("....");
            sb.AppendLine("            group.Add(new authority_role_model { role_name = \"" + model.function_name + "管理\", authority_list = " + model.business_name + " });");
            sb.AppendLine("");
            string readme = sb.ToString();

            System.IO.File.WriteAllText($"wwwroot/uploads/code/{model.model_name}.cs", entity_code);
            System.IO.File.WriteAllText($"{controller_dir}/{model.business_name}Controller.cs", controller_code);
            System.IO.File.WriteAllText($"{views_dir}/create.cshtml", create_code);
            System.IO.File.WriteAllText($"{views_dir}/edit.cshtml", edit_code);
            System.IO.File.WriteAllText($"{views_dir}/index.cshtml", index_code);
            System.IO.File.WriteAllText($"wwwroot/uploads/code/{model.business_name}说明.txt", readme);

            var resp = new global_response
            {
                code = 200,
                msg = "生成成功",
                success = true
            };
            return Json(resp);
        }

        /// <summary>
        /// 选择表导入
        /// </summary>
        /// <returns></returns>
        [admin_role_filter(authority_code = "code_generate/select_table")]
        public async Task<IActionResult> select_table()
        {
            var tables = await _context.code_generate.OrderBy(m => m.table_name).ToListAsync();

            var model = new List<code_generate_entity>();
            string strConn = Configuration.GetConnectionString("SqlServerConnection");
            string strSql = "select top 1000 ROW_NUMBER() OVER (ORDER BY a.name) AS No, a.name AS 表名称, isnull(g.[value],'-') AS 表描述 from sys.tables a left join sys.extended_properties g on (a.object_id = g.major_id AND g.minor_id = 0)";
            using (var conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (var cmd = new SqlCommand(strSql, conn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string table_name = dr["表名称"].ToString();
                            bool exist = tables.Select(m => m.table_name).Contains(table_name);
                            if (!exist && !table_name.StartsWith("sys_"))
                            {
                                model.Add(new code_generate_entity
                                {
                                    id = int.Parse(dr["No"].ToString()),
                                    table_name = table_name,
                                    table_desc = dr["表描述"].ToString()
                                });
                            }
                        }
                        dr.Close();
                    }
                }
                conn.Close();
            }

            return View(model);
        }

        /// <summary>
        /// 导入表
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="table_desc"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "code_generate/import_table")]
        public async Task<IActionResult> import_table(string table_name, string table_desc)
        {
            if (!exists(table_name))
            {
                string business_name = table_name;
                if (business_name.StartsWith("T_"))
                {
                    business_name = business_name.Substring(2, table_name.Length - 2);
                }
                string function_name = table_desc;
                if (function_name.EndsWith("表"))
                {
                    function_name = function_name.Substring(0, function_name.Length - 1);
                }
                var code = new code_generate_entity
                {
                    table_name = table_name,
                    table_desc = table_desc,
                    model_name = business_name + "_entity",
                    business_name = business_name,
                    function_name = function_name,
                    create_time = DateTime.Now
                };
                _context.code_generate.Add(code);
                await _context.SaveChangesAsync();
                string strConn = Configuration.GetConnectionString("SqlServerConnection");
                string strSql = @"SELECT CASE WHEN col.colorder = 1 THEN obj.name
                            ELSE ''
                            END AS 表名,
                            col.colorder AS 序号 ,
                            col.name AS 字段列名,
                            ISNULL(ep.[value], '') AS 列描述,
                            t.name AS 数据类型 ,
                            col.length AS 长度 ,
                            ISNULL(COLUMNPROPERTY(col.id, col.name, 'Scale'), 0) AS 小数位数 ,
                            CASE WHEN COLUMNPROPERTY(col.id, col.name, 'IsIdentity') = 1 THEN 'True'
                            ELSE 'False'
                            END AS 标识,
                            CASE WHEN EXISTS ( SELECT 1
                            FROM dbo.sysindexes si
                            INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id
                            AND si.indid = sik.indid
                            INNER JOIN dbo.syscolumns sc ON sc.id = sik.id
                            AND sc.colid = sik.colid
                            INNER JOIN dbo.sysobjects so ON so.name = si.name
                            AND so.xtype = 'PK'
                            WHERE sc.id = col.id
                            AND sc.colid = col.colid ) THEN 'True'
                            ELSE 'False'
                            END AS 主键 ,
                            CASE WHEN col.isnullable = 1 THEN 'True'
                            ELSE 'False'
                            END AS 允许空 ,
                            ISNULL(comm.text, '') AS 默认值
                            FROM dbo.syscolumns col
                            LEFT JOIN dbo.systypes t ON col.xtype = t.xusertype
                            inner JOIN dbo.sysobjects obj ON col.id = obj.id
                            AND obj.xtype = 'U'
                            AND obj.status >= 0
                            LEFT JOIN dbo.syscomments comm ON col.cdefault = comm.id
                            LEFT JOIN sys.extended_properties ep ON col.id = ep.major_id
                            AND col.colid = ep.minor_id
                            AND ep.name = 'MS_Description'
                            LEFT JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id
                            AND epTwo.minor_id = 0
                            AND epTwo.name = 'MS_Description'
                            WHERE obj.name = @table_name --表名
                            ORDER BY col.colorder";
                using (var conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(strSql, conn))
                    {
                        cmd.Parameters.Add("@table_name", System.Data.SqlDbType.NVarChar, 50).Value = table_name;
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string column_desc = dr["列描述"].ToString();
                                string column_display = column_desc;
                                if (column_display.Contains("："))
                                {
                                    column_display = column_display.Split('：')[0];
                                }
                                var column = new table_column_entity
                                {
                                    code_generate_id = code.id,
                                    column_name = dr["字段列名"].ToString(),
                                    column_desc = column_desc,
                                    column_display = column_display,
                                    data_type = dr["数据类型"].ToString(),
                                    data_length = int.Parse(dr["长度"].ToString()),
                                    scale = int.Parse(dr["小数位数"].ToString()),
                                    is_identity = bool.Parse(dr["标识"].ToString()),
                                    is_pk = bool.Parse(dr["主键"].ToString()),
                                    is_nullable = bool.Parse(dr["允许空"].ToString()),
                                    default_value = dr["默认值"].ToString()
                                };
                                if (column.data_type == "int")
                                {
                                    column.dotnet_type = "int";
                                }
                                else if (column.data_type == "nvarchar" || column.data_type == "char")
                                {
                                    column.dotnet_type = "string";
                                }
                                else if (column.data_type == "decimal")
                                {
                                    column.dotnet_type = "decimal";
                                }
                                else if (column.data_type == "bit")
                                {
                                    column.dotnet_type = "bool";
                                }
                                else if (column.data_type == "datetime")
                                {
                                    column.dotnet_type = "DateTime";
                                }
                                else
                                {
                                    column.dotnet_type = column.data_type;
                                }
                                _context.table_column.Add(column);
                            }
                            dr.Close();
                        }
                    }
                    conn.Close();
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(index));
        }

        [admin_role_filter(authority_code = "code_generate/edit")]
        public async Task<IActionResult> edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.code_generate.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            var columns = await _context.table_column.Where(m => m.code_generate_id == model.id).ToListAsync();
            ViewData["columns"] = columns;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [admin_role_filter(authority_code = "code_generate/edit")]
        public async Task<IActionResult> edit(int id, code_generate_entity model)
        {
            if (id != model.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.update_time = DateTime.Now;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!code_generate_entityExists(model.id))
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [admin_role_filter(authority_code = "code_generate/delete")]
        public async Task<IActionResult> delete(int? id)
        {
            await _context.table_column.Where(m => m.code_generate_id == id).DeleteFromQueryAsync();
            await _context.code_generate.Where(m => m.id == id).DeleteFromQueryAsync();
            return RedirectToAction(nameof(index));
        }

        private bool code_generate_entityExists(int id)
        {
            return _context.code_generate.Any(e => e.id == id);
        }

        private bool exists(string table_name)
        {
            return _context.code_generate.Any(e => e.table_name == table_name);
        }
    }
}
