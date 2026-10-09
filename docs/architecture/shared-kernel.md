# SharedKernel Decision

`SharedKernel` contains only cross-cutting domain concepts that are shared across modules.

It provides common building blocks and abstractions required by different modules while keeping business rules and module-specific behavior isolated.

The SharedKernel must remain independent from any specific business module.

## Included Concepts

- Domain Primitives
  - `BaseEntity`
  - `AuditableEntity`
  - `AggregateRoot`

- Domain Events
  - `IDomainEvent`
  - Domain event abstractions shared across modules

- Value Objects
  - `Money`

- Result Pattern
  - `Result`
  - `Result<T>`

- Common Exceptions
  - `DomainException`

- Common Abstractions
  - `IDateTimeProvider`

## Design Rules

SharedKernel should contain only concepts that:
- Have no dependency on a specific business module
- Are reusable across multiple modules
- Represent common domain or application patterns

Adding module-specific logic to SharedKernel is not allowed because it creates unnecessary coupling between modules.

## Excluded Concepts

The following concepts must remain inside their own modules:

- Wallet business rules
- Transaction business rules
- Module-specific domain events
- Module-specific entities and value objects
- Module-specific repositories
- Generic Repository as a mandatory abstraction

## Current Implementation

The current SharedKernel provides the foundation for FinTracker modules by defining common domain building blocks without introducing any financial business rules.