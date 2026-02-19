# Bulky-MVC2

Bulky-MVC2 is a full-stack e-commerce web application built with ASP.NET Core MVC (.NET 8).  
This project was developed following the course:

**Build Real World E-Commerce Application using ASP.NET Core MVC, Entity Framework Core and ASP.NET Core Identity** by Bhrugen Patel.

The application demonstrates real-world architecture patterns, authentication and authorization, payment integration, and deployment strategies.

## Features

- Product and Category CRUD operations
- ASP.NET Core MVC Architecture (.NET 8)
- Entity Framework Core with Code-First Migrations
- Repository Pattern implementation
- ASP.NET Core Identity integration
- Role Management (Admin / Customer)
- Authentication & Authorization
- Stripe Payment Integration
- Email Notifications
- Session Management
- TempData usage
- Bootstrap v5 UI
- Automatic Database Seeding
- Deployment-ready (Azure)

## Architecture

The solution is structured using a clean layered architecture:
```
Bulky-MVC2/
│
├── BulkyWeb # Main Web Application (MVC)
├── Bulky.DataAccess # Data access layer (Repository + EF Core)
├── Bulky.Models # Domain models
├── Bulky.Utility # Utility & helper classes
├── RazorProyect_Temp # Razor project examples
└── Bulky.slnx # Solution file
```

```
Bulky-MVC2
│
├── Bulky.DataAccess
│ ├── Data
│ │ └── ApplicationDbContext.cs
│ ├── DbInitializer
│ ├── Migrations
│ └── Repository
│ ├── IRepositoy
│ ├── CategoryRepository.cs
│ ├── CompanyRepository.cs
│ ├── ProductRepository.cs
│ ├── ShoppingCartRepository.cs
│ ├── OrderHeaderRepository.cs
│ ├── OrderDetailRepository.cs
│ ├── ApplicationUserRepository.cs
│ └── UnitOfWork.cs
│
├── Bulky.Models
│ ├── ViewModels
│ └── ApplicationUser.cs
│
├── Bulky.Utility
│
├── BulkyWeb
│ ├── Areas
│ │ ├── Admin
│ │ │ ├── Controllers
│ │ │ └── Views
│ │ ├── Customer
│ │ │ ├── Controllers
│ │ │ └── Views
│ │ └── Identity
│ ├── ViewComponents
│ ├── Views
│ └── wwwroot
│
└── Bulky.slnx
```


This architecture follows separation of concerns and scalable enterprise patterns.

## Technologies

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server
- Stripe API
- Bootstrap 5
- Azure Deployment
- IIS Hosting

## Requirements

- Visual Studio 2022
- SQL Server / SQL Server Management Studio
- .NET 8 SDK

## Running the Project

1. Clone the repository:

```
git clone https://github.com/DorisC-debug/Bulky-MVC2.git
```
2. Update the connection string in appsettings.json.

3. Apply migrations:
```
update-database
```
4. Run the project from Visual Studio.

## What This Project Demonstrates

Enterprise-level project structure

Clean architecture with Repository Pattern

Authentication & role-based authorization

Secure payment integration

Database migrations & seeding

Production-ready deployment strategy

👩‍💻 Author

DorisC-debug
