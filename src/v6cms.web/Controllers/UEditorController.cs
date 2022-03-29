using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using V6UEditor.Core;

namespace v6cms.web.Controllers
{
    public class UEditorController : Controller
    {
        private readonly UEditorService _ueditorService;
        public UEditorController(UEditorService ueditorService)
        {
            this._ueditorService = ueditorService;
        }

        //如果是API，可以按MVC的方式特别指定一下API的URI
        [HttpGet, HttpPost]
        public ContentResult Upload()
        {
            var response = _ueditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }

        public IActionResult test()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("(2)请先登录");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageAsync(IFormFile file)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("(3)请先登录");
            }
            try
            {
                if (null == file)
                {
                    //Logger.LogError("file is null.");
                    return BadRequest();
                }

                if (file.Length > 0)
                {
                    var name = Path.GetFileName(file.FileName);
                    if (name != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);

                            // Add watermark
                            var watermarkedStream = new MemoryStream();
                            using (var img = Image.FromStream(stream))
                            {
                                using (var graphic = Graphics.FromImage(img))
                                {
                                    var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                                    var color = Color.FromArgb(128, 255, 255, 255);
                                    var brush = new SolidBrush(color);
                                    var point = new Point(img.Width - 120, img.Height - 30);

                                    graphic.DrawString("v6cms.cn", font, brush, point);
                                    img.Save(watermarkedStream, ImageFormat.Png);
                                }
                                //img.Save(hostingEnv.WebRootPath + "/" + name);

                            }
                            return StatusCode(StatusCodes.Status200OK);
                        }
                    }
                }
                return BadRequest();
            }
            catch (Exception)
            {
                //Logger.LogError(e, $"Error uploading image.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}