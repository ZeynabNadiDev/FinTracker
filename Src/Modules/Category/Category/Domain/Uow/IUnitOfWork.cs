using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Domain.Uow
{
    public interface IUnitOfWork:IDisposable
    {
       Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
