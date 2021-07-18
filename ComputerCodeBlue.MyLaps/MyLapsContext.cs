using Microsoft.EntityFrameworkCore;

namespace ComputerCodeBlue.MyLaps
{
    public class MyLapsContext : DbContext
    {
        public DbSet<EventWithRunsResult> Events { get; set; }

        public MyLapsContext(DbContextOptions opts) : base (opts)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EventWithRunsResult>()
                .ToTable("Events");

            builder.Entity<GroupWithRunsResult>()
                .ToTable("Groups");

            builder.Entity<RunResult>()
                .ToTable("Runs");

            builder.Entity<ClassResult>()
                .ToTable("Classes");

            builder.Entity<DriverResult>()
                .ToTable("Drivers");

            builder.Entity<EventWithRunsResult>()
                .HasMany(e => e.Groups)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventId);

            builder.Entity<GroupWithRunsResult>()
                .HasMany(e => e.Runs)
                .WithOne(e => e.Group)
                .HasForeignKey(e => e.GroupId);

            builder.Entity<RunResult>()
                .HasMany(e => e.Classes)
                .WithOne(e => e.Run)
                .HasForeignKey(e => e.RunId);

            builder.Entity<ClassResult>()
                .HasMany(e => e.Results)
                .WithOne(e => e.Class)
                .HasForeignKey(e => e.ClassId);
        }
    }
}
