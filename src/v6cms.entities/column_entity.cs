using v6cms.entities.enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

namespace v6cms.entities
{
    /// <summary>
    /// 网站栏目表
    /// </summary>
    [Table("T_column")]
    public class column_entity
    {
        /// <summary>
        /// id主键
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        [Display(Name = "路由值")]
        public string route_value { get; set; }

        /// <summary>
        /// 文章页路由
        /// </summary>
        [Display(Name = "文章页路由")]
        public string article_route { get; set; }

        /// <summary>
        /// 父栏目id
        /// </summary>
        [Display(Name = "父栏目id")]
        public int parent_id { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Display(Name = "级别")]
        public int level { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "栏目名称")]
        public string column_name { get; set; }

        /// <summary>
        /// 栏目名称简称
        /// </summary>
        [Display(Name = "栏目名称简称")]
        public string column_name_abbr { get; set; }

        /// <summary>
        /// 栏目介绍
        /// </summary>
        [Display(Name = "栏目介绍")]
        public string intro { get; set; }

        /// <summary>
        /// 栏目内容
        /// </summary>
        [Display(Name = "栏目内容")]
        public string content { get; set; }

        /// <summary>
        /// 栏目图片
        /// </summary>
        [Display(Name = "栏目图片")]
        public string pic { get; set; }

        /// <summary>
        /// 列表页视图路径
        /// </summary>
        [Display(Name = "列表页视图路径")]
        public string list_view_path { get; set; }

        /// <summary>
        /// 内容页视图路径
        /// </summary>
        [Display(Name = "内容页视图路径")]
        public string details_view_path { get; set; }

        /// <summary>
        /// html模板
        /// </summary>
        [Display(Name = "html模板")]
        public string html_template { get; set; }

        /// <summary>
        /// html路径规则
        /// </summary>
        [Display(Name = "html路径规则")]
        public string html_path_rule { get; set; }

        /// <summary>
        /// 列表选项：链接到默认页=0, 链接到列表第一页=1, 使用动态页=2
        /// </summary>
        [Display(Name = "列表选项")]
        public list_option_enum list_option { get; set; }

        /// <summary>
        /// 栏目属性：最终列表栏目=0, 栏目封面=1, 频道封面=2, 外部链接=3
        /// </summary>
        [Display(Name = "栏目属性")]
        public column_attribute_enum column_attribute { get; set; }

        /// <summary>
        /// 外部链接
        /// </summary>
        [Display(Name = "外部链接")]
        public string external_link { get; set; }

        /// <summary>
        /// 打开窗口
        /// </summary>
        [Display(Name = "打开窗口")]
        public string target { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [Display(Name = "是否推荐")]
        public bool is_recommend { get; set; }

        /// <summary>
        /// 是否显示在导航栏
        /// </summary>
        [Display(Name = "是否显示在导航栏")]
        public bool is_show_nav { get; set; }

        /// <summary>
        /// 发布文章是否需要审核
        /// </summary>
        [Display(Name = "发布文章是否需要审核")]
        public bool is_need_review { get; set; }

        /// <summary>
        /// 是否限制ip
        /// </summary>
        [Display(Name = "是否限制ip")]
        public bool is_limit_ip { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int sort_rank { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        [Display(Name = "分值")]
        [Column(TypeName = "decimal(16,2)")]
        public decimal score { get; set; }

        /// <summary>
        /// 公安部分值
        /// </summary>
        [Display(Name = "公安部分值")]
        [Column(TypeName = "decimal(16,2)")]
        public decimal score_gab { get; set; }

        /// <summary>
        /// 省厅分值
        /// </summary>
        [Display(Name = "省厅分值")]
        [Column(TypeName = "decimal(16,2)")]
        public decimal score_province { get; set; }

        /// <summary>
        /// 市局分值
        /// </summary>
        [Display(Name = "市局分值")]
        [Column(TypeName = "decimal(16,2)")]
        public decimal score_city { get; set; }

        /// <summary>
        /// 分局分值
        /// </summary>
        [Display(Name = "分局分值")]
        [Column(TypeName = "decimal(16,2)")]
        public decimal score_branch { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? update_time { get; set; }

        /// <summary>
        /// 父级栏目列表(位置导航用)
        /// </summary>
        [NotMapped]
        public List<column_entity> parent_list { get; set; }
    }
}
