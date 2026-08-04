# ADR-002: Module Boundaries

## Status

Accepted

---

## Date

2026-08-03

---

## Context

FinTracker consists of multiple business capabilities.

Without clear ownership, business logic may become duplicated, tightly coupled, and difficult to maintain.

To avoid this problem, each module must own a specific business responsibility.

---

## Decision

Each business capability will be implemented as an independent module with a single responsibility.

The initial modules are:

- Identity
- Wallet
- Transaction
- Category
- Budget
- Report
- Notification

Each module owns its own business logic and is responsible for maintaining its own consistency.

---

## Module Responsibilities

### Identity

Responsible for:

- Authentication
- Authorization
- User Profile

Not responsible for:

- Wallets
- Transactions
- Reports
- Budgets

---

### Wallet

Responsible for:

- Wallet management
- Wallet balances

Not responsible for:

- Reports
- Authentication

---

### Transaction

Responsible for:

- Income
- Expenses
- Transfers
- Transaction history

Not responsible for:

- Reporting
- Notifications

---

### Category

Responsible for:

- Income categories
- Expense categories

Not responsible for:

- Transactions

---

### Budget

Responsible for:

- Budget planning
- Budget monitoring

Not responsible for:

- Recording transactions

---

### Report

Responsible for:

- Financial analysis
- Reports
- Statistics

Report is read-only.

It does not modify business data.

---

### Notification

Responsible for:

- User notifications
- Budget alerts
- System notifications

Notification reacts to business events.

It does not own financial data.

---

## Architectural Rules

The following rules must always be respected:

1. Every module owns its own business logic.

2. Modules should not directly modify another module's internal data.

3. Communication between modules should happen through explicit contracts.

4. Business rules must remain inside the owning module.

5. Modules should remain as independent as possible.

6. Report consumes data but does not own business data.

7. Notification reacts to events but does not change business state.

---

## Expected Benefits

- Better maintainability
- Clear responsibilities
- Easier testing
- Easier onboarding
- Better scalability
- Cleaner architecture

---

## Future Considerations

Additional modules may be introduced in future versions if new business capabilities require independent ownership.

The current MVP intentionally keeps the number of modules small and focused.