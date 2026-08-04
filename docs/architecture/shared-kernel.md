# SharedKernel Decision

SharedKernel contains only cross-cutting concepts that are shared between modules.

It must not contain business logic that belongs to a specific module.

## Included Concepts

- Domain Primitives
  - BaseEntity
  - AggregateRoot

- Domain Events
  - Domain Event abstractions

- Value Objects
  - Money

- Result Pattern

- Common Exceptions

- Common Abstractions
  - Time Provider

## Excluded Concepts

The following concepts must remain inside their own modules:

- Wallet business rules
- Transaction business rules
- Module-specific domain events
- Module-specific repositories
- Generic Repository as a mandatory abstraction