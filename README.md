# FulfilmentApi

A .NET learning project — a small order fulfilment microservice built incrementally to explore modern backend patterns.

## What it does

Simulates an order fulfilment pipeline: placing orders, reserving inventory, and scheduling delivery. The goal is understanding, not production readiness.

## Tech stack

- **.NET 10** / ASP.NET Core (Minimal APIs)
- **PostgreSQL** — persistent storage via Entity Framework Core
- **RabbitMQ** — async messaging via MassTransit
- **Docker Compose** — local infrastructure
- **xUnit** + Testcontainers — unit and integration tests

## Project structure

```
src/
  FulfilmentApi/                 # Web API — routes, middleware, configuration
  FulfilmentApi.Domain/          # Domain models, value objects, events (no infrastructure)
  FulfilmentApi.Infrastructure/  # EF Core, message bus, external concerns
tests/
  FulfilmentApi.UnitTests/
  FulfilmentApi.IntegrationTests/
```

## Running locally

```bash
dotnet run --project src/FulfilmentApi
```

## Running tests

```bash
dotnet test
```
