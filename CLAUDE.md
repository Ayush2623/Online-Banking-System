# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Structure

This is a full-stack online banking system with:
- **Backend**: .NET 8 Web API using layered architecture (Controller → Business → Repository → Model)
- **Frontend**: Angular 16 application with TypeScript and SCSS

### Backend Architecture (C:\Workspace\Online-Banking-System\Backend)

The backend follows a clean layered architecture:

- **OnlineBank**: Main Web API project containing controllers and startup configuration
- **BussinessLayer**: Business logic services and interfaces
- **RepositoryLayer**: Data access layer with Entity Framework Core
- **ModelLayer**: Domain models and DTOs
- **TestProject1**: Unit tests using NUnit and Moq

Key architectural patterns:
- Dependency injection for service registration in `Program.cs:76-87`
- JWT authentication with role-based authorization (Admin/User) configured in `Program.cs:37-73`
- Entity Framework Code First with SQL Server (`OnlineBankDbContext` in RepositoryLayer)
- CORS configured for Angular frontend (localhost:4200) in `Program.cs:126-136`
- Swagger UI enabled with JWT Bearer token support for API documentation

### Frontend Architecture (C:\Workspace\Online-Banking-System\Frontend)

Angular 16 application structure:
- **Guards**: `AuthGuard` and `AccountGuard` for route protection
- **Services**: Authentication, admin, dashboard, fund-transfer, and netbanking services
- **Components**: Login, Register, Admin Dashboard, User Dashboard (lazy-loaded module with sidebar navigation)
- **Interceptors**: JWT token handling in `auth.interceptor.ts`
- **Models**: TypeScript interfaces for type safety across the application
- Uses TailwindCSS for styling (configured in `tailwind.config.js`)

## Development Commands

### Backend (.NET)
Run from `Backend/` directory:
- **Build solution**: `dotnet build OnlineBank.sln`
- **Run API**: `dotnet run --project OnlineBank/OnlineBank.csproj` (API runs on configured port with Swagger UI)
- **Run tests**: `dotnet test TestProject1/TestProject1.csproj`
- **Run specific test**: `dotnet test TestProject1/TestProject1.csproj --filter "TestMethodName"`
- **Database migrations**: `dotnet ef migrations add <MigrationName> --project RepositoryLayer --startup-project OnlineBank`
- **Update database**: `dotnet ef database update --project RepositoryLayer --startup-project OnlineBank`
- **Remove last migration**: `dotnet ef migrations remove --project RepositoryLayer --startup-project OnlineBank`

### Frontend (Angular)
Run from `Frontend/` directory:
- **Install dependencies**: `npm install`
- **Development server**: `npm start` or `ng serve` (runs on http://localhost:4200)
- **Build**: `npm run build` or `ng build`
- **Production build**: `ng build --configuration production`
- **Run tests**: `npm test` or `ng test`
- **Run specific test**: `ng test --include="**/component-name.component.spec.ts"`
- **Watch build**: `npm run watch` or `ng build --watch --configuration development`
- **Generate component**: `ng generate component component-name`
- **Generate service**: `ng generate service service-name`

## Database Configuration

- Uses Entity Framework Core with SQL Server
- Connection string configured in `Backend/OnlineBank/appsettings.json:10`
- Main entities: Auth, Account, PendingAccount, NetBanking, Transaction, Payee
- Database context: `OnlineBankDbContext` in RepositoryLayer with existing migration `InitialCreate1`
- Entity relationships are established through navigation properties

## Authentication & Security

- JWT-based authentication with configurable key/issuer in `appsettings.json:12-16`
- Role-based authorization with "AdminPolicy" for admin-only endpoints
- Frontend guards protect routes based on authentication status and account verification
- CORS configured specifically for Angular development server (localhost:4200)
- JWT tokens include user roles and are validated on every API request
- Auth interceptor automatically attaches tokens to HTTP requests

## Key Services & Controllers

Backend controllers handle:
- **AuthController**: Login, registration, password reset, OTP verification
- **AccountController**: Account management, approval workflows, account details
- **DashboardController**: User dashboard data, account summaries
- **NetBankingController**: Net banking registration, PIN management
- **FundTransferController**: Money transfers, payee management
- **AdminController**: Administrative functions, pending account approvals

Service layer interfaces ensure loose coupling between controllers and business logic. Each service has a corresponding repository for data access.

## Testing

- Backend uses NUnit with Moq for mocking in TestProject1
- Frontend uses Jasmine/Karma (standard Angular testing framework)
- Test files: `UnitTest1.cs` and `UnitTest2.cs` for backend testing
- Frontend component tests follow Angular conventions with `.spec.ts` files

## Development Workflow

1. **Backend Development**: Start with creating models in ModelLayer, then repositories in RepositoryLayer, services in BussinessLayer, and finally controllers in OnlineBank
2. **Frontend Development**: User Dashboard is a lazy-loaded module with sidebar navigation for better performance
3. **Database Changes**: Always create migrations when modifying entities and update the database
4. **Authentication Flow**: Users must register → get admin approval → enable net banking → access full dashboard features