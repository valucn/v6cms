using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 网站访问统计表
    /// </summary>
    [Table("T_site_stat")]
    public class site_stat_entity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        public DateTime date { get; set; }

        /// <summary>
        /// ip统计
        /// </summary>
        [Display(Name = "ip统计")]
        public int ip { get; set; }

        /// <summary>
        /// pv统计
        /// </summary>
        [Display(Name = "pv统计")]
        public int pv { get; set; }

    }
}
