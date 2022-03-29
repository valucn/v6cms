using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.mysql_entities.db_set
{
    /// <summary>
    /// dedecms分类信息表
    /// </summary>
    [Table("dede_addoninfos")]
    public class addoninfos_entity
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
        /// 文章标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 封面缩略图
        /// </summary>
        public string litpic { get; set; }

        /// <summary>
        /// 发布时间timestamp
        /// </summary>
        public int senddate { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string body { get; set; }
    }
}