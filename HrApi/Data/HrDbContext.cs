using HrApi.Enums.Request;
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
    public DbSet<EmployeeContract> EmployeeContracts { get; set; }
    public DbSet<ContractStateHistory> ContractStateHistories { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<EmployeeShiftAssignment> ShiftAssignments { get; set; }
    public DbSet<EmployeeRequest> EmployeeRequests { get; set; }
    public DbSet<AssetAssignment> AssetAssignments { get; set; }
    public DbSet<CompanyAsset> CompanyAssets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureEmployee(modelBuilder);
        ConfigureDepartment(modelBuilder);
        ConfigureCandidate(modelBuilder);
        ConfigureEmployeeContract(modelBuilder);
        ConfigureShift(modelBuilder);
        ConfigureAttendanceRecord(modelBuilder);
        ConfigureShiftAssignment(modelBuilder);
        ConfigureEmployeeRequest(modelBuilder);
        ConfigureCompanyAsset(modelBuilder);
        ConfigureAssetAssignment(modelBuilder);
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
            .HasConversion<string>();

            entity.HasIndex(c => c.Email);

            entity.HasOne(c => c.Employee)
            .WithOne()
            .HasForeignKey<Candidate>(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
    private void ConfigureEmployeeContract(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeContract>(entity =>
        {
            entity.ToTable("EmployeeContracts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.EmployeeId)
                .IsRequired();

            entity.Property(x => x.ContractType)
                .IsRequired();

            entity.Property(x => x.Status)
                .IsRequired();

            entity.Property(x => x.StartDate)
                .IsRequired();

            entity.Property(x => x.EndDate)
                .IsRequired();

            entity.Property(x => x.ProbationEndDate)
                .IsRequired(false);

            entity.Property(x => x.BaseSalary)
                .IsRequired();

            entity.Property(x => x.Currency)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();

            entity.Property(x => x.RowVersion)
                .IsRowVersion();

            entity.HasOne(x => x.Employee)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    private void ConfigureShift(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>(entity =>
        {
            entity.ToTable("Shifts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(x => x.GraceMinutes)
            .IsRequired(); 

            entity.Property(x => x.IsActive)
            .IsRequired(); 

            entity.HasIndex(x => x.Name)
            .IsUnique();

            entity.ComplexProperty(x => x.WorkingHours, range =>
            {
                range.Property(r => r.Start)
                    .HasColumnName("StartTime");

                range.Property(r => r.End)
                    .HasColumnName("EndTime");
            });
        });
    }
    private void ConfigureAttendanceRecord(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.WorkDate)
                .IsRequired();

            entity.Property(x => x.CheckInAt)
                .IsRequired(false);

            entity.Property(x => x.CheckOutAt)
                .IsRequired(false);

            entity.Property(x => x.WorkedMinutes)
                .IsRequired();

            entity.Property(x => x.LateMinutes)
                .IsRequired();

            entity.Property(x => x.EarlyLeaveMinutes)
                .IsRequired();

            entity.Property(x => x.OvertimeMinutes)
                .IsRequired();

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.WorkDate
            })
            .IsUnique();

            entity.Property(x => x.RowVersion)
                .IsRowVersion();

            entity.HasOne(x => x.Employee)
                  .WithMany(x => x.AttendanceRecords)
                  .HasForeignKey(x => x.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
    private void ConfigureShiftAssignment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeShiftAssignment>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EffectiveFrom)
                .IsRequired();

            entity.Property(x => x.EffectiveTo)
                .IsRequired(false);

            entity.HasOne(x => x.Employee)
                .WithMany(x => x.ShiftAssignments)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Shift)
                .WithMany(x => x.EmployeeAssignments)
                .HasForeignKey(x => x.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.EffectiveFrom
            });
        });
    }
    private void ConfigureEmployeeRequest(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeRequest>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EmployeeId)
                  .IsRequired();

            entity.Property(x => x.Type)
                  .IsRequired()
                  .HasConversion<string>();

            entity.Property(x => x.Status)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasDefaultValue(RequestStatus.Draft);

            entity.Property(x => x.FromDate)
                  .IsRequired()
                  .HasColumnType("date");

            entity.Property(x => x.ToDate)
                  .IsRequired()
                  .HasColumnType("date");

            entity.Property(x => x.TotalDays)
                  .HasComputedColumnSql(
                    "DATEDIFF(DAY, [FromDate], [ToDate]) + 1",
                    stored: true);

            entity.Property(x => x.Purpose)
                  .HasMaxLength(2000);

            entity.Property(x => x.AttachmentPath)
                  .HasMaxLength(1000);

            entity.Property(x => x.Destination)
                  .HasMaxLength(1000);

            entity.HasOne(x => x.Employee)
                  .WithMany(x => x.Requests)
                  .HasForeignKey(x => x.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.Status
            });

            entity.Property(x => x.RowVersion)
                  .IsRowVersion();
        });
    }
    private void ConfigureCompanyAsset(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyAsset>(entity =>
        {
            entity.ToTable("CompanyAssets");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.AssetCode)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.AssetCode)
                .IsUnique();

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.SerialNumber)
                .HasMaxLength(100);

            entity.HasIndex(x => x.SerialNumber)
                .IsUnique()
                .HasFilter("[SerialNumber] IS NOT NULL");

            entity.Property(x => x.Type)
                .IsRequired();

            entity.Property(x => x.Status)
                .IsRequired();

            entity.Property(x => x.RowVersion)
                .IsRowVersion();
        });
    }
    private void ConfigureAssetAssignment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetAssignment>(entity =>
        {
            entity.ToTable("AssetAssignments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.AssignedAt)
                .IsRequired();

            entity.Property(x => x.AssignmentNote)
                .HasMaxLength(500);

            entity.Property(x => x.ReturnNote)
                .HasMaxLength(500);

            entity.HasOne(x => x.Asset)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.AssetId,
                x.AssignedAt
            });
        });
    }
}
