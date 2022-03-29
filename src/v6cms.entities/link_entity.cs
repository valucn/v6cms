using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 链接表
    /// </summary>
    [Table("T_link")]
    public class link_entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        [Display(Name = "分类id")]
        public int category_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "标题必须填写")]
        public string title { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        [Display(Name = "logo")]
        public string logo { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [Display(Name = "链接")]
        public string url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int sort_rank { get; set; }

        /// <summary>
        /// 字段粗细
        /// </summary>
        [Display(Name = "字段粗细")]
        public string font_weight { get; set; }


        /// <summary>
        /// 字段颜色
        /// </summary>
        [Display(Name = "字段颜色")]
        public string font_color { get; set; }

        /// <summary>
        /// 链接分类
        /// </summary>
        [Display(Name = "链接分类")]
        //通过link_model实体的link_category_model属性，可以找到一个link_category_model实体，说明link表是一对多关系中的从表
        public virtual link_category_entity link_category { get; set; }
    }
}