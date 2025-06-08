// Data Access Layer and Database Context for the Eagle Eye system
using Microsoft.EntityFrameworkCore;
using EagleEye.DAL.Models;

namespace EagleEye.DAL.Database
{

    // Entity Framework database context for the Eagle Eye system
    public class EagleEyeDbContext : DbContext
    {
        public EagleEyeDbContext(DbContextOptions<EagleEyeDbContext> options) : base(options) { }

        public DbSet<Agent> Agents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Agent>(entity =>
            {
                entity.ToTable("agents");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("codename");

                entity.Property(e => e.RealName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("realname");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.MissionsCompleted)
                    .HasColumnName("missionscompleted")
                    .HasDefaultValue(0);

                entity.HasIndex(e => e.CodeName).IsUnique();
            });
        }
    }
}