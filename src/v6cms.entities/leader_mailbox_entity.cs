using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;
using v6cms.entities.sys;

namespace v6cms.entities
{
    /// <summary>
    /// 领导信箱表
    /// </summary>
    [Table("T_leader_mailbox")]
    public class leader_mailbox_entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public int user_id { get; set; }

        /// <summary>
        /// 留言人姓名
        /// </summary>
        [Display(Name = "留言人姓名")]
        public string guest_name { get; set; }

        /// <summary>
        /// 留言人单位
        /// </summary>
        [Display(Name = "留言人单位")]
        public string guest_unit { get; set; }

        /// <summary>
        /// 留言人电子邮箱
        /// </summary>
        [Display(Name = "留言人电子邮箱")]
        public string guest_email { get; set; }

        /// <summary>
        /// 留言人手机号码
        /// </summary>
        [Display(Name = "留言人手机号码")]
        [MaxLength(11)]
        public string guest_mobile { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        public string title { get; set; }

        /// <summary>
        /// 信件类型
        /// </summary>
        [Display(Name = "信件类型")]
        public string mail_type { get; set; }

        /// <summary>
        /// 回复方式：公开回复=0, 私下回复=1
        /// </summary>
        [Display(Name = "回复方式")]
        public reply_type_enum reply_type { get; set; }

        /// <summary>
        /// 信件内容
        /// </summary>
        [Display(Name = "信件内容")]
        public string content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        public string ip { get; set; }

        /// <summary>
        /// 写信时间
        /// </summary>
        [Display(Name = "写信时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [Display(Name = "回复内容")]
        public string reply_content { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        [Display(Name = "回复时间")]
        public DateTime? reply_time { get; set; }

        /// <summary>
        /// 关联用户表
        /// </summary>
        [ForeignKey("user_id")]
        public virtual user_entity user { get; set; }
    }
}
