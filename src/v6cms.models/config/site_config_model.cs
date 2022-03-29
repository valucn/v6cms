using System.ComponentModel;

namespace v6cms.models.config
{
    /// <summary>
    /// 网站配置
    /// </summary>
    public class site_config_model
    {
        /// <summary>
        /// 企业号接口
        /// </summary>
        [Description("企业号接口")]
        public string jundie_qy_api { get; set; }
    }
}
