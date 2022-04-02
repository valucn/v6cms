using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 评论表
    /// </summary>
    [Table("T_comment")]
    public class comment_entity
    {
        /// <summary>
        /// 评论主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 模块：文章=article, 问答=ask
        /// </summary>
        public string module { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public int member_id { get; set; }

        /// <summary>
        /// 栏目id
        /// </summary>
        public int column_id { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        [Display(Name = "资源id")]
        public int source_id { get; set; }

        /// <summary>
        /// 评论人姓名
        /// </summary>
        [Display(Name = "评论人姓名")]
        public string comment_name { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容")]
        public string comment_content { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        [Display(Name = "评论时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 评论IP
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 关联文章表
        /// </summary>
        [ForeignKey("source_id")]
        public article_entity article { get; set; }

        /// <summary>
        /// 关联问答表
        /// </summary>
        [ForeignKey("source_id")]
        public ask_entity ask { get; set; }

        /// <summary>
        /// 关联会员表
        /// </summary>
        [ForeignKey("member_id")]
        public member_entity member { get; set; }

    }
}
