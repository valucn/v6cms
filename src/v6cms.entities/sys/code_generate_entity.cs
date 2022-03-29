using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities.sys
{
    /// <summary>
    /// 代码生成表
    /// </summary>
    [Table("sys_code_generate")]
    public class code_generate_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        [Display(Name = "表名称")]
        public string table_name { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        [Display(Name = "表描述")]
        public string table_desc { get; set; }

        /// <summary>
        /// 实体类名称
        /// </summary>
        [Display(Name = "实体类名称")]
        public string model_name { get; set; }

        /// <summary>
        /// 业务名称(英文)
        /// </summary>
        [Display(Name = "业务名称(英文)")]
        public string business_name { get; set; }

        /// <summary>
        /// 功能名称(中文)
        /// </summary>
        [Display(Name = "功能名称(中文)")]
        public string function_name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string remark { get; set; }

        /// <summary>
        /// 导入时间
        /// </summary>
        [Display(Name = "导入时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? update_time { get; set; }
    }
}
