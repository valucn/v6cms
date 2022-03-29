using v6cms.entities.db_set;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace v6cms.web
{
    /// <summary>
    /// 自定义动态路由
    /// </summary>
    public class SlugRouteValueTransformer : DynamicRouteValueTransformer
    {
        private readonly db_context _context;
        public SlugRouteValueTransformer(db_context context)
        {
            _context = context;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var requestPath = httpContext.Request.Path.Value;

            if (!string.IsNullOrEmpty(requestPath) && requestPath[0] == '/')
            {
                // Trim the leading slash
                requestPath = requestPath.Substring(1);
            }
            if (string.IsNullOrEmpty(requestPath))
            {
                return new RouteValueDictionary
                {
                    { "controller", "home" },
                    { "action", "index" },
                    { "id", "" }
                };
            }
            else if (requestPath.StartsWith("v6admin") || requestPath.StartsWith("api/") || requestPath.StartsWith("templates/")
                || requestPath.ToLower().EndsWith(".jpg") || requestPath.ToLower().EndsWith(".jpeg")
                || requestPath.ToLower().EndsWith(".png") || requestPath.ToLower().EndsWith(".gif")
                || requestPath.ToLower().EndsWith(".htm") || requestPath.ToLower().EndsWith(".html"))
            {
                return values;
            }
            else
            {
                //正式发布请注释该行
                //Console.WriteLine($"requestPath={requestPath}");
                if (requestPath.EndsWith('/'))
                {
                    requestPath = requestPath.Substring(0, requestPath.Length - 1);
                }

                //此处可以自定义 也可 通过查询数据库来确定路由到何处
                //栏目路由
                var column = await _context.column.Where(m => m.route_value == requestPath).FirstOrDefaultAsync();
                if (column != null)
                {
                    var route = new RouteValueDictionary
                    {
                        { "controller", "column" },
                        { "action", "article_list" },
                        { "id", column.id.ToString() }
                    };
                    return route;
                }

                string id = Regex.Match(requestPath, "(.*?)/(\\d+).html").Groups[2].Value;
                //Console.WriteLine($"id={id}");
                string column_article = Regex.Replace(requestPath, "(.*?)/(\\d+).html", "$1/{id}.html");
                //Console.WriteLine($"column_article={column_article}");
                //栏目文章路由
                var column2 = _context.column.Where(m => m.article_route == column_article).FirstOrDefault();
                if (column2 != null)
                {
                    var route = new RouteValueDictionary
                    {
                        { "controller", "article" },
                        { "action", "details" },
                        { "id", id }
                    };
                    return route;
                }

                //Console.WriteLine($"requestPath={requestPath}");
                ////文章路由
                //var article = _context.article.Where(m => m.route_value == requestPath).FirstOrDefault();
                //if (article != null)
                //{
                //    var route = new RouteValueDictionary
                //    {
                //        { "controller", "article" },
                //        { "action", "details" },
                //        { "id", article.id.ToString() }
                //    };
                //    return route;
                //}
            }

            //可修改的values
            return values;
        }
    }
}
