using Microsoft.EntityFrameworkCore;
using SFTPFileCheckerWithHostedService.Model;

namespace SFTPFileCheckerWithHostedService.Data
{
    public class AppsDbContext : DbContext
    {
        public AppsDbContext(DbContextOptions<AppsDbContext> options)
          : base(options)
        {
        }
        public DbSet<FileHistory> FileHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
