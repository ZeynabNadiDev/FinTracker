[![FinTracker CI](https://github.com/ZeynabNadiDev/FinTracker/actions/workflows/ci.yml/badge.svg)](https://github.com/ZeynabNadiDev/FinTracker/actions/workflows/ci.yml)
# 💰 FinTracker

[![FinTracker CI](YOUR_GITHUB_ACTIONS_BADGE_URL)](YOUR_GITHUB_ACTIONS_WORKFLOW_URL)

FinTracker is a personal finance management application built with .NET. It helps individuals organize their wallets, record income and expenses, manage budgets, and review their financial activity through reports and notifications.

The project follows **Modular Monolith** architecture and **Clean Architecture** principles to maintain clear module boundaries and support long-term maintainability.

## 🎯 Problem Statement

Managing personal finances without a centralized system can make it difficult to track spending patterns, monitor budgets, and make informed financial decisions.

FinTracker aims to bring these activities together in one application, providing users with a structured way to record and review their financial activity.

**Target users:** Individuals who want to manage their personal finances. Support for small businesses and larger organizations is outside the current scope.

## ✨ Features

FinTracker is organized around seven business modules:

* **Identity:** User registration and authentication.
* **Wallet:** Create and manage wallets, view balances, and support wallet-to-wallet transfers.
* **Transaction:** Record income and expenses, transfer funds between wallets, and review transaction history.
* **Category:** Organize financial activity by income and expense categories.
* **Budget:** Create monthly budgets and monitor spending against budget limits.
* **Report:** Review financial summaries, including income, expenses, and category-based reports.
* **Notification:** Receive transaction-related notifications and budget alerts.

> **Note:** The list above describes the project's scope. Not all features may be fully implemented yet.

## 🏗️ Architecture

FinTracker is designed as a **Modular Monolith** using **Clean Architecture** principles.

The application runs as a single deployable system while organizing business capabilities into separate modules. This approach avoids the additional operational complexity of distributed microservices while maintaining clear boundaries between business domains.

Each module follows a consistent internal structure:

* **Domain:** Business entities, value objects, and domain rules.
* **Application:** Use cases, commands, queries, and application logic.
* **Infrastructure:** Persistence and external service integrations.
* **Presentation:** Module-specific API endpoints and presentation concerns.

### Architectural Goals

* Maintain clear boundaries between business modules.
* Keep domain logic independent of infrastructure concerns.
* Reduce unnecessary coupling between modules.
* Support maintainable and testable application code.
* Allow business capabilities to evolve without requiring a distributed architecture.

Module communication should use explicit contracts, shared abstractions, or domain events rather than depending directly on another module's internal domain implementation.

## 🛠️ Technology Stack

* **C# / .NET**
* **ASP.NET Core**
* **Entity Framework Core**
* **SQL Server**
* **Clean Architecture**
* **Modular Monolith**
* **Docker**
* **GitHub Actions**
* **GitHub Container Registry (GHCR)**

## 📂 Solution Structure

The main API and business modules are organized under `Src/`:

```text
Src/
├── FinTracker.Api/
│   └── Dockerfile
└── Modules/
    ├── Identity/
    ├── Wallet/
    ├── Transaction/
    ├── Category/
    ├── Budget/
    ├── Report/
    └── Notification/
```

Each module contains its own architectural layers, such as:

```text
ModuleName/
├── Domain/
├── Application/
├── Infrastructure/
└── Presentation/
```

This structure groups related business logic and technical concerns together, making the codebase easier to navigate and maintain.

## 🚀 Running the Project

FinTracker's API can run in a Docker Compose environment and listens on port `8080` inside its container.

### Prerequisites

* Docker Desktop or Docker Engine
* Docker Compose

### Start the Application

From the repository root, run:

```bash
docker compose up --build
```

Check the repository's `docker-compose.yml` and application configuration for the required services, environment variables, database setup, and host port mappings.

### View API Logs

```bash
docker compose logs fintracker-api
```

If Swagger is enabled in the active environment, open it using the host address and port mapped to container port `8080`.

## ⚙️ CI/CD and Container Image

FinTracker uses **GitHub Actions** for continuous integration and Docker for containerization.

The project also reports successful publishing of the API image to **GitHub Container Registry (GHCR)**.

The API Dockerfile is located at:

Src/FinTracker.Api/Dockerfile
```

For the latest workflow status, execution triggers, published image name, and available image tags, refer to the repository's GitHub Actions workflow and Packages page.

## 📌 Project Status

FinTracker is an evolving project focused on applying software architecture principles to a practical personal finance management application.

Its development emphasizes modular design, separation of concerns, maintainability, and clear business boundaries.
