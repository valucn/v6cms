using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 答疑评论表
    /// </summary>
    [Table("T_ask_comment")]
    public class ask_comment_entity
    {
        /// <summary>
        /// 答疑回复id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 评论方式：评论=comment, 点赞=up
        /// </summary>
        public string comment_type { get; set; }

        /// <summary>
        /// 答疑id
        /// </summary>
        [Display(Name = "答疑id")]
        public int ask_id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        [Display(Name = "会员id")]
        public int member_id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        public string comment_content { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        [Display(Name = "回复时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP")]
        public string ip { get; set; }

        /// <summary>
        /// 关联会员表
        /// </summary>
        [ForeignKey("member_id")]
        public virtual member_entity member { get; set; }
    }
}
