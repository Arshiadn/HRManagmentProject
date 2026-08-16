using HrApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data;

public class HrDbContext : IdentityDbContext<ApplicationUser>
{
    public HrDbContext(DbContextOptions<HrDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEmployee(modelBuilder);
        ConfigureDepartment(modelBuilder);
    }

    private void ConfigureEmployee(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Hr_Employees");
            entity.HasKey(e => e.Id);
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
        });
    }
}
