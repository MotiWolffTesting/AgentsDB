// Data Access Layer and Database Context for the Eagle Eye system
using Microsoft.EntityFrameworkCore;
using EagleEye.DAL.Database;
using DotNetEnv;

namespace EagleEye.DAL.Database
{
    // Database configuration implementation
    public class DatabaseConfig
    {
        private readonly string _connectionString;

        public DatabaseConfig()
        {
            Env.Load();
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
                ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING not found in .env file");
        }

        public DatabaseConfig(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString() => _connectionString;

        public DbContextOptions<EagleEyeDbContext> GetDbContextOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EagleEyeDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);
            return optionsBuilder.Options;
        }
    }
}