using Microsoft.EntityFrameworkCore;

namespace v6cms.mysql_entities.db_set
{
    public class mysql_db_context : DbContext
    {
        public mysql_db_context(DbContextOptions options) : base(options) { }

        public virtual DbSet<addonarticle_entity> addonarticle { get; set; }

        public virtual DbSet<addonimages_entity> addonimages { get; set; }

        public virtual DbSet<addoninfos_entity> addoninfos { get; set; }

        public virtual DbSet<archives_entity> archives { get; set; }
        
        public virtual DbSet<arctype_entity> arctype { get; set; }
    }
}