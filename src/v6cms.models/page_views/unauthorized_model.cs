namespace v6cms.models.page_views
{
    public class unauthorized_model
    {
        /// <summary>
        /// 显示消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 权限代码
        /// </summary>
        public string authority_code { get; set; }

        /// <summary>
        /// 跳转URL
        /// </summary>
        public string jump_url { get; set; }
    }
}
