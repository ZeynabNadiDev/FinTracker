using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Category.Infrastructure.Persistence.DBcontext
{
    public class CategoryDbContext : DbContext
    {
        public const string DefaultSchema = "category";

        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options)
        {
        }

        // Using the fully qualified name or adding a using statement for Category entity
        public DbSet<Domain.Entities.Category.Category> Categories => Set<Domain.Entities.Category.Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce the schema for all tables in this module
            modelBuilder.HasDefaultSchema(DefaultSchema);

            // Apply all configurations (Entities and Value Objects) automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryDbContext).Assembly);
        }
    }
}
