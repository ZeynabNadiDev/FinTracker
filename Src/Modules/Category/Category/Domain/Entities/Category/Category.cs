using Category.Domain.Entities.Category.Events;
using Category.Domain.Enums;
using FinTracker.SharedKernel.Domain;
using FinTracker.SharedKernel.Exceptions;
using System;

namespace Category.Domain.Entities.Category
{

    public class Category : AggregateRoot<int>
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public Guid UserId { get; private set; }
        public CategoryType Type { get; private set; }
        public bool IsRemoved { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Required by EF Core
        protected Category() : base(0)
        {
        }

        private Category(
            int id,
            string name,
            string? description,
            Guid userId,
            CategoryType type) : base(id)
        {
            EnsureNotEmpty(name, nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
            UserId = userId;
            Type = type;
            IsRemoved = false;
            CreatedAt = DateTime.UtcNow;
        }

        public static Category Create(
            string name,
            string? description,
            Guid userId,
            CategoryType type)
        {
            var category = new Category(0, name, description, userId, type);

            category.AddDomainEvent(new CategoryCreated(category));

            return category;
        }

        public long Update(
            string name,
            string? description,
            CategoryType type)
        {
            if (IsRemoved)
                throw new DomainException("Removed category cannot be updated.");

            EnsureNotEmpty(name, nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
            Type = type;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new CategoryUpdated(this));

            return Version;
        }

        public long Remove()
        {
            if (IsRemoved)
                throw new DomainException("Category is already removed.");

            IsRemoved = true;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new CategoryDeleted(this));

            return Version;
        }

        private static void EnsureNotEmpty(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{fieldName} cannot be null or empty.");
        }
    }
}