using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.enums;

namespace v6cms.entities
{
    /// <summary>
    /// 附件资源表
    /// </summary>
    [Table("T_attachment")]
    public class attachment_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public int user_id { get; set; }

        /// <summary>
        /// 模块名称：文章=article, 广告=advertisement，链接=link
        /// </summary>
        [Display(Name = "模块名称")]
        public string module { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        [Display(Name = "资源id")]
        public int source_id { get; set; }

        /// <summary>
        /// 源文件名
        /// </summary>
        [Display(Name = "源文件名")]
        public string org_name { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Display(Name = "文件类型")]
        public string file_type { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Display(Name = "文件路径")]
        public string file_path { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [Display(Name = "上传时间")]
        public DateTime upload_time { get; set; }

    }
}
