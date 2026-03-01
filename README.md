[![CI/CD - Auth API (Development)](https://github.com/juanjo192021/auth-service-net/actions/workflows/auth-api-dev.yml/badge.svg)](https://github.com/juanjo192021/auth-service-net/actions/workflows/auth-api-dev.yml)

# Authentication.RefreshToken

**Authentication.RefreshToken** is an authentication service based on **JWT** and **Refresh Tokens**, developed with a modular architecture that separates responsibilities across Application, Infrastructure, Persistence, and Services layers.  
The project integrates validations, API versioning, authentication, Swagger, and CI/CD with GitHub Actions.

---

## 🧱 Project Architecture

The solution follows **Clean Architecture** principles, along with **CQRS + MediatR**, to maintain clean, scalable, and decoupled code.

---

# 📂 Infraestructure

## 🔐 Authentication.RefreshToken.Infrastructure

This layer is responsible for authentication configuration, JWT handling, and related services.

### 📦 Packages

- `BCrypt.Net-Next` — 4.0.3  
- `Microsoft.AspNetCore.Authentication.JwtBearer` — 9.0.10  
- `Microsoft.Extensions.DependencyInjection` — 9.0.10  
- `Microsoft.Extensions.Options.ConfigurationExtensions` — 9.0.10  
- `System.IdentityModel.Tokens.Jwt` — 8.14.0  

---

## 🗄️ Authentication.RefreshToken.Persistence

Contains data access logic, Entity Framework Core configurations, and the `ApplicationDbContext`.

### 📦 Packages

- `BCrypt.Net-Next` — 4.0.3  
- `Microsoft.AspNetCore.Identity` — 2.3.1  
- `Microsoft.EntityFrameworkCore` — 9.0.10  
- `Microsoft.EntityFrameworkCore.Relational` — 9.0.10  
- `Microsoft.EntityFrameworkCore.SqlServer` — 9.0.10  
- `Microsoft.Extensions.DependencyInjection` — 9.0.10  
- `Microsoft.Extensions.Options.ConfigurationExtensions` — 9.0.10  

### 🛠️ Migration Commands

**Create a migration**

```
dotnet ef migrations add [SchemeName] --project Authentication.RefreshToken.Persistence --startup-project Authentication.RefreshToken.Services.WebApi --output-dir Migrations --context ApplicationDbContext
```

**Update the database**

```
dotnet ef database update --project Authentication.RefreshToken.Persistence --startup-project Authentication.RefreshToken.Services.WebApi --context ApplicationDbContext
```

**Create script of database**

```
dotnet ef migrations script --project Authentication.RefreshToken.Persistence --startup-project Authentication.RefreshToken.Services.WebApi --context ApplicationDbContext -o Database/[SchemeName].sql
```

---

# 📂 Application

## 🧠 Authentication.RefreshToken.Application.UseCases

This layer contains use cases, validations, mappings, and CQRS handlers.

### 📦 Packages

- `FluentValidation` — 12.1.0
- `FluentValidation.DependencyInjectionExtensions` — 12.1.0
- `Mapster` — 7.4.0
- `Mapster.DependencyInjection` — 1.0.1
- `MediatR` — 13.1.0

---

# 📂 Services

## 🌐 Authentication.RefreshToken.Services.WebApi

Exposure of REST endpoints, documentation, versioning, and API configuration.

### 📦 Packages

- `Asp.Versioning.Mvc` — 8.1.0
- `Asp.Versioning.Mvc`.ApiExplorer — 8.1.0
- `Microsoft.AspNetCore.OpenApi` — 9.0.11
- `Microsoft.EntityFrameworkCore.Design` — 9.0.10
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` — 1.23.0
- `Swashbuckle.AspNetCore —` 9.0.6
- `Swashbuckle.AspNetCore.Annotations` — 9.0.6

---
