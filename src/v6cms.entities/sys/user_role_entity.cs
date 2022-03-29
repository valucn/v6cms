using v6cms.entities.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities.sys
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("sys_user_role")]
    public class user_role_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        public string role_name { get; set; }

        /// <summary>
        /// 角色权限编码集
        /// </summary>
        [Display(Name = "角色权限编码集")]
        public string authority_code_list { get; set; }

        /// <summary>
        /// 级别：越小权限越大
        /// </summary>
        [Display(Name = "级别：越小权限越大")]
        public int level { get; set; }

        /// <summary>
        /// 数据范围：1全部数据权限，2自定数据权限，3本部门数据权限，4本部门及以下数据权限，5仅本人数据权限
        /// </summary>
        [Display(Name = "数据范围")]
        public data_range_enum data_range { get; set; }

        /// <summary>
        /// 发布文章是否需要审核
        /// </summary>
        [Display(Name = "发布文章是否需要审核")]
        public bool is_need_review { get; set; }
    }
}
