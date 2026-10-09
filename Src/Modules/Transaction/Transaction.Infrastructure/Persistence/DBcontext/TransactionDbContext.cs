using FinTracker.SharedKernel.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransactionEntity = Transaction.Domain.Entities.Transaction.Transaction;

namespace Transaction.Infrastructure.Persistence.DBcontext
{
    public class TransactionDbContext : DbContext
    {
        public const string DefaultSchema = "transaction";
        private readonly IMediator _mediator;

        public TransactionDbContext(
            DbContextOptions<TransactionDbContext> options,
            IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce schema
            modelBuilder.HasDefaultSchema(DefaultSchema);

            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Fetch domain events from tracked aggregates
            var domainEntities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is AggregateRoot<Guid> aggregate && aggregate.DomainEvents.Any())
                .Select(x => (AggregateRoot<Guid>)x.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            // 2. Clear domain events so they aren't published again
            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            // 3. Save to database
            var result = await base.SaveChangesAsync(cancellationToken);

            // 4. Publish events to MediatR handlers
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
