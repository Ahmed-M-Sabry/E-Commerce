
# 🛒 ECommerce Platform - Full-Stack E-Commerce Solution

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-18-DD0031?style=flat&logo=angular)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![Redis](https://img.shields.io/badge/Redis-7.x-DC382D?style=flat&logo=redis)](https://redis.io/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A modern, scalable e-commerce platform built with **Clean Architecture** principles, featuring advanced authentication, real-time shopping cart management, and comprehensive product catalog functionality.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [Future Improvements](#-future-improvements)
- [Screenshots](#-screenshots)
- [Contributing](#-contributing)
- [Contact](#-contact)

---

## 🎯 Overview

This ECommerce platform is a production-ready solution designed to handle the complete lifecycle of online shopping operations. Built with scalability and maintainability in mind, it implements industry-standard patterns and modern development practices.

**Key Highlights:**
- Clean Architecture with CQRS pattern using MediatR
- JWT-based authentication with refresh token mechanism
- Redis-powered shopping cart for optimal performance
- Real-time inventory management
- Email verification and password reset flows
- Role-based authorization (Admin, Seller, Buyer)

---

## 🛠 Tech Stack

### Backend
- **Framework:** ASP.NET Core 9.0 Web API
- **ORM:** Entity Framework Core 9.0
- **Database:** SQL Server 2022
- **Cache:** Redis (StackExchange.Redis)
- **Authentication:** ASP.NET Core Identity + JWT
- **Validation:** FluentValidation
- **Mapping:** AutoMapper
- **Email:** MailKit + MimeKit

### Frontend *(Planned)*
- **Framework:** Angular 18
- **State Management:** NgRx *(planned)*
- **UI Library:** Angular Material *(planned)*

### Architecture Patterns
- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection
- Pipeline Behaviors (Validation, Authorization)

---

## 🏗 Architecture

The solution follows **Clean Architecture** principles with clear separation of concerns:

```
ECommerceProject/
│
├── ECommerce.Domain/          # Enterprise business rules
│   ├── Entities/              # Domain entities
│   ├── IRepositories/         # Repository interfaces
│   └── AuthenticationHelper/  # Auth DTOs and models
│
├── ECommerce.Application/     # Application business rules
│   ├── Features/              # CQRS commands/queries
│   ├── IServices/             # Service interfaces
│   ├── Mappings/              # AutoMapper profiles
│   ├── PipelineBehaviors/     # MediatR behaviors
│   └── Common/                # Shared DTOs and results
│
├── ECommerce.Infrastructure/  # External concerns
│   ├── Data/                  # DbContext and migrations
│   ├── Repositories/          # Repository implementations
│   ├── Services/              # Service implementations
│   └── Migrations/            # EF Core migrations
│
└── ECommerce.API/             # Presentation layer
    ├── Controllers/           # API endpoints
    ├── Middleware/            # Custom middleware
    └── ApplicationBase/       # Base controllers and extensions
```

### Key Design Patterns

**CQRS with MediatR**
- Commands and Queries separated for better scalability
- Pipeline behaviors for cross-cutting concerns (validation, authorization)

**Result Pattern**
- Type-safe error handling
- Consistent API responses with `Result<T>` wrapper

**Repository Pattern**
- Abstraction over data access
- Testability and flexibility

---

## ✨ Features

### 🔐 Authentication & Authorization
- ✅ JWT-based authentication with refresh tokens
- ✅ Email verification on registration
- ✅ Password reset via email
- ✅ Role-based access control (Admin, Seller, Buyer)
- ✅ Secure cookie-based refresh token storage
- ✅ Token revocation on logout

### 🛍 Product Management
- ✅ CRUD operations for products
- ✅ Multiple image upload per product
- ✅ Category management with soft delete
- ✅ Advanced product search and filtering
- ✅ Pagination with keyset pagination support
- ✅ Stock quantity tracking
- ✅ Product ratings and reviews
- ✅ Seller-specific product ownership

### 🛒 Shopping Cart
- ✅ Redis-backed cart for high performance
- ✅ Real-time inventory validation
- ✅ Automatic price updates from product catalog
- ✅ Quantity increment/decrement
- ✅ Cart persistence across sessions (3 days)
- ✅ Automatic stock checking and item removal

### 🔒 Security Features
- ✅ Rate limiting (80 requests per 30 seconds per IP)
- ✅ Security headers (CSP, X-Frame-Options, etc.)
- ✅ HTTPS enforcement
- ✅ CORS policy configuration
- ✅ File upload validation (type, size, MIME type)
- ✅ SQL injection protection via EF Core

### 📧 Email System
- ✅ Styled HTML email templates
- ✅ Email confirmation on registration
- ✅ Password reset emails
- ✅ Asynchronous email delivery

---

## 📂 Project Structure

<details>
<summary>Click to expand detailed structure</summary>

```
D:\Project\ECommerceProject\
│
├── ECommerce.API/
│   ├── Controllers/
│   │   ├── AuthenticationController.cs
│   │   ├── BasketController.cs
│   │   ├── CategoryController.cs
│   │   ├── ProductController.cs
│   │   ├── ProductPhotoController.cs
│   │   └── ErrorController.cs
│   ├── Middleware/
│   │   └── ExceptionsMiddleware.cs
│   ├── ApplicationBase/
│   │   ├── ApplicationControllerBase.cs
│   │   └── ResultExtensions.cs
│   ├── wwwroot/
│   │   └── Uploads/
│   ├── Program.cs
│   └── appsettings.json
│
├── ECommerce.Application/
│   ├── Features/
│   │   ├── AuthenticationFeatures/
│   │   ├── BasketFeatures/
│   │   ├── CategoryFeatures/
│   │   ├── ProductFeatures/
│   │   └── ProductPhotoFeatures/
│   ├── IServices/
│   ├── Mappings/
│   ├── PipelineBehaviors/
│   └── Common/
│
├── ECommerce.Domain/
│   ├── Entities/
│   ├── IRepositories/
│   └── AuthenticationHelper/
│
└── ECommerce.Infrastructure/
    ├── Data/
    ├── Repositories/
    ├── Services/
    └── Migrations/
```

</details>

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server 2022](https://www.microsoft.com/sql-server/sql-server-downloads) or SQL Server Express
- [Redis](https://redis.io/download) (or Docker: `docker run -d -p 6379:6379 redis`)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/Ahmed-M-Sabry/ECommerceProject.git
cd ECommerceProject
```

2. **Configure the database**

Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ECommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
  "redis": "localhost:6379"
}
```

3. **Configure JWT settings**

Update JWT configuration in `appsettings.json`:
```json
"jwtSittings": {
  "Key": "YourSuperSecretKeyHereMustBe32CharsMin",
  "Issuer": "ECommerceAPI",
  "Audience": "ECommerceClient",
  "DurationInHours": 1
}
```

4. **Configure email settings**

Add email configuration in `appsettings.json`:
```json
"EmailConfiguration": {
  "Email": "your-email@example.com",
  "Password": "your-app-password",
  "DisplayName": "ECommerce Team",
  "Host": "smtp.gmail.com",
  "Port": 587
}
```

5. **Apply database migrations**
```bash
cd ECommerce.API
dotnet ef database update
```

6. **Run the application**
```bash
dotnet run
```

The API will be available at `https://localhost:7XXX` and Swagger UI at `https://localhost:7XXX/swagger`

### Default Admin Account

After running migrations, a default admin account is created:
- **Email:** `admin@eshop.com`
- **Password:** `Aa123**`

---

## 📡 API Endpoints

### Authentication
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/Authentication/RegisterBuyer` | Register as buyer | No |
| POST | `/api/Authentication/RegisterSeller` | Register as seller | No |
| POST | `/api/Authentication/Login` | User login | No |
| GET | `/api/Authentication/Confirm-Email` | Confirm email | No |
| POST | `/api/Authentication/Resend-Confirm-Email` | Resend confirmation | No |
| POST | `/api/Authentication/Generate-New-token-From-RefreshToken` | Refresh JWT token | Yes |
| POST | `/api/Authentication/Forget-Password` | Request password reset | No |
| POST | `/api/Authentication/Rest-Password` | Reset password | No |
| POST | `/api/Authentication/Logout` | Logout user | Yes |

### Products
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/Product/Get-All-Products` | Get paginated products | No |
| GET | `/api/Product/Get-Product-By-Id` | Get product details | No |
| POST | `/api/Product/Create-Product` | Create new product | Seller |
| PUT | `/api/Product/Edit-Product` | Update product | Seller |
| DELETE | `/api/Product/Delete-Product` | Delete product | Seller |

### Categories
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/Category/Get-All-Categories-User` | Get active categories | No |
| GET | `/api/Category/Get-All-Categories-Admin` | Get all categories | Admin |
| GET | `/api/Category/Get-Category-ById` | Get category by ID | Admin |
| POST | `/api/Category/Create-Category` | Create category | Admin |
| PUT | `/api/Category/Edit-Category` | Update category | Admin |
| DELETE | `/api/Category/Soft-Delete-Category` | Soft delete category | Admin |
| PATCH | `/api/Category/Restore-Category` | Restore category | Admin |

### Shopping Cart
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/Basket/Get-Basket` | Get user cart | No |
| POST | `/api/Basket/Create-Basket` | Create/update cart | No |
| POST | `/api/Basket/Add-Item` | Add item to cart | No |
| DELETE | `/api/Basket/Remove-Item` | Remove item | No |
| PUT | `/api/Basket/Increment-Item-Quantity` | Increase quantity | No |
| PUT | `/api/Basket/Decrement-Item-Quantity` | Decrease quantity | No |
| DELETE | `/api/Basket/Delete-Basket` | Clear cart | No |

---

## 🔮 Future Improvements

### Short-term (Next Sprint)
- [ ] Implement payment gateway integration (Stripe/PayPal)
- [ ] Add order management system
- [ ] Implement product reviews and ratings UI
- [ ] Add advanced search with Elasticsearch
- [ ] Implement real-time notifications with SignalR

### Medium-term
- [ ] Build Angular frontend with NgRx
- [ ] Add unit and integration tests (xUnit)
- [ ] Implement CI/CD pipeline (GitHub Actions)
- [ ] Add Docker support and Docker Compose
- [ ] Implement API versioning
- [ ] Add GraphQL endpoint

### Long-term
- [ ] Microservices architecture migration
- [ ] Add recommendation engine
- [ ] Implement analytics dashboard
- [ ] Add multi-language support
- [ ] Mobile app (Flutter/React Native)
- [ ] Admin dashboard with real-time metrics

---

## 📸 Screenshots

*Coming soon! Frontend development in progress.*

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 📞 Contact

**Ahmed Mohamed Sabry**

- GitHub: [@Ahmed-M-Sabry](https://github.com/Ahmed-M-Sabry)
- LinkedIn: [a7medsabrii](https://www.linkedin.com/in/a7medsabrii)
- Email: a7med.mohamed.sabri@gmail.com

---

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- ASP.NET Core Documentation
- Redis Documentation
- Entity Framework Core Team

---

<div align="center">

**⭐ Star this repository if you find it helpful!**

Made with ❤️ by [Ahmed M. Sabry](https://github.com/Ahmed-M-Sabry)

</div>
