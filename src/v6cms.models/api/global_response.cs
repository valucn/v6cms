namespace v6cms.models.api
{
    public class global_response
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }
    }
}
