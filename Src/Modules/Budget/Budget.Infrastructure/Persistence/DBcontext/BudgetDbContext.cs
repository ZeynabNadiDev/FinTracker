using FinTracker.SharedKernel.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Persistence.DBcontext
{
    public class BudgetDbContext : DbContext
    {
        private readonly IPublisher _publisher;

        public BudgetDbContext(
            DbContextOptions<BudgetDbContext> options,
            IPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }

        public DbSet<Budget.Domain.Entities.Budget.Budget> Budgets => Set<Budget.Domain.Entities.Budget.Budget>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Collect all domain events from tracked budget aggregates
            var domainEvents = ChangeTracker
                .Entries<Budget.Domain.Entities.Budget.Budget>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var events = entity.DomainEvents.ToList();
                    entity.ClearDomainEvents();
                    return events;
                })
                .ToList();

            // 2. Persist state changes in database
            var result = await base.SaveChangesAsync(cancellationToken);

            // 3. Dispatch domain events via MediatR
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
