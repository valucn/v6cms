using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 值班表
    /// </summary>
    [Table("T_duty")]
    public class duty_entity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        public DateTime date { get; set; }

        /// <summary>
        /// B列
        /// </summary>
        [Display(Name = "B列")]
        public string column_B { get; set; }

        /// <summary>
        /// C列
        /// </summary>
        [Display(Name = "C列")]
        public string column_C { get; set; }

        /// <summary>
        /// D列
        /// </summary>
        [Display(Name = "D列")]
        public string column_D { get; set; }

        /// <summary>
        /// E列
        /// </summary>
        [Display(Name = "E列")]
        public string column_E { get; set; }

        /// <summary>
        /// F列
        /// </summary>
        [Display(Name = "F列")]
        public string column_F { get; set; }

        /// <summary>
        /// G列
        /// </summary>
        [Display(Name = "G列")]
        public string column_G { get; set; }

        /// <summary>
        /// H列
        /// </summary>
        [Display(Name = "H列")]
        public string column_H { get; set; }

        /// <summary>
        /// I列
        /// </summary>
        [Display(Name = "I列")]
        public string column_I { get; set; }

        /// <summary>
        /// J列
        /// </summary>
        [Display(Name = "J列")]
        public string column_J { get; set; }

        /// <summary>
        /// K列
        /// </summary>
        [Display(Name = "K列")]
        public string column_K { get; set; }

        /// <summary>
        /// L列
        /// </summary>
        [Display(Name = "L列")]
        public string column_L { get; set; }

        /// <summary>
        /// M列
        /// </summary>
        [Display(Name = "M列")]
        public string column_M { get; set; }

    }
}
