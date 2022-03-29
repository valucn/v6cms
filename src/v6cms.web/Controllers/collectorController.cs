using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using v6cms.entities;
using v6cms.entities.db_set;
using v6cms.utils;
using Yitter.IdGenerator;

namespace v6cms.web.Controllers
{
    /// <summary>
    /// 采集器控制器
    /// </summary>
    public class collectorController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly db_context _context;
        public collectorController(IMemoryCache cache, db_context context)
        {
            _cache = cache;
            _context = context;
        }

        /// <summary>
        /// 龙虎网
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> longhoo()
        {
            string[] urls = new string[] {
                //"http://news.longhoo.net/zhxw/index.html",
                "http://news.longhoo.net/zhxw/36.html",
                "http://news.longhoo.net/zhxw/37.html",
                "http://news.longhoo.net/zhxw/38.html",
                "http://news.longhoo.net/zhxw/39.html",
                "http://news.longhoo.net/zhxw/40.html",
                "http://news.longhoo.net/zhxw/41.html",
                "http://news.longhoo.net/zhxw/42.html",
                "http://news.longhoo.net/zhxw/43.html",
                "http://news.longhoo.net/zhxw/44.html",
                "http://news.longhoo.net/zhxw/45.html",
                "http://news.longhoo.net/zhxw/46.html",
                "http://news.longhoo.net/zhxw/47.html",
                "http://news.longhoo.net/zhxw/48.html",
                "http://news.longhoo.net/zhxw/49.html"};

            var client = new HttpClient();

            int count = 0;
            //foreach (var url in urls)
            for (int i = 750; i < 800; i++)
            {
                string url = $"http://news.longhoo.net/zhxw/{i}.html";
                string list_html = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(list_html);
                var doc_node = doc.DocumentNode;
                var list_node = doc_node.SelectNodes("//*[@class='listDes']/li");
                foreach (var node in list_node)
                {
                    var item_node = node.SelectSingleNode("div/a");
                    string href = item_node.Attributes["href"].Value;

                    string content_html = await client.GetAsync(href).Result.Content.ReadAsStringAsync();
                    var content_doc = new HtmlDocument();
                    content_doc.LoadHtml(content_html);
                    var content_doc_node = content_doc.DocumentNode;
                    var title_node = content_doc_node.SelectSingleNode("//h1[@class='title']");
                    string title = "";
                    if (title_node != null)
                    {
                        title = title_node.InnerText.Trim();
                    }
                    Console.WriteLine($"title={title}");

                    var create_time_node = content_doc_node.SelectSingleNode("//div[@class='message']");
                    DateTime create_time = DateTime.Now;
                    string create_time_str = "";
                    if (title_node != null)
                    {
                        create_time_str = create_time_node.InnerText.Trim();
                    }
                    DateTime.TryParse(create_time_str, out create_time);
                    Console.WriteLine($"create_time={create_time}");

                    var content_node = content_doc_node.SelectSingleNode("//div[@class='articalCont']");
                    string content = "";
                    if (title_node != null)
                    {
                        content = content_node.InnerHtml.Trim();
                    }
                    string content_nohtml = content.NoHTML();
                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content_nohtml))
                    {
                        var article_snow_id = YitIdHelper.NextId();
                        var article = new article_entity
                        {
                            article_snow_id = article_snow_id,
                            column_id = 2,
                            title = title,
                            create_time = create_time,
                            content_nohtml = content_nohtml,
                            is_review = true
                        };
                        _context.article.Add(article);
                        //保存得到文章主键id
                        await _context.SaveChangesAsync();
                        //保存文章内容
                        _context.article_content.Add(new article_content_entity
                        {
                            article_id = article.id,
                            content = content
                        });
                        await _context.SaveChangesAsync();
                    }

                    count++;
                }
                await _context.SaveChangesAsync();
            }
            return Content($"count={count}");
        }

        /// <summary>
        /// 龙虎网
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> weixin()
        {
            string[] urls = new string[] {
                "https://mp.weixin.qq.com/s?__biz=MjM5NDAwMTA2MA==&amp;mid=2695729619&amp;idx=1&amp;sn=8be0b6bd0210cee0d492ebdf20f7371f&amp;chksm=83d74818b4a0c10ef286b33bb7deb73226125f866ddb5b2781166066a69afef3705eabdb3b85&amp;scene=4#wechat_redirect"};

            var client = new HttpClient();

            int count = 0;
            foreach (var url in urls)
            {
                string list_html = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(list_html);
                var doc_node = doc.DocumentNode;
                var list_node = doc_node.SelectNodes("//*[@class='listDes']/li");
                foreach (var node in list_node)
                {
                    var item_node = node.SelectSingleNode("div/a");
                    string href = item_node.Attributes["href"].Value;

                    string content_html = await client.GetAsync(href).Result.Content.ReadAsStringAsync();
                    var content_doc = new HtmlDocument();
                    content_doc.LoadHtml(content_html);
                    var content_doc_node = content_doc.DocumentNode;
                    var title_node = content_doc_node.SelectSingleNode("//h1[@class='title']");
                    string title = "";
                    if (title_node != null)
                    {
                        title = title_node.InnerText.Trim();
                    }
                    Console.WriteLine($"title={title}");

                    var create_time_node = content_doc_node.SelectSingleNode("//div[@class='message']");
                    DateTime create_time = DateTime.Now;
                    string create_time_str = "";
                    if (title_node != null)
                    {
                        create_time_str = create_time_node.InnerText.Trim();
                    }
                    DateTime.TryParse(create_time_str, out create_time);
                    Console.WriteLine($"create_time={create_time}");

                    var content_node = content_doc_node.SelectSingleNode("//div[@class='articalCont']");
                    string content = "";
                    if (title_node != null)
                    {
                        content = content_node.InnerHtml.Trim();
                    }
                    string content_nohtml = content.NoHTML();
                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content_nohtml))
                    {
                        var article_snow_id = YitIdHelper.NextId();
                        var article = new article_entity
                        {
                            article_snow_id = article_snow_id,
                            column_id = 2,
                            title = title,
                            create_time = create_time,
                            content_nohtml = content_nohtml,
                            is_review = true
                        };
                        _context.article.Add(article);
                        //保存得到文章主键id
                        await _context.SaveChangesAsync();
                        //保存文章内容
                        _context.article_content.Add(new article_content_entity
                        {
                            article_id = article.id,
                            content = content
                        });
                        await _context.SaveChangesAsync();
                    }

                    count++;
                }
            }
            return Content($"count={count}");
        }
    }
}
