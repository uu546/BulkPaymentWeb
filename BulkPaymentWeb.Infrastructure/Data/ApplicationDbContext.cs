using BulkPaymentWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulkPaymentWeb.Infrastructure.Data
{
    /// <summary>
    /// Класс контекста Ef Core.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PaymentRegistryEntity> Registries { get; set; }

        public DbSet<PaymentItemEntity> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentRegistryEntity>(entity =>
            {
                entity.ToTable("registries");

                entity.Property(e => e.Id).HasColumnName("id").IsRequired();

                entity.Property(e => e.FileName).HasColumnName("file_name").IsRequired();

                entity.Property(e => e.UploadedAt)
               .HasColumnName("uploaded_at")
               .HasColumnType("timestamp with time zone")
               .HasDefaultValueSql("now()")
               .IsRequired();

                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            });

            modelBuilder.Entity<PaymentItemEntity>(entity =>
            {
                entity.ToTable("payments");

                entity.Property(e => e.Id).HasColumnName("id").IsRequired();

                entity.Property(e => e.RegistryId)
                .HasColumnName("registry_id")
                .IsRequired();

                entity.HasOne(d => d.PaymentRegistryEntity)
                .WithMany(p => p.PaymentItemEntities)
                .HasForeignKey(d => d.RegistryId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Amount)
               .HasColumnName("amount")
               .HasPrecision(18, 2)
               .IsRequired();

                entity.Property(e => e.PayerInn)
               .HasColumnName("payer_inn")
               .HasMaxLength(12)
               .IsRequired();

                entity.Property(e => e.ReceiverInn)
               .HasColumnName("receiver_inn")
               .HasMaxLength(12)
               .IsRequired();

                entity.Property(e => e.PayerAccount)
               .HasColumnName("payer_account")
               .HasMaxLength(20)
               .IsRequired();

                entity.Property(e => e.ReceiverAccount)
               .HasColumnName("receiver_account")
               .HasMaxLength(20)
               .IsRequired();

                entity.Property(e => e.ReceiverBik)
               .HasColumnName("receiver_bik")
               .HasMaxLength(9)
               .IsRequired();

                entity.Property(e => e.Purpose)
               .HasColumnName("purpose")
               .HasMaxLength(500)
               .IsRequired();

                entity.Property(e => e.IsValid)
               .HasColumnName("is_valid")
               .IsRequired();

                entity.Property(e => e.ValidationError)
               .HasColumnName("validation_error");
            });
        }
    }
}
