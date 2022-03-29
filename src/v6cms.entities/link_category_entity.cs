using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 链接分类表
    /// </summary>
    [Table("T_link_category")]
    public class link_category_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "分类名称必须填写")]
        public string category_name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int sort_rank { get; set; }

        /// <summary>
        /// 链接列表
        /// </summary>
        [ForeignKey("category_id")]
        //通过link_category_model实体的link_list属性，可以找到多个link_list实体，说明link_category表是一对多关系中的主表
        public virtual ICollection<link_entity> link_list { get; set; }

    }
}
