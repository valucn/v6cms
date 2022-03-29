using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using v6cms.entities.sys;

namespace v6cms.entities
{
    /// <summary>
    /// 文章表
    /// </summary>
    [Table("T_article")]
    public class article_entity
    {
        /// <summary>
        /// 文章主键id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 文章id雪花算法
        /// </summary>
        public long article_snow_id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int dept_id { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        [Display(Name = "路由值")]
        public string route_value { get; set; }

        /// <summary>
        /// 网站栏目id
        /// </summary>
        [Display(Name = "网站栏目id")]
        public int column_id { get; set; }

        /// <summary>
        /// 副栏目
        /// </summary>
        [Display(Name = "副栏目")]
        public string sub_column { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [Display(Name = "文章标题")]
        [MaxLength(140)]
        public string title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        public string author { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Display(Name = "来源")]
        public string source { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        [Display(Name = "概要")]
        [MaxLength(140)]
        public string summary { get; set; }

        /// <summary>
        /// 文章内容无html代码
        /// </summary>
        [Display(Name = "文章内容无html代码")]
        public string content_nohtml { get; set; }

        /// <summary>
        /// 内容页视图路径
        /// </summary>
        [Display(Name = "内容页视图路径")]
        public string details_view_path { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        public int views { get; set; }

        /// <summary>
        /// 是否通过审核
        /// </summary>
        [Display(Name = "是否通过审核")]
        public bool is_review { get; set; }

        /// <summary>
        /// 是否幻灯
        /// </summary>
        [Display(Name = "是否幻灯")]
        public bool is_slide { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [Display(Name = "是否置顶")]
        public bool is_top { get; set; }

        /// <summary>
        /// 是否精华文章
        /// </summary>
        [Display(Name = "是否精华文章")]
        public bool is_best { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [Display(Name = "是否推荐")]
        public bool is_recommend { get; set; }

        /// <summary>
        /// 是否特别推荐
        /// </summary>
        [Display(Name = "是否特别推荐")]
        public bool is_sr { get; set; }

        /// <summary>
        /// 是否热门
        /// </summary>
        [Display(Name = "是否热门")]
        public bool is_hot { get; set; }

        /// <summary>
        /// 是否图片文章
        /// </summary>
        [Display(Name = "是否图片文章")]
        public bool is_pic { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        [Display(Name = "封面图片")]
        public string pic { get; set; }

        /// <summary>
        /// 视频地址
        /// </summary>
        [Display(Name = "视频地址")]
        public string video { get; set; }

        /// <summary>
        /// 是否限制ip
        /// </summary>
        [Display(Name = "是否限制ip")]
        public bool is_limit_ip { get; set; }

        /// <summary>
        /// 公安部采用
        /// </summary>
        [Display(Name = "公安部采用")]
        public bool use_gab { get; set; }

        /// <summary>
        /// 省厅采用
        /// </summary>
        [Display(Name = "省厅采用")]
        public bool use_province { get; set; }

        /// <summary>
        /// 市局采用
        /// </summary>
        [Display(Name = "市局采用")]
        public bool use_city { get; set; }

        /// <summary>
        /// 分局采用
        /// </summary>
        [Display(Name = "部局采用")]
        public bool use_branch { get; set; }

        /// <summary>
        /// HTML静态页面路径
        /// </summary>
        public string html_path { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Display(Name = "发布时间")]
        public DateTime publish_time { get; set; }

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
        /// 评论时间
        /// </summary>
        [Display(Name = "评论时间")]
        public DateTime? comment_time { get; set; }

        /// <summary>
        /// 关联应网站栏目表
        /// </summary>
        [ForeignKey("column_id")]
        public virtual column_entity column { get; set; }

        /// <summary>
        /// 关联部门表
        /// </summary>
        [ForeignKey("dept_id")]
        public virtual dept_entity dept { get; set; }

        /// <summary>
        /// 文章内容，不在数据库字段中
        /// </summary>
        [NotMapped]
        public string content { get; set; }
    }
}
