using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 问答表
    /// </summary>
    [Table("T_ask")]
    public class ask_entity
    {
        /// <summary>
        /// 问答主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        [Display(Name = "会员id")]
        public int member_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        public string content { get; set; }

        /// <summary>
        /// 提问时间
        /// </summary>
        [Display(Name = "提问时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP")]
        public string ip { get; set; }

        /// <summary>
        /// 是否通过审核
        /// </summary>
        [Display(Name = "是否通过审核")]
        public bool is_review { get; set; }

        /// <summary>
        /// 回复状态：未回复=0, 新回复=1, 回复已读=2
        /// </summary>
        [Display(Name = "回复状态")]
        public reply_status_enum reply_status { get; set; }

        /// <summary>
        /// 关联会员表
        /// </summary>
        [ForeignKey("member_id")]
        public virtual member_entity member { get; set; }

        /// <summary>
        /// 关联答疑表
        /// </summary>
        [ForeignKey("ask_id")]
        public virtual List<ask_reply_entity> reply { get; set; }
    }
}
