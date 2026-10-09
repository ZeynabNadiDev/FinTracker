using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Persistence
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Domain.Entities.Budget.Budget>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Budget.Budget> builder)
        {
            builder.ToTable("Budgets", "budget");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedNever();

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.Property(b => b.CategoryId)
                .IsRequired();

            builder.Property(b => b.TargetAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(b => b.Month)
                .IsRequired();

            builder.Property(b => b.Year)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt);

            // Prevent duplicate budgets for the same category in the same month/year
            builder.HasIndex(b => new { b.UserId, b.CategoryId, b.Month, b.Year })
                .IsUnique();

            // Ignore domain events from EF Core mapping
            builder.Ignore(b => b.DomainEvents);
        }
    }
}
