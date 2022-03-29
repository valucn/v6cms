namespace v6cms.utils
{
    /// <summary>
    /// ICookie接口
    /// </summary>
    public interface ICookie
    {
        /// <summary>
        /// 获取南京公安openid
        /// </summary>
        /// <param name="return_url">获取成功后跳转的URL</param>
        string get_njga_openid(string return_url);

        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">cookie值</param>
        /// <param name="minutes">过期时长，单位：分钟</param>
        void SetCookies(string key, string value, int minutes = 300);

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键名</param>
        void DeleteCookies(string key);

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>返回对应的值</returns>
        string GetCookies(string key);
    }
}
