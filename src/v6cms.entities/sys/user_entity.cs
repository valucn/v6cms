using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities.sys
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("sys_user")]
    public class user_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        [Display(Name = "角色id")]
        public int role_id { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [Display(Name = "部门id")]
        public int dept_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string real_name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        public string avatar { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        [Display(Name = "职务")]
        public string post { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        [Display(Name = "介绍")]
        public string intro { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        public string mobile { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        public string card_id { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        public DateTime? date_of_birth { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        [Display(Name = "是否锁定")]
        public bool is_lock { get; set; }

        /// <summary>
        /// 是否需要修改密码
        /// </summary>
        [Display(Name = "需要改密码")]
        public bool is_need_edit_password { get; set; }

        /// <summary>
        /// 是否开通领导信箱
        /// </summary>
        [Display(Name = "是否开通领导信箱")]
        public bool is_leader_mailbox { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int sort_rank { get; set; } = 50;

        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        [ForeignKey("role_id")]
        public virtual user_role_entity user_role { get; set; }
    }
}
