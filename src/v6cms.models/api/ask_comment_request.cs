namespace v6cms.models.api
{
    /// <summary>
    /// 提交答疑评论实体类
    /// </summary>
    public class ask_comment_request
    {
        /// <summary>
        /// 问题id
        /// </summary>
        public int ask_id { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string comment_content { get; set; }
    }
}
