using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// SSCMS栏目表
    /// </summary>
    [Table("siteserver_Channel")]
    public class sscms_Channel_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 计分分值
        /// </summary>
        [Column(TypeName = "decimal(16,2)")]
        public decimal score { get; set; }

    }
}
