using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 值班表配置表
    /// </summary>
    [Table("T_duty_config")]
    public class duty_config_entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 列序号
        /// </summary>
        [Display(Name = "列序号")]
        public string column_no { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Display(Name = "显示名称")]
        public string display_name { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        public bool is_show { get; set; }
    }
}
