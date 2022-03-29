using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities.sys
{
    /// <summary>
    /// 网站配置表
    /// </summary>
    [Table("sys_site_config")]
    public class site_config_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 网站名称
        /// </summary>
        [Display(Name = "网站名称")]
        public string site_name { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        [Display(Name = "关键词")]
        [MaxLength(150)]
        public string keywords { get; set; }

        /// <summary>
        /// 网页描述
        /// </summary>
        [Display(Name = "网页描述")]
        [MaxLength(150)]
        public string description { get; set; }

        /// <summary>
        /// 网站地址，不以/结尾
        /// </summary>
        [Display(Name = "网站地址")]
        public string site_url { get; set; } = "colour";

        /// <summary>
        /// 网站颜色：彩色=colour,黑白=gray
        /// </summary>
        [Display(Name = "网站颜色")]
        [StringLength(20)]
        public string site_color { get; set; }

        /// <summary>
        /// 版权内容，支持html
        /// </summary>
        [Display(Name = "版权内容")]
        public string copyright { get; set; }

        /// <summary>
        /// 网站备案号
        /// </summary>
        [Display(Name = "网站备案号")]
        public string icp { get; set; }

        /// <summary>
        /// 统计代码
        /// </summary>
        [Display(Name = "统计代码")]
        public string count_code { get; set; }

        /// <summary>
        /// 领导评论置顶天数
        /// </summary>
        [Display(Name = "领导评论置顶天数")]
        public int comment_top_days { get; set; } = 15;
    }
}
