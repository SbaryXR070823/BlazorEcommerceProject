# PC Parts Store - Blazor Ecommerce Project

A modern, responsive e-commerce web application for selling computer parts and components, built with Blazor and .NET 9.

## About The Project

PC Parts Store is a full-featured e-commerce platform specifically designed for selling computer hardware components. The application provides a seamless shopping experience with modern UI/UX design, comprehensive product search and filtering capabilities, user account management, shopping cart functionality, and secure checkout process.

### Key Features

- **Product Catalog**: Browse through categorized PC components with detailed specifications
- **Search Functionality**: Lucene.NET-powered search for finding products quickly
- **User Authentication**: Secure login and registration with ASP.NET Core Identity
- **Shopping Cart**: Add products to cart, modify quantities, and checkout
- **Order Management**: View and track orders
- **Admin Panel**: Manage products, categories, and orders
- **Responsive Design**: Modern UI that works across desktop and mobile devices

## Built With

- **Frontend**:
  - [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) - Client-side web UI framework
  - [Bootstrap 5](https://getbootstrap.com/) - CSS framework for responsive design
  - [Bootstrap Icons](https://icons.getbootstrap.com/) - Icon library
  
- **Backend**:
  - [ASP.NET Core 9](https://dotnet.microsoft.com/apps/aspnet) - Web framework
  - [Blazor Server](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) - Server-side rendering
  - [Entity Framework Core 9](https://docs.microsoft.com/en-us/ef/core/) - ORM for database operations
  - [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) - Authentication and authorization
  
- **Database**:
  - [SQL Server](https://www.microsoft.com/en-us/sql-server) - Relational database

- **Additional Libraries**:
  - [Lucene.NET](https://lucenenet.apache.org/) - For search functionality
  - [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage) - Local storage in Blazor WebAssembly
  - [LinqKit](https://github.com/scottksmith95/LINQKit) - Extensions for LINQ to SQL and EF

## Architecture

This project follows a clean architecture approach with clear separation of concerns:

### Solution Structure

- **PcPartsStore** (Server Project)
  - Contains API controllers, services implementation, and server-side Blazor components
  - Handles database connections and authentication
  - Implements business logic
  
- **PcPartsStore.Client** (WebAssembly Project)
  - Client-side Blazor components and pages
  - Interacts with the server via API calls
  - Provides interactive user interface
  
- **Shared** (Class Library)
  - Contains models, DTOs, and interfaces shared between server and client
  - Ensures consistency in data structures

### Design Patterns

- **Repository Pattern**: Abstracts data access layer, making the application more testable
- **Unit of Work**: Manages database transactions and ensures data consistency
- **Dependency Injection**: Used throughout the application for loose coupling
- **Service Layer Pattern**: Business logic encapsulated in service classes

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer edition is sufficient)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (17.13 or higher) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. Clone the repository
   ```sh
   git clone https://github.com/SbaryXR070823/BlazorEcommerceProject
   ```

2. Update the connection string in `appsettings.json` to point to your SQL Server instance
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YourServerName;Database=PcPartsStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

3. Navigate to the project directory and restore dependencies
   ```sh
   cd PcPartsStore
   dotnet restore
   ```

4. Apply database migrations
   ```sh
   cd PcPartsStore/PcPartsStore
   dotnet ef database update
   ```

5. Run the application
   ```sh
   dotnet run
   ```

6. Access the application at `https://localhost:5001` or `https://localhost:7071`

### Default Admin Account

After running the application for the first time, a default admin account is created:
- **Email**: admin@admin.com
- **Password**: 12345678

> **Important**: Remember to change these credentials in a production environment.

## Project Structure Highlights

- **Controllers**: API endpoints for client-server communication
- **Services**: Business logic implementation
- **Repository**: Data access abstraction
- **Models**: Entity definitions mapped to database tables
- **Components/Pages**: UI elements and pages
- **wwwroot**: Static assets (CSS, JS, images)

## License

Distributed under the MIT License. See `LICENSE` for more information.

