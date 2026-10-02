using HrApi.Enums.Request;
using HrApi.Models;
using HrApi.Models.Performance.Audit;
using HrApi.Models.Performance.Review;
using HrApi.Models.Performance.Rubric;
using HrApi.Models.Skill;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;

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
    public DbSet<PerformanceReview> PerformanceReviews { get; set; }
    public DbSet<ReviewPeriod> ReviewPeriods { get; set; }
    public DbSet<ReviewScore> ReviewScores { get; set; }
    public DbSet<ReviewRubric> ReviewRubrics { get; set; }
    public DbSet<RubricCriterion> RubricCriteria { get; set; }
    public DbSet<ReviewAuditEntry> ReviewAuditEntries { get; set; }
    public DbSet<Position> Positions { get; set;}
    public DbSet<Skill> Skills { get; set; }
    public DbSet<PositionSkill> PositionSkills { get; set; }
    public DbSet<EmployeeSkillState> EmployeeSkillStates { get; set; }
    public DbSet<SkillEvidence> SkillEvidences { get; set; }
    public DbSet<SkillAssessment> SkillAssessments { get; set; }
    public DbSet<SkillStateHistory> SkillStateHistories { get; set; }
    public DbSet<SkillEvidenceHistory> EvidenceHistories { get; set;}
    public DbSet<EmployeeSkillClaim> EmployeeSkillClaims { get; set;}
    public DbSet<PositionSkillHistory> PositionSkillHistories { get; set; }

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
        ConfigurePerformanceReview(modelBuilder);
        ConfigureReviewPeriod(modelBuilder);
        ConfigureReviewScore(modelBuilder);
        ConfigureReviewRubric(modelBuilder);
        ConfigureRubricCriterion(modelBuilder);
        ConfigureReviewAuditEntry(modelBuilder);
        ConfigurePosition(modelBuilder);
        ConfigureApplicationUser(modelBuilder);
        ConfigureSkill(modelBuilder);
        ConfigurePositionSkill(modelBuilder);
        ConfigureEmployeeSkillState(modelBuilder);
        ConfigureSkillEvidence(modelBuilder);
        ConfigureSkillAssessment(modelBuilder);
        ConfigureSkillStateHistory(modelBuilder);
        ConfigureSkillEvidenceHistory(modelBuilder);
        ConfigureEmployeeSkillClaim(modelBuilder);
        ConfigurePositionSkillHistory(modelBuilder);
    }

    private static void ConfigureEmployee(ModelBuilder modelbuilder)
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
    private static void ConfigureDepartment(ModelBuilder modelbuilder)
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
    private static void ConfigureCandidate(ModelBuilder modelBuilder)
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
    private static void ConfigureEmployeeContract(ModelBuilder modelBuilder)
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
    private static void ConfigureShift(ModelBuilder modelBuilder)
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
    private static void ConfigureAttendanceRecord(ModelBuilder modelBuilder)
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
    private static void ConfigureShiftAssignment(ModelBuilder modelBuilder)
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
    private static void ConfigureEmployeeRequest(ModelBuilder modelBuilder)
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
    private static void ConfigureCompanyAsset(ModelBuilder modelBuilder)
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
    private static void ConfigureAssetAssignment(ModelBuilder modelBuilder)
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
    private static void ConfigurePerformanceReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerformanceReview>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

            entity.Property(x => x.EmployeeComment)
            .HasMaxLength(2000);

            entity.Property(x => x.RubricSnapshotJson)
            .HasColumnType("nvarchar(max)");

            entity.Property(x => x.OverallScore)
            .HasPrecision(5, 2);

            entity.Property(x => x.RowVersion)
            .IsRowVersion();

            // ReviewPeriod relationship

            entity.HasOne<ReviewPeriod>()
            .WithMany()
            .HasForeignKey(x => x.ReviewPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

            // ReviewRubric relationship

            entity.HasOne<ReviewRubric>()
            .WithMany()
            .HasForeignKey(x => x.ReviewRubricId)
            .OnDelete(DeleteBehavior.Restrict);

            // Employee relationship

            entity.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ReviewScores relationship

            entity.HasMany(x => x.Scores)
            .WithOne(x => x.PerformanceReview)
            .HasForeignKey(x => x.PerformanceReviewId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.ReviewPeriodId,
                x.EmployeeId
            })
            .IsUnique();
        });
    }
    private static void ConfigureReviewScore(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewScore>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CriterionCode)
            .HasMaxLength(50)
            .IsRequired();

            entity.Property(x => x.Score)
            .IsRequired();

            entity.Property(x => x.Comment)
            .HasMaxLength(1000);

            entity.HasIndex(x => new
            {
                x.PerformanceReviewId,
                x.CriterionCode
            });
        });
    }
    private static void ConfigureReviewPeriod(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewPeriod>(entity => 
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

            entity.Property(x => x.StartsOn)
            .IsRequired();

            entity.Property(x => x.EndsOn)
            .IsRequired();

            entity.Property(x => x.IsClosed)
            .IsRequired();

            entity.HasOne(x => x.SelectedRubric)
            .WithMany()
            .HasForeignKey(x => x.SelectedRubricId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.StartsOn);

            entity.HasIndex(x => x.EndsOn);
        });
    }
    private static void ConfigureReviewRubric(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewRubric>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Version)
            .IsRequired();

            entity.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

            entity.HasIndex(x => new
            {
                x.Name,
                x.Version
            })
            .IsUnique();
        });
    }
    private static void ConfigureRubricCriterion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RubricCriterion>(entity =>
        {
            entity.HasKey(x => new
            {
                x.ReviewRubricId,
                x.Code
            });

            entity.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

            entity.Property(x => x.Weight)
            .HasPrecision(5, 2)
            .IsRequired();

            entity.HasOne(x => x.ReviewRubric)
            .WithMany(x => x.Criteria)
            .HasForeignKey(x => x.ReviewRubricId)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
    private static void ConfigureReviewAuditEntry(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewAuditEntry>(entity =>
        {

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Action)
            .HasMaxLength(100)
            .IsRequired();

            entity.Property(x => x.FromStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

            entity.Property(x => x.ToStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

            entity.Property(x => x.ActorType)
            .HasMaxLength(50)
            .IsRequired();

            entity.Property(x => x.Reason)
                .HasMaxLength(1000);

            entity.Property(x => x.OccurredAt)
             .IsRequired();

            // PerformanceReview 1 -> Many ReviewAuditEntries

            entity.HasOne(x => x.PerformanceReview)
            .WithMany()
            .HasForeignKey(x => x.PerformanceReviewId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.PerformanceReviewId,
                x.OccurredAt
            });
        });
    }
    private static void ConfigurePosition(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Positions");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

            entity.Property(p => p.Description)
            .HasMaxLength(500);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.HasMany(x => x.Employees)
            .WithOne(x => x.Position)
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.Title)
            .IsUnique();
        });
    }
    private static void ConfigureApplicationUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasOne(u => u.Employee)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(u => u.EmployeeId)
            .IsUnique(); 
        });
    }
    private static void ConfigureSkill(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);
         
            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => x.Title)
                .IsUnique();
        });
    }
    private static void ConfigurePositionSkill(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PositionSkill>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Position)
                .WithMany(x => x.Skills)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Skill)
                .WithMany(x => x.Positions)
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.RequiredLevel)
                .IsRequired();

            entity.HasIndex(x => new
            {
                x.PositionId,
                x.SkillId
            })
            .IsUnique();
        });
    }
    private static void ConfigureEmployeeSkillState(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeSkillState>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Skill)
                .WithMany(x => x.EmployeeSkillStates)
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Employee)
                .WithMany(x => x.SkillStates)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.CurrentLevel)
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .IsRequired();

            entity.HasIndex(x => new
            {
                x.SkillId,
                x.EmployeeId
            })
            .IsUnique();
        });
    }
    private static void ConfigureSkillEvidence(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillEvidence>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Type)
                .IsRequired();

            entity.Property(x => x.IssuedAt)
                .IsRequired();


            entity.Property(x => x.ExpiresAt)
                .IsRequired(false);

            entity.HasIndex(x => new
            {
                x.SkillId,
                x.EmployeeId
            });
        });
    }
    private static void ConfigureSkillAssessment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillAssessment>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.SkillClaim)
                .WithMany()
                .HasForeignKey(x => x.SkillClaimId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Decision)
                .IsRequired();

            entity.Property(x => x.AssessorId)
                .IsRequired();

            entity.Property(x => x.Comment)
                .HasMaxLength(1000);

            entity.Property(x => x.AssessedAt)
                .IsRequired();

            entity.HasIndex(x => x.SkillClaimId)
                .IsUnique();
        });
    }
    private static void ConfigureSkillStateHistory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillStateHistory>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.SkillAssessment)
                .WithMany()
                .HasForeignKey(x => x.SkillAssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Reason)
                .HasMaxLength(500);

            entity.Property(x => x.OccurredAt)
                .IsRequired();

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.SkillId
            });
        });
    }
    private static void ConfigureSkillEvidenceHistory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillEvidenceHistory>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.SkillEvidence)
                .WithMany(x => x.History)
                .HasForeignKey(x => x.SkillEvidenceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Reason)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.ChangedByUserId)
                .IsRequired();

            entity.Property(x => x.ChangedAt)
                .IsRequired();
        });
    }
    private static void ConfigureEmployeeSkillClaim(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeSkillClaim>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(x => x.Evidence)
                .WithMany(x => x.Claims)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeSkillClaimEvidence",
                    right => right
                        .HasOne<SkillEvidence>()
                        .WithMany()
                        .HasForeignKey("SkillEvidenceId")
                        .OnDelete(DeleteBehavior.Restrict),
                    left => left
                        .HasOne<EmployeeSkillClaim>()
                        .WithMany()
                        .HasForeignKey("EmployeeSkillClaimId")
                        .OnDelete(DeleteBehavior.Restrict),
                    join =>
                    {
                        join.HasKey(
                            "EmployeeSkillClaimId",
                            "SkillEvidenceId");
                    });

            entity.Property(x => x.ClaimedLevel)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.SkillId
            });
        });
    }
    private static void ConfigurePositionSkillHistory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PositionSkillHistory>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.PositionSkill)
                .WithMany(x => x.History)
                .HasForeignKey(x => x.PositionSkillId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.OldRequiredLevel)
                .IsRequired();

            entity.Property(x => x.NewRequiredLevel)
                .IsRequired();

            entity.Property(x => x.Reason)
                .IsRequired();

            entity.Property(x => x.ChangedByUserId)
                .IsRequired();

            entity.Property(x => x.ChangedAt)
                .IsRequired();
        });
    }
}
