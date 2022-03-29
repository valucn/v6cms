using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// SSCMS文章内容表
    /// </summary>
    [Table("siteserver_20220117_1")]
    public class sscms_content_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 栏目id
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 管理员id
        /// </summary>
        public int adminId { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool Top { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime? comment_time { get; set; }

        /// <summary>
        /// 关联栏目表
        /// </summary>
        [ForeignKey("ChannelId")]
        public sscms_Channel_entity sscms_Channel { get; set; }
    }
}
