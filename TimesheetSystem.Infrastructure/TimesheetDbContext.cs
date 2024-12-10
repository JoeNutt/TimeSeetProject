using Microsoft.EntityFrameworkCore;
using TimesheetSystem.Domain.Entities;

namespace TimesheetSystem.Infrastructure
{
    public class TimesheetDbContext : DbContext
    {
        public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options)
            : base(options)
        {
        }

        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimesheetEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.ProjectName).IsRequired();
                entity.Property(e => e.Description).IsRequired();

                entity.OwnsOne(e => e.HoursWorked, hp =>
                {
                    hp.Property(h => h.HoursWorked).HasColumnName("HoursWorked").IsRequired();
                });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}