using EmployeeShiftManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace EmployeeShiftManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Shift configuration
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.StartTime).IsRequired();
                entity.Property(s => s.EndTime).IsRequired();
                entity.Property(s => s.IsDeleted).HasDefaultValue(false);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(s => s.Employee)
                    .WithMany(e => e.Shifts)
                    .HasForeignKey(s => s.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
