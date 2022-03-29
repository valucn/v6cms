using System;
using System.Collections.Generic;
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
        /// id主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 栏目id
        /// </summary>
        public int column_id { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        [Display(Name = "文章id")]
        public int article_id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string name { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容")]
        public string comment_content { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        [Display(Name = "领导评论时间")]
        public DateTime comment_time { get; set; }

        /// <summary>
        /// 关联文章表
        /// </summary>
        [ForeignKey("article_id")]
        public article_entity article { get; set; }
    }
}
