using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// 生日名单表
    /// </summary>
    [Table("T_birthday_list")]
    public class birthday_list_entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string real_name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        public DateTime? date_of_birth { get; set; }
    }
}
