using Microsoft.AspNetCore.Http;
using System;
using System.Web;

namespace v6cms.utils
{
    public class cookie_util : ICookie
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public cookie_util(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取南京公安openid
        /// </summary>
        /// <param name="return_url">获取成功后跳转的URL</param>
        /// <returns></returns>
        public string get_njga_openid(string return_url)
        {
            string njga_openid = GetCookies("njga_openid");
            if (string.IsNullOrEmpty(njga_openid))
            {
                return_url = HttpUtility.UrlEncode(return_url);
                _httpContextAccessor.HttpContext.Response.Redirect("/weixin/njga_go?return_url=" + return_url);
            }
            return njga_openid;
        }

        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">cookie值</param>
        /// <param name="minutes">过期时长，单位：分钟</param>
        public void SetCookies(string key, string value, int minutes = 300)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public void DeleteCookies(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>返回对应的值</returns>
        public string GetCookies(string key)
        {
            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
    }
}
