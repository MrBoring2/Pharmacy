using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PharmacyApi.Data.DBData.EntityModels;

#nullable disable

namespace PharmacyApi.Data.DBData
{
    public partial class PharmacyContext : DbContext
    {
        public PharmacyContext()
        {
        }

        public PharmacyContext(DbContextOptions<PharmacyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Analizer> Analizers { get; set; }
        public virtual DbSet<AnalizerWork> AnalizerWorks { get; set; }
        public virtual DbSet<AuthenticationLogger> AuthenticationLoggers { get; set; }
        public virtual DbSet<InsuranceСompany> InsuranceСompanies { get; set; }
        public virtual DbSet<InvoicesIssued> InvoicesIssueds { get; set; }
        public virtual DbSet<LaboratoryService> LaboratoryServices { get; set; }
        public virtual DbSet<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Analizer>(entity =>
            {
                entity.Property(e => e.AnalizerName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AnalizerWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AnalizerWork");

                entity.Property(e => e.OrderDateOfReceipt).HasColumnType("datetime");

                entity.Property(e => e.OrderDateOfcompletion)
                    .HasColumnType("datetime")
                    .HasColumnName("OrderDateOFCompletion");

                entity.HasOne(d => d.Analizer)
                    .WithMany()
                    .HasForeignKey(d => d.AnalizerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalizerWork_Analizers");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalizerWork_Order");
            });

            modelBuilder.Entity<AuthenticationLogger>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AuthenticationLoggers_UserId");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthenticationLoggers)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<InsuranceСompany>(entity =>
            {
                entity.ToTable("InsuranceСompany");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Bic)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("BIC");

                entity.Property(e => e.CheckingAccount)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("INN");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvoicesIssued>(entity =>
            {
                entity.HasKey(e => new { e.InsuranceCompanyId, e.UserId });

                entity.ToTable("InvoicesIssued");

                entity.Property(e => e.EndPeriod).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.StartPeriod).HasColumnType("date");

                entity.HasOne(d => d.InsuranceCompany)
                    .WithMany(p => p.InvoicesIssueds)
                    .HasForeignKey(d => d.InsuranceCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoicesIssued_InsuranceСompany");
            });

            modelBuilder.Entity<LaboratoryService>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_LaboratiryServices");

                entity.Property(e => e.Code).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<LaboratoryServicesToOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.LaboratoryServiceId });

                entity.ToTable("LaboratoryServicesToOrder");

                entity.Property(e => e.DateOfFinished)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Analyzer)
                    .WithMany(p => p.LaboratoryServicesToOrders)
                    .HasForeignKey(d => d.AnalyzerId)
                    .HasConstraintName("FK_LaboratoryServicesToOrder_Analizers");

                entity.HasOne(d => d.LaboratoryService)
                    .WithMany(p => p.LaboratoryServicesToOrders)
                    .HasForeignKey(d => d.LaboratoryServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratoryServicesToOrder_LaboratiryServices");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.LaboratoryServicesToOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LaboratoryServicesToOrder_Order");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LaboratoryServicesToOrders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_LaboratoryServicesToOrder_Users");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfCreation)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Patients");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Ein)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EIN");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("GUID");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PassportNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassportSeries)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.SosialSecNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.SosialType)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ua)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("UA");

                entity.HasOne(d => d.InsuranceCompany)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.InsuranceCompanyId)
                    .HasConstraintName("FK_Patients_InsuranceСompany");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastEnterDate).HasColumnType("date");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.ServicesCodes)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
