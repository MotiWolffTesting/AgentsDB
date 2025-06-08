# Eagle Eye Field Agent Management System

🪖 A secret military unit database system for managing field agents built with C# and PostgreSQL.

## Project Structure

```
EagleEye/
├── Models/
│   └── Agent.cs          # Agent model class
├── DAL/
│   └── AgentDAL.cs       # Data Access Layer
├── Program.cs            # Main program
└── README.md
```

## Database Schema

**Database:** `eagleeyedb`  
**Table:** `agents`

| Column | Type | Constraints |
|--------|------|-------------|
| id | INT | AUTO_INCREMENT, PRIMARY KEY |
| codename | VARCHAR(50) | NOT NULL, UNIQUE |
| realname | VARCHAR(100) | NOT NULL |
| location | VARCHAR(100) | NOT NULL |
| status | VARCHAR(20) | NOT NULL |
| missionscompleted | INT | DEFAULT 0 |

**Valid Status Values:** "Active", "Injured", "Missing", "Retired"

## Setup Instructions

1. **Install NuGet Packages:**
   ```bash
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

2. **Configure Database Connection:**
   - Update the connection string in `DAL/AgentDAL.cs`
   - Replace: `Host=localhost;Database=eagleeyedb;Username=postgres;Password=your_password`

3. **Create Database:**
   ```sql
   CREATE DATABASE eagleeyedb;
   ```

4