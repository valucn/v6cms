using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.mysql_entities.db_set
{
    /// <summary>
    /// dedecms栏目表
    /// </summary>
    [Table("dede_arctype")]
    public class arctype_entity
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string typename { get; set; }

        /// <summary>
        /// 内容模型：分类信息=-8, 普通文章=1, 图片集=2, 软件=3, 商品=6
        /// </summary>
        public int channeltype { get; set; }

        /// <summary>
        /// 生成html文件名规则
        /// </summary>
        public string namerule { get; set; }
    }
}