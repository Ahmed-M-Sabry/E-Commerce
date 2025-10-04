# 🚗 CarSales API

CarSales is a modern Web API project for managing new and used car listings.
Built with **.NET 9**, **Clean Architecture**, **CQRS**, and **MediatR**, it provides a scalable, testable, and maintainable backend foundation.

---

## 🧱 Architecture Overview

This project follows **Clean Architecture** principles and is divided into 4 key layers:

| Layer                       | Responsibility                                                             |
| --------------------------- | -------------------------------------------------------------------------- |
| **CarSales.API**            | Entry point. Hosts Controllers, Middleware, and base configurations.       |
| **CarSales.Application**    | Business logic: Handlers (CQRS), Validators, Service Interfaces, Mappings. |
| **CarSales.Domain**         | Core entities, Enums, Interfaces, and Domain rules.                        |
| **CarSales.Infrastructure** | EF Core DbContext, Repositories, external services (e.g., Identity).       |

---

## ⚙️ Tech Stack

* **.NET 9**
* **CQRS + MediatR**
* **FluentValidation**
* **AutoMapper**
* **ASP.NET Core Identity** (wrapped in `IIdentityServices`)
* **Global Exception Middleware**
* **Custom Result<T> & ApiResponse<T> pattern**
* **JWT Authentication** (with Refresh Tokens via `JwtSettings` & `RefreshToken`)
* **Pipeline Behaviors** (AdminAuthorizationBehavior, UserAuthorizationBehavior, PublicAuthorizationBehavior, ValidationBehavior)
* **UserContextService** (extracts user info from JWT claims)

---

## 🧠 Core Concepts

| Concept                       | Purpose                                                   |
| ----------------------------- | --------------------------------------------------------- |
| **CQRS**                      | Separates write (Command) and read (Query) operations.    |
| **FluentValidation**          | Validates requests at the Application layer.              |
| **Result<T>**                 | Standardizes success/failure without throwing exceptions. |
| **ApiResponse<T>**            | Wraps all API responses in a consistent format.           |
| **ErrorType Enum**            | Maps business errors to corresponding HTTP status codes.  |
| **IIdentityServices**         | Abstracts Identity logic from ASP.NET Core Identity.      |
| **GlobalExceptionMiddleware** | Catches unhandled exceptions and formats them.            |
| **Pipeline Behaviors**        | Handles cross-cutting concerns like auth and validation.  |
| **UserContextService**        | Extracts user information from JWT claims.                |

---

## ✅ Implemented Features

* ✅ Register new users
* ✅ Login with JWT & Refresh Tokens
* ✅ Validate email & password
* ✅ Structured error messages
* ✅ Role-based authorization
* ✅ Soft delete for entities
* ✅ Pipeline behaviors for auth/validation

---

## 🔐 Authentication Flow

**`POST /api/auth/register`**

1. Validates user input with `ValidationBehavior`.
2. Creates a user via `IIdentityServices`.
3. Returns `Result<RegisterUserDto>` wrapped in `ApiResponse`.

**`POST /api/auth/login`**

1. Checks if the email exists.
2. Validates password.
3. Generates JWT + Refresh Token.
4. Returns `Result<ResponseAuthModel>`.

**`POST /api/auth/refresh-token`**

1. Validates refresh token.
2. Issues new JWT & Refresh Token.

**Authorization:**

* **AdminAuthorizationBehavior** → Admin-only access.
* **UserAuthorizationBehavior** → Authenticated user endpoints.
* **PublicAuthorizationBehavior** → Public endpoints.
* **UserContextService** → Extracts `UserId` from JWT claims.

---

## 🧾 Result & Error Handling

**Result<T> Pattern**

```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public ErrorType? ErrorType { get; }

    public static Result<T> Success(T data) => new(true, data, null, null);
    public static Result<T> Failure(string error, ErrorType type) => new(false, default, error, type);
}
```

**ApiResponse<T> Pattern**

```csharp
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Message { get; } 
        public ErrorType ErrorType { get; }

        private Result(T value, string message = null)
        {
            IsSuccess = true;
            Value = value;
            Message = message;
            ErrorType = ErrorType.None;
        }

        private Result(string message, ErrorType errorType)
        {
            IsSuccess = false;
            Value = default;
            Message = message;
            ErrorType = errorType;
        }

        public static Result<T> Success(T value, string message = null) => new Result<T>(value, message);
        public static Result<T> Failure(string message, ErrorType errorType) => new Result<T>(message, errorType);
    }
```

**ErrorType Enum**

```csharp
public enum ErrorType
{
    BadRequest,    // 400
    Unauthorized,  // 401
    Forbidden,     // 403
    NotFound,      // 404
    Internal       // 500
}
```

---

## 📦 Entities with Soft Delete

| Entity               | Features                         |
| -------------------- | -------------------------------- |
| **Brand**            | Soft delete, `GetAllActiveAsync` |
| **FuelType**         | Soft delete, `GetAllActiveAsync` |
| **TransmissionType** | Soft delete, `GetAllActiveAsync` |
| **Category**         | Soft delete, `GetAllActiveAsync` |

---

## 🧩 Design Principles Followed

* ✅ **SOLID Principles**
* ✅ **Open/Closed Principle**
* ✅ **Separation of Concerns**
* ✅ **Unit-testable Handlers and Services**

---

## 📂 Folder Structure

```
CarSales.Solution/
├── CarSales.API/                # Entry point
│   ├── Controllers/
│   ├── Middlewares/
│   └── Program.cs
│
├── CarSales.Application/        # Business logic
│   ├── Features/
│   ├── IServices/
│   ├── Validators/
│   ├── Behaviors/
│   ├── Common/
│
├── CarSales.Domain/             # Core entities
│   ├── Entities/
│   ├── Enums/
│   ├── IRepositories/
│
├── CarSales.Infrastructure/     # Data & external services
│   ├── Repositories/
│   ├── Identity/
│   ├── Services/
│   ├── DbContext/
```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

---

## 📄 License

MIT License © 2025 - \[Ahmed Sabry]

---
