namespace v6cms.models.page_views
{
    public class edit_password_model
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        public string old_password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        public string confirm_password { get; set; }
    }
}
