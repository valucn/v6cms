using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 会员表
    /// </summary>
    [Table("T_member")]
    public class member_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

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
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [MaxLength(20)]
        public string nick_name { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        [MaxLength(20)]
        public string real_name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        public string avatar { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "单位")]
        public string company { get; set; }

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
        /// 是否锁定
        /// </summary>
        [Display(Name = "是否锁定")]
        public bool is_lock { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        public DateTime reg_time { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>
        [Display(Name = "注册IP")]
        public string reg_ip { get; set; }

        /// <summary>
        /// 会员级别：注册会员=0, 审核会员=1
        /// </summary>
        [Display(Name = "会员级别")]
        public member_level_enum member_level { get; set; }

        /// <summary>
        /// 会员问题1
        /// </summary>
        [Display(Name = "会员问题1")]
        public string member_question1 { get; set; }

        /// <summary>
        /// 会员回答1
        /// </summary>
        [Display(Name = "会员回答1")]
        public string member_answer1 { get; set; }

        /// <summary>
        /// 会员问题2
        /// </summary>
        [Display(Name = "会员问题2")]
        public string member_question2 { get; set; }

        /// <summary>
        /// 会员回答2
        /// </summary>
        [Display(Name = "会员回答2")]
        public string member_answer2 { get; set; }

        /// <summary>
        /// 会员问题3
        /// </summary>
        [Display(Name = "会员问题3")]
        public string member_question3 { get; set; }

        /// <summary>
        /// 会员回答3
        /// </summary>
        [Display(Name = "会员回答3")]
        public string member_answer3 { get; set; }

    }
}
