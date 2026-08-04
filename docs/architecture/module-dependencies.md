# Module Dependency Rules

## Overview

FinTracker follows a modular monolith architecture.

Each module owns its business logic and internal implementation.
Modules must communicate only through well-defined contracts.

---

## Dependency Rule

Modules should not directly access the internal implementation of other modules.

Communication must happen through well-defined contracts.

---

## Allowed Dependencies

A module can depend on:

- SharedKernel
- Public contracts exposed by other modules

Example:

Transaction Module

can depend on:

Transaction
    |
    ↓
Wallet.Contracts

---

## Forbidden Dependencies

A module must not directly depend on:

- Another module's Domain layer
- Another module's Application implementation
- Another module's Infrastructure layer
- Another module's database context
- Internal business rules of another module

Example:

Invalid dependency:

Transaction
    |
    ↓
Wallet.Infrastructure


Invalid dependency:

Transaction
    |
    ↓
Wallet.Domain

---

## Module Communication

When a module needs functionality from another module:

1. The owning module exposes a contract.
2. Other modules depend only on that contract.
3. Internal implementation remains hidden.

This keeps modules independent and allows future changes without affecting other modules.