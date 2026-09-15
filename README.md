# 💰 FinTracker

A modern Personal Finance Management system built with .NET.

---

# About Project

FinTracker is a personal finance management application that helps users organize their financial activities in a simple, structured, and efficient way.

The first version of the project focuses on essential personal finance features such as wallet management, transactions, budgeting, expense tracking, reports, and notifications.

The architecture is designed to be extensible, allowing future versions to support more advanced financial features without requiring major architectural changes.

---

# Problem Statement

Managing personal finances is a challenge for many people. Without a proper system, it becomes difficult to understand spending habits, monitor budgets, and make informed financial decisions.

FinTracker provides a centralized platform that enables users to record their financial activities, monitor their budgets, and gain better visibility into their financial situation.

---

# Target Users

The first version of FinTracker is designed for individual users who want to manage their personal finances.

Future versions may support small businesses and larger organizations through dedicated financial management features.

---

# MVP Modules

- Identity
- Wallet
- Transaction
- Category
- Budget
- Report
- Notification

---

# Planned Features

## Identity

- Register
- Login
- Logout
- Refresh Token
- Change Password
- Update Profile

---

## Wallet

- Create Wallet
- Update Wallet
- Delete Wallet
- View Wallet Balance

---

## Transaction

- Record Income
- Record Expense
- Transfer Between Wallets
- Transaction History

---

## Category

- Income Categories
- Expense Categories

---

## Budget

- Monthly Budgets
- Budget Monitoring

---

## Report

- Monthly Reports
- Expense Reports
- Income Reports
- Category Reports

---

## Notification

- Budget Alerts
- Transaction Notifications

---

# Architecture

FinTracker is designed using a Modular Monolith architecture with Clean Architecture principles.

## Why Modular Monolith?

As the system grows, maintaining clear boundaries between different business capabilities becomes important.

Instead of organizing the application only by technical layers, FinTracker is divided into independent business modules. Each module owns its domain logic, application logic, infrastructure concerns, and presentation layer.

The main goals of this architecture are:

- Maintaining clear boundaries between business domains
- Reducing coupling between different parts of the system
- Improving maintainability and readability
- Making feature development easier
- Providing a structure that can evolve as the system grows

Each module follows the same internal structure:

Module

├── Domain

├── Application

├── Infrastructure

└── Presentation

Current modules:

├── Identity

├── Wallet

├── Transaction

├── Category

├── Budget

├── Report

└── Notification


Each module is responsible for its own business rules and communicates with other modules through clear boundaries.

---

### Design Standards
To ensure the system remains maintainable and scalable as it grows:

*   **Module Communication:** Modules are loosely coupled. They communicate via shared contracts (Shared Kernel) and integration events, avoiding direct dependency between domains.
*   **Database Isolation:** Each module owns its database schema, strictly enforced through Entity Framework Core configurations to prevent cross-module table access.
*   **Quality Assurance:** The project follows a test-driven mindset, focusing on unit testing the Domain layer and implementing integration tests for the Application layer logic.


# .NET Version Decision

## Why .NET 8?

FinTrack is developed using .NET 8.

The main reason for choosing .NET 8 is its Long Term Support (LTS) lifecycle, which provides a stable and reliable foundation for a long-running financial management system.

The decision was based on:

- Long-term support lifecycle
- Production stability
- Mature ecosystem and library compatibility
- Improved runtime performance
- Modern ASP.NET Core features
- Better maintainability

Choosing a framework version is not only about using the newest technology; it is about selecting a reliable platform that matches the project's long-term goals.

For a financial management system, stability, maintainability, and future evolution are important factors in technology selection.

---

# Solution Structure

src


├── FinTrack.Api


├── Modules


│ ├── Identity

│ │ ├── Domain

│ │ ├── Application

│ │ ├── Infrastructure

│ │ └── Presentation

│ │

│ ├── Wallet

│ ├── Transaction

│ ├── Category

│ ├── Budget

│ ├── Report

│ └── Notification


└── Shared

# Technologies

- .NET 8
- ASP.NET Core
- Entity Framework Core 8
- SQL Server
- Clean Architecture
- Modular Monolith Architecture

.NET 8 LTS
+
Clean Architecture
+
Modular Monolith
+
EF Core 8
+
SQL Server
