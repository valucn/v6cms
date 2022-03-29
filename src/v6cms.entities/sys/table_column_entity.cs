using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities.sys
{
    /// <summary>
    /// 代码生成表字段表
    /// </summary>
    [Table("sys_table_column")]
    public class table_column_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 代码生成表id
        /// </summary>
        public int code_generate_id { get; set; }

        /// <summary>
        /// 字段列名
        /// </summary>
        [Display(Name = "字段列名")]
        public string column_name { get; set; }

        /// <summary>
        /// 列描述
        /// </summary>
        [Display(Name = "列描述")]
        public string column_desc { get; set; }

        /// <summary>
        /// 字段显示名称
        /// </summary>
        [Display(Name = "字段显示名称")]
        public string column_display { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string data_type { get; set; }

        /// <summary>
        /// .net类型
        /// </summary>
        public string dotnet_type { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int data_length { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public int scale { get; set; }

        /// <summary>
        /// 是否标识
        /// </summary>
        public bool is_identity { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool is_pk { get; set; }

        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool is_nullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string default_value { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }
    }
}
