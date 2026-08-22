using HrApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data;

public class HrDbContext : IdentityDbContext<ApplicationUser>
{
    public HrDbContext(DbContextOptions<HrDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<RecruitmentStageHistory> RecruitmentStageHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEmployee(modelBuilder);
        ConfigureDepartment(modelBuilder);
        ConfigureCandidate(modelBuilder);
    }

    private void ConfigureEmployee(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Hr_Employees");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PersonnelCode)
                  .IsRequired()
                  .HasMaxLength(20);
            entity.HasIndex(e => e.PersonnelCode)
                  .IsUnique();
            entity.HasOne(e => e.Department)
                  .WithMany(e => e.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
    private void ConfigureDepartment(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Hr_Departments");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name)
                  .HasMaxLength(100)
                  .IsRequired();
            entity.Property(d => d.Description)
                  .HasMaxLength(500);
            // enforce uniqueness only for non-deleted rows
            entity.HasIndex(d => d.Name)
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0"); // SQL Server filtered index
            entity.HasQueryFilter(d => !d.IsDeleted);
        });
    }
    private void ConfigureCandidate(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.ToTable("Candidates");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.FullName)
            .HasMaxLength(200)
            .IsRequired();

            entity.Property(c => c.Email)
            .HasMaxLength(200)
            .IsRequired();

            entity.Property(c => c.PhoneNumber)
            .HasMaxLength(11)
            .IsRequired();

            entity.Property(c => c.Stage)
            .HasConversion<int>();

            entity.HasIndex(c => c.Email);

            entity.HasOne(c => c.Employee)
            .WithOne()
            .HasForeignKey<Candidate>(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
