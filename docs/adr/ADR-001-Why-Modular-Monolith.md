# ADR-001: Why Modular Monolith

## Status

Accepted

---

## Date

2026-08-03

---

## Context

FinTracker is a personal finance management application that is expected to grow over time.

The first version (MVP) focuses on core financial capabilities including Identity, Wallet, Transaction, Budget, Category, Report, and Notification.

The project should be easy to understand, maintain, and extend while remaining realistic for a single developer to build within a limited time.

Several architectural styles were considered:

- Layered Architecture
- Clean Architecture
- Microservices
- Modular Monolith

The project goal is not to demonstrate distributed systems, but to demonstrate good software architecture, clean boundaries, maintainability, and software engineering practices.

---

## Decision

FinTracker will be implemented as a **Modular Monolith**.

Each business capability will be implemented as an independent module.

Each module follows **Clean Architecture** internally:

- Domain
- Application
- Infrastructure
- Presentation

The application will be deployed as a single process and use a single relational database.

The architecture is intentionally designed to keep business modules isolated while avoiding the operational complexity of microservices.

---

## Rationale

This architecture was selected because it provides:

- Clear separation of business responsibilities.
- Better maintainability as the project grows.
- Independent business modules.
- Easier testing.
- Better readability for developers and code reviewers.
- A realistic architecture for a production-ready MVP.
- A possible migration path toward microservices in the future if required.

---

## Alternatives Considered

### Layered Architecture

Pros

- Very simple
- Fast to implement

Cons

- Business logic becomes centralized.
- Module boundaries become unclear as the application grows.

Decision: Rejected.

---

### Microservices

Pros

- Independent deployment
- Excellent scalability

Cons

- Operational complexity
- Distributed communication
- More infrastructure
- Too complex for a single-developer MVP

Decision: Rejected for the first version.

---

## Consequences

### Positive

- Well-defined module boundaries
- Easier maintenance
- Easier feature development
- Better project organization
- Easier future migration

### Negative

- Modules still share one deployment unit.
- Architectural boundaries must be respected by developers.

---

## Future Evolution

If FinTracker grows significantly, some modules (such as Notification or Identity) may later be extracted into independent services.

No architectural changes are currently required for the MVP.