using v6cms.entities.sys;
using Microsoft.EntityFrameworkCore;

namespace v6cms.entities.db_set
{
    public class db_context : DbContext
    {
        //多个数据库应该使用这个构造函数，参数是上下文的集合
        public db_context(DbContextOptions<db_context> options) : base(options) { }
        //public db_context(DbContextOptions options) : base(options) { }

        /// <summary>
        /// 法律咨询提问
        /// </summary>
        public DbSet<ask_entity> ask { get; set; }

        /// <summary>
        /// 法律咨询答疑
        /// </summary>
        public DbSet<ask_reply_entity> ask_reply { get; set; }

        /// <summary>
        /// 法律咨询回复
        /// </summary>
        public DbSet<ask_comment_entity> ask_comment { get; set; }

        /// <summary>
        /// 广告
        /// </summary>
        public DbSet<advertisement_entity> advertisement { get; set; }

        /// <summary>
        /// 广告图片集合
        /// </summary>
        public DbSet<advertisement_pic_list_entity> advertisement_pic_list { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public DbSet<article_content_entity> article_content { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public DbSet<article_entity> article { get; set; }

        /// <summary>
        /// 附件资源
        /// </summary>
        public DbSet<attachment_entity> attachment { get; set; }

        /// <summary>
        /// 生日名单
        /// </summary>
        public DbSet<birthday_list_entity> birthday_list { get; set; }

        /// <summary>
        /// 网站栏目
        /// </summary>
        public DbSet<column_entity> column { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<comment_entity> comment { get; set; }

        /// <summary>
        /// 值班表配置
        /// </summary>
        public DbSet<duty_config_entity> duty_config { get; set; }

        /// <summary>
        /// 值班表
        /// </summary>
        public DbSet<duty_entity> duty { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public DbSet<ip_address_entity> ip_address { get; set; }

        /// <summary>
        /// 领导信箱
        /// </summary>
        public DbSet<leader_mailbox_entity> leader_mailbox { get; set; }

        /// <summary>
        /// 链接分类
        /// </summary>
        public DbSet<link_category_entity> link_category { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public DbSet<link_entity> link { get; set; }

        /// <summary>
        /// 会员
        /// </summary>
        public DbSet<member_entity> member { get; set; }

        /// <summary>
        /// 网站访问统计
        /// </summary>
        public DbSet<site_stat_entity> site_stat { get; set; }

        // ------------------- 以下是系统表 -------------------
        /// <summary>
        /// 代码生成
        /// </summary>
        public DbSet<code_generate_entity> code_generate { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public DbSet<dept_entity> dept { get; set; }

        /// <summary>
        /// 网站配置
        /// </summary>
        public DbSet<site_config_entity> site_config { get; set; }

        /// <summary>
        /// 表字段表
        /// </summary>
        public DbSet<table_column_entity> table_column { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<user_entity> user { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public DbSet<user_role_entity> user_role { get; set; }
    }
}
