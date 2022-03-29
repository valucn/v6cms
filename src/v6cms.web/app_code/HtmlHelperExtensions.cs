using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace v6cms.web
{
    public static class HtmlHelperExtensions
    {
        public static string get_route(this IHtmlHelper helper, string key)
        {
            string value = helper.ViewContext.RouteData.Values[key].ToString();
            return value;
        }

        public static int get_route_int(this IHtmlHelper helper, string key)
        {
            string value_str = helper.ViewContext.RouteData.Values[key].ToString();
            int value;
            int.TryParse(value_str, out value);
            return value;
        }

        public static long get_route_long(this IHtmlHelper helper, string key)
        {
            string value_str = helper.ViewContext.RouteData.Values[key].ToString();
            long value;
            long.TryParse(value_str, out value);
            return value;
        }

        public static string get_query(this IHtmlHelper helper, string key)
        {
            string value = helper.ViewContext.HttpContext.Request.Query[key].ToString();
            return value;
        }

        public static int get_query_int(this IHtmlHelper helper, string key)
        {
            string value_str = helper.ViewContext.HttpContext.Request.Query[key].ToString();
            int value;
            int.TryParse(value_str, out value);
            return value;
        }

        public static string current_ip(this IHtmlHelper helper)
        {
            HttpContextAccessor context = new HttpContextAccessor();
            string request_client_ip = context.HttpContext?.Connection.RemoteIpAddress.ToString();
            if (request_client_ip == "::1")
            {
                request_client_ip = "127.0.0.1";
            }
            return request_client_ip;
        }
    }
}
