using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;
using v6cms.entities.sys;

namespace v6cms.entities
{
    /// <summary>
    /// 答疑回复表
    /// </summary>
    [Table("T_ask_reply")]
    public class ask_reply_entity
    {
        /// <summary>
        /// 答疑回复id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 答疑id
        /// </summary>
        [Display(Name = "答疑id")]
        public int ask_id { get; set; }

        /// <summary>
        /// 后台用户id
        /// </summary>
        [Display(Name = "后台用户id")]
        public int user_id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        public string content { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        [Display(Name = "回复时间")]
        public DateTime reply_time { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP")]
        public string ip { get; set; }

        /// <summary>
        /// 关联用户表
        /// </summary>
        [ForeignKey("user_id")]
        public virtual user_entity user { get; set; }
    }
}
