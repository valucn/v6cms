using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 广告表
    /// </summary>
    [Table("T_advertisement")]
    public class advertisement_entity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 广告名称
        /// </summary>
        [Display(Name = "广告名称")]
        public string ad_name { get; set; }

        /// <summary>
        /// 广告类型：文字广告=0, 图片广告=1, 飘窗广告=2, 浮动广告=3, 幻灯广告=4
        /// </summary>
        [Display(Name = "广告类型")]
        public ad_type_enum ad_type { get; set; }

        /// <summary>
        /// 显示时间限制：不限制=0, 到期不显示=1
        /// </summary>
        [Display(Name = "显示时间限制")]
        public view_time_limit_enum view_time_limit { get; set; }

        /// <summary>
        /// 广告文字
        /// </summary>
        [Display(Name = "广告文字")]
        public string text { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [Display(Name = "链接地址")]
        public string url { get; set; }

        /// <summary>
        /// 广告图片
        /// </summary>
        [Display(Name = "广告图片")]
        public string pic { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public DateTime? end_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 关联广告图片集合表
        /// </summary>
        [ForeignKey("ad_id")]
        public virtual List<advertisement_pic_list_entity> pic_list { get; set; }
    }
}
