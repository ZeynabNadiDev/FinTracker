using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallet.Infrastructure.Persistence
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet.Domain.Entities.Wallet.Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet.Domain.Entities.Wallet.Wallet> builder)
        {
            // Table name and schema
            builder.ToTable("Wallets", "wallet");

            // Primary Key
            builder.HasKey(w => w.Id);

            // Title
            builder.Property(w => w.Title)
                .HasMaxLength(100)
                .IsRequired();

            // Money Value Object Mapping (Balance & Currency)
            builder.OwnsOne(w => w.Balance, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Balance")
                    .HasPrecision(18, 2)
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(10)
                    .IsRequired();
            });

            // UserId
            builder.Property(w => w.UserId)
                .IsRequired();

            // Timestamps and Tracking
            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.Property(w => w.UpdatedAt)
                .IsRequired(false);

            // Global Query Filter for Soft Delete 
            builder.HasQueryFilter(w => !w.IsRemoved);
        }
    }
}
