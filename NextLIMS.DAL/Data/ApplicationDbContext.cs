using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TenantDepartment> TenantDepartments { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<ConfirmationTestTemplate> ConfirmationTestTemplates { get; set; }
        public DbSet<TenantTest> TenantTests { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleTest> SampleTests { get; set; }
        public DbSet<EnumerationData> EnumerationData { get; set; }
        public DbSet<EnumerationDilution> EnumerationDilutions { get; set; }
        public DbSet<DetectionData> DetectionData { get; set; }
        public DbSet<SampleConfirmationTest> SampleConfirmationTests { get; set; }
        public DbSet<SampleWorkflow> SampleWorkflows { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<PasswordReset>passwordResets { get; set; }
        public DbSet<SampleType> SampleTypes { get; set; }
        public DbSet<TestSampleType> TestSampleTypes { get; set; }
        public DbSet<TenantTestSampleType> TenantTestSampleTypes { get; set; }
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.SubscriptionTier).IsRequired();
                e.Property(x => x.SubscriptionStatus).IsRequired();
                e.Property(x => x.SamplesUsedThisMonth).HasDefaultValue(0);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.CreatedBy);
            });

            // ── Role ────────────────────────────────────────────────────────────
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.Roles)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Permission ──────────────────────────────────────────────────────
            modelBuilder.Entity<Permission>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            // ── RolePermission ──────────────────────────────────────────────────
            modelBuilder.Entity<RolePermission>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.GrantedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();
                e.HasIndex(x => x.PermissionId);
                e.HasIndex(x => x.GrantedBy);

                e.HasOne(x => x.Role)
                 .WithMany(x => x.RolePermissions)
                 .HasForeignKey(x => x.RoleId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Permission)
                 .WithMany(x => x.RolePermissions)
                 .HasForeignKey(x => x.PermissionId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── User ────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.Email).IsRequired();
                e.Property(x => x.PasswordHash).IsRequired();
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.RoleId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.Users)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Role)
                 .WithMany(x => x.Users)
                 .HasForeignKey(x => x.RoleId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Client ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Client>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.NID).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => new { x.TenantId, x.NID }).IsUnique();
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.Clients)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Department ──────────────────────────────────────────────────────
            modelBuilder.Entity<Department>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            // ── TenantDepartment  (composite PK) ────────────────────────────────
            modelBuilder.Entity<TenantDepartment>(e =>
            {
                e.HasKey(x => new { x.TenantId, x.DepartmentId });
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.DepartmentId);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.TenantDepartments)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Department)
                 .WithMany(x => x.TenantDepartments)
                 .HasForeignKey(x => x.DepartmentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Test ────────────────────────────────────────────────────────────
            modelBuilder.Entity<Test>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.TestName).IsRequired();
                e.Property(x => x.TestType).IsRequired();
                e.HasIndex(x => x.DepartmentId);
                e.HasIndex(x => x.TenantId);

                e.HasOne(x => x.Department)
                 .WithMany(x => x.Tests)
                 .HasForeignKey(x => x.DepartmentId)
                 .OnDelete(DeleteBehavior.Restrict);

            });

            // ── ConfirmationTestTemplate ─────────────────────────────────────────
            modelBuilder.Entity<ConfirmationTestTemplate>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.ConfirmationTestName).IsRequired();
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.TestId);

                e.HasOne(x => x.Tenant)
                 .WithMany()
                 .HasForeignKey(x => x.TenantId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Test)
                 .WithMany(x => x.ConfirmationTestTemplates)
                 .HasForeignKey(x => x.TestId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── TenantTest ──────────────────────────────────────────────────────
            modelBuilder.Entity<TenantTest>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.Price).HasColumnType("decimal(18,4)");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => new { x.TenantId, x.TestId }).IsUnique();
                e.HasIndex(x => x.TestId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.TenantTests)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Test)
                 .WithMany(x => x.TenantTests)
                 .HasForeignKey(x => x.TestId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Sample ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Sample>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.SampleName).IsRequired();
                e.Property(x => x.SampleType).IsRequired();
                e.Property(x => x.Status).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => new { x.TenantId, x.Id }).IsUnique();
                e.HasIndex(x => x.ClientId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.Samples)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Client)
                 .WithMany(x => x.Samples)
                 .HasForeignKey(x => x.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── SampleTest ──────────────────────────────────────────────────────
            modelBuilder.Entity<SampleTest>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Status).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.SampleId);
                e.HasIndex(x => x.TenantTestId);
                e.HasIndex(x => x.AssignedToUserId);
                e.HasIndex(x => x.ApprovedBy);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.Sample)
                 .WithMany(x => x.SampleTests)
                 .HasForeignKey(x => x.SampleId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.TenantTest)
                 .WithMany(x => x.SampleTests)
                 .HasForeignKey(x => x.TenantTestId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.AssignedToUser)
                 .WithMany()
                 .HasForeignKey(x => x.AssignedToUserId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.ApprovedByUser)
                 .WithMany()
                 .HasForeignKey(x => x.ApprovedBy)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── SampleType ─────────────────────────────────────────────────
            modelBuilder.Entity<SampleType>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Name).IsUnique();
            });

            // ── TestSampleType ─────────────────────────────────────────────────
            modelBuilder.Entity<TestSampleType>(e =>
            {
                e.HasKey(x => new { x.TestId, x.SampleTypeId });

                e.HasOne(x => x.Test)
                 .WithMany(x => x.TestSampleTypes)
                 .HasForeignKey(x => x.TestId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.SampleType)
                 .WithMany(x => x.TestSampleTypes)
                 .HasForeignKey(x => x.SampleTypeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── TenantTestSampleType ─────────────────────────────────────────────────
            modelBuilder.Entity<TenantTestSampleType>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.HasIndex(x => new
                {
                    x.TenantTestId,
                    x.SampleTypeId
                }).IsUnique();

                e.HasOne(x => x.TenantTest)
                 .WithMany(x => x.TenantTestSampleTypes)
                 .HasForeignKey(x => x.TenantTestId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.SampleType)
                 .WithMany(x => x.TenantTestSampleTypes)
                 .HasForeignKey(x => x.SampleTypeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── EnumerationData ─────────────────────────────────────────────────
            modelBuilder.Entity<EnumerationData>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Weight).HasColumnType("decimal(18,4)");
                e.Property(x => x.DiluentAmount).HasColumnType("decimal(18,4)");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.SampleTest)
                 .WithOne(x => x.EnumerationData)
                 .HasForeignKey<EnumerationData>(x => x.SampleTestId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Tenant)
                 .WithMany()
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── EnumerationDilution ─────────────────────────────────────────────
            modelBuilder.Entity<EnumerationDilution>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.DilutionOrVolume).IsRequired();
                e.Property(x => x.DilutionType).IsRequired();
                e.Property(x => x.ColonyCount).IsRequired();
                e.Property(x => x.IsSelectedForCalculation).HasDefaultValue(false);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.EnumerationDataId);
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.EnumerationData)
                 .WithMany(x => x.EnumerationDilutions)
                 .HasForeignKey(x => x.EnumerationDataId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Tenant)
                 .WithMany()
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── DetectionData ───────────────────────────────────────────────────
            modelBuilder.Entity<DetectionData>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Weight).HasColumnType("decimal(18,4)");
                e.Property(x => x.MediaAmount).HasColumnType("decimal(18,4)");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.CreatedBy);

                e.HasOne(x => x.SampleTest)
                 .WithOne(x => x.DetectionData)
                 .HasForeignKey<DetectionData>(x => x.SampleTestId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Tenant)
                 .WithMany()
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── SampleConfirmationTest ──────────────────────────────────────────
            modelBuilder.Entity<SampleConfirmationTest>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.ConfirmationTestName).IsRequired();
                e.Property(x => x.Result).IsRequired();
                e.Property(x => x.DatePerformed).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.SampleTestId);
                e.HasIndex(x => x.PerformedBySeniorAnalystId);

                e.HasOne(x => x.SampleTest)
                 .WithMany(x => x.SampleConfirmationTests)
                 .HasForeignKey(x => x.SampleTestId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.PerformedBySeniorAnalyst)
                 .WithMany()
                 .HasForeignKey(x => x.PerformedBySeniorAnalystId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── SampleWorkflow ──────────────────────────────────────────────────
            modelBuilder.Entity<SampleWorkflow>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Flag).HasDefaultValue(false);
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.SampleId);
                e.HasIndex(x => x.AssignedToId);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.SampleWorkflows)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Sample)
                 .WithMany(x => x.SampleWorkflows)
                 .HasForeignKey(x => x.SampleId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.AssignedTo)
                 .WithMany()
                 .HasForeignKey(x => x.AssignedToId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── AuditLog ────────────────────────────────────────────────────────
            modelBuilder.Entity<AuditLog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.Property(x => x.Action).IsRequired();
                e.Property(x => x.Timestamp).HasDefaultValueSql("GETUTCDATE()");
                e.HasIndex(x => x.TenantId);
                e.HasIndex(x => x.SampleId);
                e.HasIndex(x => x.SampleTestId);
                e.HasIndex(x => x.UserId);

                e.HasOne(x => x.Tenant)
                 .WithMany(x => x.AuditLogs)
                 .HasForeignKey(x => x.TenantId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Sample)
                 .WithMany(x => x.AuditLogs)
                 .HasForeignKey(x => x.SampleId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.SampleTest)
                 .WithMany(x => x.AuditLogs)
                 .HasForeignKey(x => x.SampleTestId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
