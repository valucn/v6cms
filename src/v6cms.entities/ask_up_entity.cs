using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 问答点赞表
    /// </summary>
    [Table("T_ask_up")]
    public class ask_up_entity
    {
        /// <summary>
        /// 问答点赞主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 模块：文章=article, 问答=ask
        /// </summary>
        public string module { get; set; }

        /// <summary>
        /// 问答id
        /// </summary>
        [Display(Name = "问答id")]
        public int source_id { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        [Display(Name = "会员id")]
        public int member_id { get; set; }

        /// <summary>
        /// 游客连接id
        /// </summary>
        [Display(Name = "游客连接id")]
        public string connection_id { get; set; }

        /// <summary>
        /// 点赞时间
        /// </summary>
        [Display(Name = "点赞时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP")]
        public string ip { get; set; }
    }
}
