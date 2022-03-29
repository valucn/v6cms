using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// IP地址表
    /// </summary>
    [Table("T_ip_address")]
    public class ip_address_entity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// IP地址开始
        /// </summary>
        [Display(Name = "IP地址开始")]
        [Required(ErrorMessage = "请填写")]
        public string ip_start { get; set; }

        /// <summary>
        /// IP结束
        /// </summary>
        [Display(Name = "IP结束")]
        [Required(ErrorMessage = "请填写")]
        public string ip_end { get; set; }

        /// <summary>
        /// IP类型
        /// </summary>
        [Display(Name = "IP类型")]
        public ip_type_enum ip_type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Required(ErrorMessage = "请填写")]
        public string remark { get; set; }
    }
}
