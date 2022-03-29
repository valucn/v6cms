using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models.api;

namespace v6cms.web.Areas.v6admin.Controllers.api
{
    [Route("v6admin/api/[controller]/[action]")]
    [ApiController]
    public class duty_apiController : ControllerBase
    {
        private readonly db_context _context;

        public duty_apiController(db_context context)
        {
            _context = context;
        }

        [HttpPost]
        [admin_role_filter(authority_code = "duty_manager/import", is_ajax = true)]
        public async Task<object> import(IFormFile file, int worksheets = 0)
        {
            var resp = new global_response();
            //指定EPPlus使用非商业证书
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string root = "wwwroot";
            string dir = "/uploads/import_excel/";
            if (!Directory.Exists(Path.Combine(root + dir)))
            {
                Directory.CreateDirectory(Path.Combine(root + dir));
            }
            string file_name = $"{Guid.NewGuid().ToString("N")}.xlsx";
            FileInfo path = new FileInfo(Path.Combine(root + dir, file_name));

            using (FileStream fs = new FileStream(path.ToString(), FileMode.Create))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            int success_count = 0;
            using (ExcelPackage package = new ExcelPackage(path))
            {
                StringBuilder sb = new StringBuilder();
                ExcelWorksheet worksheet;
                try
                {
                    worksheet = package.Workbook.Worksheets[worksheets];
                }
                catch
                {
                    resp.code = 500;
                    resp.msg = "请使用新版本excel";
                    return resp;
                }
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 1; row <= rowCount; row++)
                {
                    if (row > 1)
                    {
                        var date_column = worksheet.Cells[row, 1];
                        if (date_column.Value == null)
                        {
                            Console.WriteLine($"数据为空，跳到下一行{row}");
                            continue;
                        }
                        DateTime date = DateTime.Now;

                        try
                        {
                            Console.WriteLine($"date_column.Value={date_column.Value}");
                            date = DateTime.Parse(date_column.Value.ToString());
                        }
                        catch//(date_column.Style.Numberformat.Format.IndexOf("yyyy") > -1 && date_column.GetType().ToString() == "System.Double")//注意这里，是处理日期时间格式的关键代码
                        {
                            date = date_column.GetValue<DateTime>();
                        }
                        string column_01 = Convert.ToString(worksheet.Cells[row, 2].Value);
                        string column_02 = Convert.ToString(worksheet.Cells[row, 3].Value);
                        string column_03 = Convert.ToString(worksheet.Cells[row, 4].Value);
                        string column_04 = Convert.ToString(worksheet.Cells[row, 5].Value);
                        string column_05 = Convert.ToString(worksheet.Cells[row, 6].Value);
                        string column_06 = Convert.ToString(worksheet.Cells[row, 7].Value);
                        string column_07 = Convert.ToString(worksheet.Cells[row, 8].Value);
                        string column_08 = Convert.ToString(worksheet.Cells[row, 9].Value);
                        string column_09 = Convert.ToString(worksheet.Cells[row, 10].Value);
                        string column_10 = Convert.ToString(worksheet.Cells[row, 11].Value);
                        string column_11 = Convert.ToString(worksheet.Cells[row, 12].Value);
                        string column_12 = Convert.ToString(worksheet.Cells[row, 13].Value);

                        _context.duty.Add(new duty_entity
                        {
                            date = date,
                            column_B = column_01,
                            column_C = column_02,
                            column_D = column_03,
                            column_E = column_04,
                            column_F = column_05,
                            column_G = column_06,
                            column_H = column_07,
                            column_I = column_08,
                            column_J = column_09,
                            column_K = column_10,
                            column_L = column_11,
                            column_M = column_12
                        });
                    }
                }
                success_count = await _context.SaveChangesAsync();
            }
            resp.code = 200;
            resp.msg = "success";
            resp.data = new
            {
                success_count
            };
            return resp;
        }
    }
}
