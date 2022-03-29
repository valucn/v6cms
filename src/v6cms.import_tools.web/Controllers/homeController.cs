using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.models;
using v6cms.mysql_entities.db_set;
using v6cms.utils;
using Yitter.IdGenerator;

namespace v6cms.import_tools.web.Controllers
{
    public class homeController : Controller
    {
        private readonly db_context _context;
        private readonly mysql_db_context _mysql;

        public homeController(db_context context, mysql_db_context mysql)
        {
            _context = context;
            _mysql = mysql;
        }

        public IActionResult index()
        {
            var arctype_list = _mysql.arctype.ToList().Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.id + "----" + x.typename
            });
            ViewData["arctype_list"] = arctype_list;

            var column_list = _context.column.ToList().Select(x => new SelectListItem
            {
                Value = x.id.ToString(),
                Text = x.id + "----" + x.column_name
            });
            ViewData["column_list"] = column_list;

            return View();
        }

        [HttpPost]
        public IActionResult index(int typeid, int column_id)
        {
            int count = 0;
            var arctype = _mysql.arctype.Where(m => m.id == typeid).FirstOrDefault();
            if (arctype != null)
            {
                if (arctype.channeltype == 1)
                {
                    var archives = _mysql.archives.Where(m => m.typeid == typeid).Include(m => m.addonarticle).ToList();
                    count = archives.Count();
                    foreach (var item in archives)
                    {
                        var pubdate = V6.ConvertToDateTime(item.pubdate);
                        string title = item.title;
                        string body = item.addonarticle.body;

                        var article_snow_id = item.id;// YitIdHelper.NextId();
                        var article = new article_entity
                        {
                            article_snow_id = article_snow_id,
                            column_id = column_id,
                            title = title,
                            content_nohtml = V6.NoHTML(body),
                            create_time = pubdate,
                            publish_time = pubdate,
                            is_review = true
                        };
                        _context.article.Add(article);
                        //保存得到文章主键id
                        _context.SaveChanges();
                        _context.article_content.Add(new article_content_entity
                        {
                            article_id = article.id,
                            content = body
                        });
                        _context.SaveChanges();
                        Console.WriteLine($"正在导入 ---->>>> {title} ....");
                    }
                }
                else if (arctype.channeltype == -8)
                {
                    var addoninfos = _mysql.addoninfos.Where(m => m.typeid == typeid).ToList();
                    count = addoninfos.Count();
                    foreach (var item in addoninfos)
                    {
                        var pubdate = V6.ConvertToDateTime(item.senddate);
                        string title = item.title;
                        string body = item.body;

                        var article_snow_id = item.aid;// YitIdHelper.NextId();
                        var article = new article_entity
                        {
                            article_snow_id = article_snow_id,
                            column_id = column_id,
                            title = title,
                            content_nohtml = V6.NoHTML(body),
                            create_time = pubdate,
                            publish_time = pubdate,
                            is_review = true
                        };
                        _context.article.Add(article);
                        //保存得到文章主键id
                        _context.SaveChanges();
                        _context.article_content.Add(new article_content_entity
                        {
                            article_id = article.id,
                            content = body
                        });
                        _context.SaveChanges();
                        Console.WriteLine($"正在导入 ---->>>> {title} ....");
                    }
                }
                else if (arctype.channeltype == 2)
                {
                    var archives = _mysql.archives.Where(m => m.typeid == typeid).Include(m => m.addonimages).ToList();
                    count = archives.Count();
                    foreach (var item in archives)
                    {
                        var pubdate = V6.ConvertToDateTime(item.pubdate);
                        string title = item.title;
                        string imgurls = item.addonimages.imgurls;
                        imgurls = Regex.Replace(imgurls, "{dede:pagestyle maxwidth='.*?' pagepicnum='.*?' ddmaxwidth='.*?' row='.*?' col='.*?' value='.*?'/}", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        imgurls = Regex.Replace(imgurls, "{dede:img ddimg='(.*?)' text='.*?' width='.*?' height='.*?'} .*? {/dede:img}", "<img src='$1' /><br />", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string body = imgurls;
                        body += item.addonimages.body;

                        var article_snow_id = item.id;// YitIdHelper.NextId();
                        var article = new article_entity
                        {
                            article_snow_id = article_snow_id,
                            column_id = column_id,
                            title = title,
                            content_nohtml = V6.NoHTML(body),
                            create_time = pubdate,
                            publish_time = pubdate,
                            is_review = true
                        };
                        _context.article.Add(article);
                        //保存得到文章主键id
                        _context.SaveChanges();
                        _context.article_content.Add(new article_content_entity
                        {
                            article_id = article.id,
                            content = body
                        });
                        _context.SaveChanges();
                        Console.WriteLine($"正在导入 ---->>>> {title} ....");
                    }
                }
            }
            return Content($"typeid={typeid}, column_id={column_id}, Count={count}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}