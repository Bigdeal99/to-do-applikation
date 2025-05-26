using Microsoft.EntityFrameworkCore;
using SecureTodoApi.Models;

namespace SecureTodoApi.Data
{
    
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ActionType).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.IpAddress).IsRequired();
                entity.Property(e => e.UserAgent).IsRequired();
                entity.Property(e => e.EntityType).IsRequired();
                entity.Property(e => e.ActionDetails).IsRequired();
            });
        }
    }
}
