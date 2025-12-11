using EmployeeLeaveManagementSys.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).HasDefaultValue("Employee");


            });


            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.LeaveRequestId);
                entity.HasOne(e => e.Employee)
                      .WithMany(e => e.LeaveRequests)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

            }
            );

            modelBuilder.Entity<LeaveBalance>(entity =>
            {
                entity.HasKey(e => e.LeaveBalanceId);
                entity.HasOne(e => e.Employee)
                .WithOne(e => e.LeaveBalance)
                .HasForeignKey<LeaveBalance>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            });

              
        }

    }
}
