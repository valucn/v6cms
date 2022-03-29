using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.mysql_entities.db_set
{
    /// <summary>
    /// dedecms文章内容表
    /// </summary>
    [Table("dede_addonarticle")]
    public class addonarticle_entity
    {
        /// <summary>
        /// 文章id
        /// </summary>
        [Key]
        public int aid { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string body { get; set; }
    }
}