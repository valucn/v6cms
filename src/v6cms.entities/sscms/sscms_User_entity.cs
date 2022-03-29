using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace v6cms.entities
{
    /// <summary>
    /// SSCMS用户表
    /// </summary>
    [Table("siteserver_User")]
    public class sscms_User_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 领导
        /// </summary>
        [Display(Name = "领导")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [Display(Name = "扩展字段")]
        public string ExtendValues { get; set; }
    }
}
