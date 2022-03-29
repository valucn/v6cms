using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 广告图片集合表
    /// </summary>
    [Table("T_advertisement_pic_list")]
    public class advertisement_pic_list_entity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int id { get; set; }
        
        /// <summary>
        /// 广告id
        /// </summary>
        public int ad_id { get; set; }

        /// <summary>
        /// 广告图片
        /// </summary>
        [Display(Name = "广告图片")]
        public string pic { get; set; }

    }
}
