using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// SSCMS管理员表
    /// </summary>
    [Table("siteserver_Administrator")]
    public class sscms_Administrator_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string DisplayName { get; set; }

    }
}
