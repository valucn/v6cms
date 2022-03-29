namespace v6cms.models.api
{
    /// <summary>
    /// 提交评论实体类
    /// </summary>
    public class comment_request
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public int article_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string comment_content { get; set; }
    }
}
