
using Identity.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTracker.Identity.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name and schema
            builder.ToTable("Users", "identity");

            // Primary Key
            builder.HasKey(u => u.Id);

            // Configure Email Value Object
            builder.OwnsOne(u => u.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(256)
                    .IsRequired();

                // Unique index for Email
                emailBuilder.HasIndex(e => e.Value)
                    .IsUnique();
            });

            // First Name
            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            // Last Name
            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            // Phone Number (Optional)
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            // Password Hash
            builder.Property(u => u.PasswordHash)
                .IsRequired();

            // Timestamps and Tracking
            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);

            builder.Property(u => u.LastLogin)
                .IsRequired(false);

            // Global Query Filter for Soft Delete
            builder.HasQueryFilter(u => !u.IsRemoved);
        }
    }
}
