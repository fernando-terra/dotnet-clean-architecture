# .NET Clean Architecture Template

[![CI](https://github.com/fernando-terra/dotnet-clean-architecture/actions/workflows/ci.yml/badge.svg)](https://github.com/fernando-terra/dotnet-clean-architecture/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)

A production-ready .NET 8 template implementing **Clean Architecture**, **Domain-Driven Design (DDD)**, **CQRS with MediatR**, and **SOLID principles**.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                        API Layer                        │
│              Controllers · Middleware · DI               │
└────────────────────────┬────────────────────────────────┘
                         │ depends on
┌────────────────────────▼────────────────────────────────┐
│                   Application Layer                     │
│          CQRS · Use Cases · DTOs · Validators           │
└──────────┬─────────────────────────────┬────────────────┘
           │ depends on                  │ depends on
┌──────────▼──────────┐    ┌─────────────▼──────────────┐
│    Domain Layer     │    │   Infrastructure Layer     │
│ Entities · VOs ·   │    │  EF Core · Repositories ·  │
│ Repos (interfaces) │    │  External Services          │
└─────────────────────┘    └────────────────────────────┘
```

> The **Domain** layer has **zero external dependencies** — it's the heart of the application.

## Project Structure

```
src/
├── Domain/              # Enterprise business rules
│   ├── Common/          # Base classes (Entity, ValueObject, IDomainEvent)
│   ├── Entities/        # Domain entities (Product)
│   ├── ValueObjects/    # Value objects (Money)
│   ├── Repositories/    # Repository interfaces
│   └── Events/          # Domain events
├── Application/         # Application business rules
│   ├── Common/          # Behaviors, Exceptions, Interfaces
│   └── Products/        # Feature slice: Commands + Queries
├── Infrastructure/      # External concerns
│   └── Persistence/     # EF Core, Configurations, Repositories
└── API/                 # Entry point
    ├── Controllers/
    └── Middleware/

tests/
├── Domain.Tests/        # Unit tests for domain logic
└── Application.Tests/   # Unit tests for use cases (Moq + FluentAssertions)
```

## Key Concepts

### Clean Architecture
Each layer has a single responsibility and dependencies only point **inward**:
- **Domain** — pure business logic, no framework dependencies
- **Application** — orchestrates use cases, defines interfaces
- **Infrastructure** — implements interfaces (DB, external APIs)
- **API** — HTTP entry point, wires everything together

### DDD Patterns
- **Entities** — objects with identity (`Product`)
- **Value Objects** — immutable, compared by value (`Money`)
- **Domain Events** — side-effects decoupled via MediatR (`ProductCreatedEvent`)
- **Repository pattern** — domain defines the interface, infrastructure implements it

### CQRS with MediatR
- **Commands** — mutate state (`CreateProductCommand`, `DeleteProductCommand`)
- **Queries** — read-only (`GetProductByIdQuery`, `GetAllProductsQuery`)
- **Pipeline Behaviors** — cross-cutting concerns (validation, logging)

### SOLID in Practice
| Principle | Where |
|---|---|
| **S**ingle Responsibility | Each handler does one thing |
| **O**pen/Closed | New features = new handlers, not modified code |
| **L**iskov Substitution | Repository interface / concrete implementation |
| **I**nterface Segregation | `IProductRepository`, `IApplicationDbContext` |
| **D**ependency Inversion | Domain defines interfaces, Infrastructure implements |

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker & Docker Compose](https://www.docker.com/)

### Run with Docker
```bash
docker-compose up --build
```
API available at: `http://localhost:8080/swagger`

### Run locally
```bash
# 1. Start SQL Server
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong@Passw0rd' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

# 2. Apply migrations
cd src/API
dotnet ef database update --project ../Infrastructure

# 3. Run
dotnet run --project src/API
```

### Run Tests
```bash
dotnet test
```

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/products` | List all active products |
| `GET` | `/api/products/{id}` | Get product by ID |
| `POST` | `/api/products` | Create a new product |
| `DELETE` | `/api/products/{id}` | Delete a product |

### Example Request
```bash
curl -X POST http://localhost:8080/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Clean Architecture Book",
    "description": "Robert C. Martin masterpiece",
    "price": 49.99,
    "stockQuantity": 100
  }'
```

## Tech Stack

| Technology | Purpose |
|---|---|
| **.NET 8** | Runtime |
| **MediatR 12** | CQRS / Mediator pattern |
| **Entity Framework Core 8** | ORM |
| **SQL Server** | Database |
| **FluentValidation 11** | Input validation |
| **Swashbuckle / Swagger** | API documentation |
| **xUnit + Moq + FluentAssertions** | Testing |
| **Docker + Compose** | Containerization |
| **GitHub Actions** | CI/CD |

## License

MIT © [Fernando Terra](https://github.com/fernando-terra)
