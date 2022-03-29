using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.mysql_entities.db_set
{
    /// <summary>
    /// dedecms文章内容表
    /// </summary>
    [Table("dede_archives")]
    public class archives_entity
    {
        /// <summary>
        /// 文章id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 栏目id
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string writer { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 发布时间timestamp
        /// </summary>
        public int pubdate { get; set; }

        /// <summary>
        /// 关联文章内容表
        /// </summary>
        [ForeignKey("id")]
        public virtual addonarticle_entity addonarticle { get; set; }

        /// <summary>
        /// 关联文章图片集表
        /// </summary>
        [ForeignKey("id")]
        public virtual addonimages_entity addonimages { get; set; }
    }
}