using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 文章内容表
    /// </summary>
    [Table("T_article_content")]
    public class article_content_entity
    {
        /// <summary>
        /// 文章主键id
        /// </summary>
        [Key]
        public int article_id { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [Display(Name = "文章内容")]
        public string content { get; set; }
    }
}
