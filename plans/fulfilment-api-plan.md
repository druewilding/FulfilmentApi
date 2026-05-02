# FulfilmentApi

A .NET learning project: a small order fulfilment microservice built incrementally to explore modern backend patterns.

## What this is

A practice API that simulates an order fulfilment pipeline — placing orders, reserving inventory, and scheduling delivery. Built week by week, layering in new concepts.

## Tech stack

- .NET 10 / ASP.NET Core (minimal API or controllers)
- PostgreSQL
- RabbitMQ (via MassTransit)
- Docker Compose
- xUnit + Testcontainers

## Plan

### Part 1: API Basics

- [ ] Scaffold project: `dotnet new webapi -n FulfilmentApi`
- [ ] POST `/orders` — accept an order (product, quantity, delivery address)
- [ ] GET `/orders/{id}` — return order status
- [ ] Service layer with dependency injection (interface + implementation)
- [ ] Request logging middleware
- [ ] Background worker (BackgroundService) that "processes" orders from an in-memory queue
- [ ] Basic xUnit tests for the service layer

### Part 2: Domain Modelling (DDD)

- [ ] Create `Domain/` folder — no infrastructure references allowed here
- [ ] `Order` aggregate root with invariants:
  - Can't add items after confirmation
  - Can't exceed max delivery weight
- [ ] Value objects: `Address`, `Money`, `Weight`
- [ ] Domain events: `OrderPlaced`, `OrderConfirmed`
- [ ] `DeliverySlot` entity
- [ ] Unit tests for domain logic (invariant enforcement)

### Part 3: Event-Driven Messaging

- [ ] Add RabbitMQ via Docker Compose
- [ ] Install MassTransit
- [ ] Publish `OrderPlaced` event when an order is created
- [ ] Consumer: `InventoryReservationConsumer` listens and "reserves stock"
- [ ] Consumer: `DeliverySchedulingConsumer` assigns a delivery slot
- [ ] Idempotency — handle duplicate messages safely
- [ ] Dead-letter queue for failed messages

### Part 4: Containerisation & Integration Tests

- [ ] Multi-stage Dockerfile (SDK build → runtime image)
- [ ] `docker-compose.yml`: API + PostgreSQL + RabbitMQ
- [ ] Health check endpoint (`/health`)
- [ ] Environment-based configuration (appsettings vs. env vars)
- [ ] Integration tests with WebApplicationFactory
- [ ] Testcontainers for PostgreSQL and RabbitMQ in tests
- [ ] Swap in-memory store for EF Core + PostgreSQL

## Structure (target)

```
FulfilmentApi/
├── src/
│   ├── FulfilmentApi/          # Web API (controllers, middleware, config)
│   ├── FulfilmentApi.Domain/   # Domain models, events, interfaces
│   └── FulfilmentApi.Infrastructure/  # EF Core, message bus, external concerns
├── tests/
│   ├── FulfilmentApi.UnitTests/
│   └── FulfilmentApi.IntegrationTests/
├── docker-compose.yml
├── Dockerfile
└── README.md
```

## Useful commands

```bash
# Create solution
dotnet new sln -n FulfilmentApi
dotnet new webapi -n FulfilmentApi -o src/FulfilmentApi
dotnet new classlib -n FulfilmentApi.Domain -o src/FulfilmentApi.Domain
dotnet new classlib -n FulfilmentApi.Infrastructure -o src/FulfilmentApi.Infrastructure
dotnet new xunit -n FulfilmentApi.UnitTests -o tests/FulfilmentApi.UnitTests
dotnet new xunit -n FulfilmentApi.IntegrationTests -o tests/FulfilmentApi.IntegrationTests

# Add projects to solution
dotnet sln add src/FulfilmentApi
dotnet sln add src/FulfilmentApi.Domain
dotnet sln add src/FulfilmentApi.Infrastructure
dotnet sln add tests/FulfilmentApi.UnitTests
dotnet sln add tests/FulfilmentApi.IntegrationTests

# References
dotnet add src/FulfilmentApi reference src/FulfilmentApi.Domain
dotnet add src/FulfilmentApi reference src/FulfilmentApi.Infrastructure
dotnet add src/FulfilmentApi.Infrastructure reference src/FulfilmentApi.Domain

# Run
dotnet run --project src/FulfilmentApi

# Test
dotnet test

# Docker
docker compose up -d
```

## Key packages to explore

- `MassTransit` + `MassTransit.RabbitMQ` — messaging abstraction
- `Npgsql.EntityFrameworkCore.PostgreSQL` — EF Core for Postgres
- `Testcontainers` — real containers in tests
- `Microsoft.AspNetCore.Mvc.Testing` — WebApplicationFactory
- `NSubstitute` or `Moq` — mocking
