using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Category.Domain.Entities.Category;

namespace Category.Domain.Repository
{
    public interface ICategoryRepository
    {
        Task<Category.Domain.Entities.Category.Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category.Domain.Entities.Category.Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(Category.Domain.Entities.Category.Category category, CancellationToken cancellationToken = default);
        void Update(Category.Domain.Entities.Category.Category category);
        void Remove(Category.Domain.Entities.Category.Category category);
    }
}
