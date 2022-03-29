using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// SSCMS领导阅评表
    /// </summary>
    [Table("sscms_comments")]
    public class sscms_comments_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        public string Guid { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Display(Name = "发布时间")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// 站点id主键
        /// </summary>
        [Display(Name = "站点id主键")]
        public int? SiteId { get; set; }

        /// <summary>
        /// 栏目id主键
        /// </summary>
        [Display(Name = "栏目id主键")]
        public int? ChannelId { get; set; }

        /// <summary>
        /// 内容id主键
        /// </summary>
        [Display(Name = "内容id主键")]
        public int ContentId { get; set; }

        /// <summary>
        /// 状态：审核通过=Approved
        /// </summary>
        [Display(Name = "状态")]
        public string Status { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "评论内容")]
        public string Content { get; set; }

        /// <summary>
        /// 领导姓名
        /// </summary>
        [Display(Name = "领导姓名")]
        public string leader_name { get; set; }
    }
}
