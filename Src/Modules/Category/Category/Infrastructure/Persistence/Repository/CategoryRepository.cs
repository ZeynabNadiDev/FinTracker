using Category.Domain.Repository;
using Category.Infrastructure.Persistence.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Infrastructure.Persistence.Repository
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDbContext _context;

        public CategoryRepository(CategoryDbContext context)
        {
            _context = context;
        }

        public async Task<Category.Domain.Entities.Category.Category?> GetByIdAsync(
            int id, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsRemoved, cancellationToken);
        }

        public async Task<IEnumerable<Category.Domain.Entities.Category.Category>> GetByUserIdAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId && !c.IsRemoved)
                .AsNoTracking() 
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Category.Domain.Entities.Category.Category category,
            CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
        }

        public void Update(Category.Domain.Entities.Category.Category category)
        {
            _context.Categories.Update(category);
        }

        public void Remove(Category.Domain.Entities.Category.Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
