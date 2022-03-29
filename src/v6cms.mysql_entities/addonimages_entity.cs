using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.mysql_entities.db_set
{
    /// <summary>
    /// dedecms图集表
    /// </summary>
    [Table("dede_addonimages")]
    public class addonimages_entity
    {
        /// <summary>
        /// 文章id
        /// </summary>
        [Key]
        public int aid { get; set; }

        /// <summary>
        /// 栏目id
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 图片集
        /// </summary>
        public string imgurls { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string body { get; set; }
    }
}