using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities.sys
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Table("sys_dept")]
    public class dept_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        [Display(Name = "父级id")]
        public int parent_id { get; set; }

        /// <summary>
        /// 祖级列表（半角逗号分隔）
        /// </summary>
        [Display(Name = "祖级列表（半角逗号分隔）")]
        public string ancestors { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "部门名称")]
        [StringLength(20)]
        public string dept_name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Display(Name = "显示顺序")]
        public int sort_rank { get; set; }

        /// <summary>
        /// 有权限栏目id列表集合
        /// </summary>
        public string column_id_list { get; set; }

        /// <summary>
        /// 是否参与排名
        /// </summary>
        [Display(Name = "是否参与排名")]
        public bool in_rank_list { get; set; }
    }
}
