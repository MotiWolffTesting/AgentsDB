# Eagle Eye Field Agent Management System

A .NET-based data access layer for managing field agents in the Eagle Eye system.

## Project Structure

```
src/
└── DAL/
    ├── Core/                 # Application entry point and core logic
    ├── Database/            # Database context and configuration
    ├── Infrastructure/      # Infrastructure services
    ├── Interfaces/          # Interface definitions
    ├── Logging/             # Logging implementations
    ├── Models/              # Domain models
    └── Repositories/        # Data access implementations
```

## Setup

1. Create a `.env` file in the project root with the following content:
```
DATABASE_CONNECTION_STRING=Host=localhost;Database=eagleeye;Username=your_username;Password=your_password
```

2. Install dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

## Architecture

The project follows SOLID principles and clean architecture:

- **Single Responsibility Principle**: Each class has a single responsibility
- **Open/Closed Principle**: Components are open for extension but closed for modification
- **Interface Segregation**: Interfaces are specific to client needs
- **Dependency Inversion**: High-level modules depend on abstractions

## Components

- **Models**: Domain entities representing field agents
- **Interfaces**: Contracts for data access and services
- **Repositories**: Implementation of data access patterns
- **Database**: Entity Framework context and configuration
- **Logging**: Logging service implementations
- **Core**: Application entry point and business logic

## Dependencies

- .NET 7.0
- Entity Framework Core
- Npgsql (PostgreSQL provider)
- DotNetEnv (Environment variable management)