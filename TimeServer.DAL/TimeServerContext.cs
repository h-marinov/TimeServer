using Microsoft.EntityFrameworkCore;
using TimeServer.DAL.Entities;

namespace TimeServer.DAL
{
    public class TimeServerContext : DbContext
    {
        public DbSet<TimeRequestLogEntity> TimeRequestLogs { get; set; }

        public string DbPath { get; }

        public TimeServerContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "time.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeRequestLogEntity>()
                .HasKey(e => e.Id);
        }
    }
}
