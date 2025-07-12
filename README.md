# 🍕 BlazorApp - Restaurant Ordering System

<div align="center">

![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

*A modern, full-featured restaurant ordering system built with Blazor WebAssembly*

[Features](#-features) • [Installation](#-installation) • [Usage](#-usage) • [API](#-api) • [Contributing](#-contributing)

</div>

---

## 📖 About

**BlazorApp** is a comprehensive restaurant ordering system built with modern web technologies. The application provides a seamless experience for customers to browse menus, place orders, make reservations, and process payments, while offering restaurant staff powerful management tools.

### 🎯 Key Highlights

- **Modern Architecture**: Built with Blazor WebAssembly for rich client-side interactions
- **Full-Stack Solution**: Includes both client and server components with a robust API
- **Real-time Features**: Live updates and interactive user experience
- **Secure**: Comprehensive authentication and authorization system
- **Scalable**: Containerized with Docker for easy deployment
- **Responsive**: Mobile-friendly design for all devices

## ✨ Features

### 🍽️ Customer Features
- **Browse Menu**: Explore categorized food items (Burgers, Pizza, Pasta, Fries)
- **Shopping Cart**: Add, remove, and modify items with real-time price calculation
- **User Registration & Login**: Secure account management system
- **Table Reservations**: Book tables for specific dates and times
- **Payment Processing**: Secure payment gateway integration
- **Order History**: Track past orders and reorder favorites
- **Email Notifications**: Receive confirmations and updates

### 👨‍💼 Admin Features
- **User Management**: Comprehensive user administration panel
- **Menu Management**: Add, edit, and remove dishes with categories
- **Order Management**: Process and track customer orders
- **Reservation Management**: Handle table bookings and availability
- **Email Services**: Send notifications and confirmations

### 🔧 Technical Features
- **Responsive Design**: Optimized for desktop, tablet, and mobile
- **Real-time Updates**: Live data synchronization
- **Offline Support**: Progressive Web App capabilities
- **Docker Support**: Full containerization for easy deployment
- **Database Integration**: Entity Framework Core with SQL Server
- **API Documentation**: RESTful API with comprehensive endpoints

## 🚀 Technology Stack

### Frontend
- **Blazor WebAssembly** - Client-side framework
- **Bootstrap** - UI framework and responsive design
- **CSS3 & HTML5** - Modern web standards
- **JavaScript Interop** - Enhanced client functionality

### Backend
- **ASP.NET Core 8.0** - Server-side API
- **Entity Framework Core** - ORM and database operations
- **AutoMapper** - Object-to-object mapping
- **MailKit** - Email services

### Database
- **Microsoft SQL Server** - Primary database
- **Entity Framework Migrations** - Database versioning

### DevOps & Deployment
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **Nginx** - Web server for client deployment

## 📋 Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)
- [Node.js](https://nodejs.org/) (for any frontend tooling)

## 🔧 Installation

### Option 1: Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Kwameldx666/BlazorApp.git
   cd BlazorApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Setup the database**
   ```bash
   # Navigate to the server project
   cd BlazorApp.Server/Server
   
   # Update database connection string in appsettings.json
   # Run migrations
   dotnet ef database update
   ```

4. **Run the applications**
   
   **Start the API Server:**
   ```bash
   cd BlazorApp.Server/Server
   dotnet run
   ```
   
   **Start the Blazor Client:**
   ```bash
   cd BlazorApp
   dotnet run
   ```

### Option 2: Docker Deployment

1. **Using Docker Compose (Recommended)**
   ```bash
   # Clone the repository
   git clone https://github.com/Kwameldx666/BlazorApp.git
   cd BlazorApp
   
   # Start all services
   docker-compose up -d
   ```

2. **Manual Docker Build**
   ```bash
   # Build and run the server
   docker build -f Dockerfile.server -t blazorapp-server .
   docker run -p 8080:8080 blazorapp-server
   
   # Build and run the client
   docker build -f Dockerfile.client -t blazorapp-client .
   docker run -p 80:80 blazorapp-client
   ```

## 🎮 Usage

### Accessing the Application

- **Client Application**: http://localhost:5000 (or configured port)
- **API Server**: http://localhost:8080 (or configured port)
- **Database**: SQL Server on localhost:1433 (when using Docker Compose)

### Default Credentials

After initial setup, you can create an admin account through the registration process or use the user management system.

### API Endpoints

The application provides a comprehensive REST API. Key endpoints include:

- `GET /api/dishes` - Retrieve menu items
- `POST /api/cart` - Manage shopping cart
- `POST /api/reservations` - Handle table reservations
- `POST /api/auth/login` - User authentication
- `POST /api/auth/register` - User registration

## 📁 Project Structure

```
BlazorApp/
├── BlazorApp/                          # Blazor WebAssembly Client
│   ├── Layout/                         # Application layouts
│   ├── Pages/                          # Razor pages and components
│   │   ├── Authentication/             # Login and registration
│   │   ├── Cart/                       # Shopping cart management
│   │   ├── Dish/                       # Menu and dish display
│   │   ├── Home/                       # Home page
│   │   ├── Payment/                    # Payment processing
│   │   ├── Reservation/                # Table reservations
│   │   └── User/                       # User management
│   ├── wwwroot/                        # Static files and assets
│   └── BlazorApp.csproj               # Client project file
├── BlazorApp.Server/                   # Server-side components
│   ├── Server/                         # ASP.NET Core API
│   ├── Shared/                         # Shared models and DTOs
│   └── Client/                         # Additional client components
├── docker-compose.yml                  # Docker orchestration
├── Dockerfile.client                   # Client container definition
├── Dockerfile.server                   # Server container definition
└── README.md                          # This file
```

## 🛠️ Development

### Building the Application

```bash
# Build the entire solution
dotnet build

# Build specific projects
dotnet build BlazorApp/BlazorApp.csproj
dotnet build BlazorApp.Server/Server/BlazorApp.Server.csproj
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Database Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName -p BlazorApp.Server/Server

# Update database
dotnet ef database update -p BlazorApp.Server/Server
```

## 🌐 Deployment

### Production Deployment

1. **Configure Connection Strings**: Update `appsettings.Production.json` with production database connection
2. **Build for Production**: Use `dotnet publish -c Release`
3. **Deploy with Docker**: Use the provided Docker files for containerized deployment
4. **Environment Variables**: Set appropriate environment variables for production

### Environment Variables

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=YourProductionConnectionString
ASPNETCORE_URLS=http://+:8080
```

## 🤝 Contributing

We welcome contributions to improve BlazorApp! Here's how you can help:

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/amazing-feature`
3. **Make your changes**: Follow our coding standards
4. **Add tests**: Ensure your changes are well-tested
5. **Commit your changes**: `git commit -m 'Add amazing feature'`
6. **Push to the branch**: `git push origin feature/amazing-feature`
7. **Open a Pull Request**

### Development Guidelines

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure responsive design principles
- Test across different browsers and devices

## 📝 License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## 🙏 Acknowledgments

- Built with [Blazor](https://blazor.net/) by Microsoft
- UI components powered by [Bootstrap](https://getbootstrap.com/)
- Icons from [Font Awesome](https://fontawesome.com/)
- Containerization with [Docker](https://www.docker.com/)

## 📞 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/Kwameldx666/BlazorApp/issues) page
2. Create a new issue with detailed information
3. Provide steps to reproduce any bugs

---

<div align="center">

**Made with ❤️ using Blazor WebAssembly**

⭐ Star this repository if you found it helpful!

</div>