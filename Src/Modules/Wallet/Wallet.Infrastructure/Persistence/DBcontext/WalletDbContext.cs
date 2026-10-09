using FinTracker.SharedKernel.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Wallet.Infrastructure.Persistence.DBcontext
{
    public class WalletDbContext : DbContext
    {
        private readonly IPublisher _publisher;

        public WalletDbContext(
            DbContextOptions<WalletDbContext> options,
            IPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }

        public DbSet<Wallet.Domain.Entities.Wallet.Wallet> Wallets => Set<Wallet.Domain.Entities.Wallet.Wallet>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("wallet");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Collect all Domain Events from tracked Aggregate Roots
            var domainEntities = ChangeTracker
                .Entries<AggregateRoot<Guid>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // 2. Clear domain events so they are not dispatched again
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            // 3. Save changes to database (commits wallet balance changes)
            var result = await base.SaveChangesAsync(cancellationToken);

            // 4. Publish domain events via MediatR to other modules
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
