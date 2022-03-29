using v6cms.entities.sys;
using Microsoft.EntityFrameworkCore;

namespace v6cms.entities.db_set
{
    //SSCMS数据库上下文
    public class sscms_db_context : DbContext
    {
        //多个数据库应该使用这个构造函数，参数是上下文的集合
        public sscms_db_context(DbContextOptions<sscms_db_context> options) : base(options) { }
        //public sscms_db_context(DbContextOptions options) : base(options) { }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<sscms_User_entity> siteserver_User { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public DbSet<sscms_content_entity> sscms_content { get; set; }

        /// <summary>
        /// 领导阅评
        /// </summary>
        public DbSet<sscms_comments_entity> comments { get; set; }

    }
}
