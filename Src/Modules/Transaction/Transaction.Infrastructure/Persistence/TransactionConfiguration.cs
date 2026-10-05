using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionEntity = Transaction.Domain.Entities.Transaction.Transaction;

namespace Transaction.Infrastructure.Persistence
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.WalletId)
                .IsRequired();

            builder.Property(t => t.CategoryId)
                .IsRequired(false);

            // DestinationWalletId is optional (only set for transfers)
            builder.Property(t => t.DestinationWalletId)
                .IsRequired(false);

            builder.Property(t => t.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(t => t.Type)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.IsRemoved)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(t => t.UserId);
            builder.HasIndex(t => t.WalletId);
            builder.HasIndex(t => t.CategoryId);
            builder.HasIndex(t => new { t.UserId, t.TransactionDate });
            builder.HasIndex(t => t.IsRemoved);

            // Global Query Filter for soft delete
            builder.HasQueryFilter(t => !t.IsRemoved);

            // If DomainEvents property doesn't exist on Transaction entity, remove or comment this line:
            // builder.Ignore(t => t.DomainEvents);
        }
    }
}
