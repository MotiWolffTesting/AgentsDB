// Data Access Layer and Database Context for the Eagle Eye system
using Microsoft.EntityFrameworkCore;
using EagleEye.DAL.Database;

namespace EagleEye.DAL.Interfaces
{
    // Interface for database configuration
    public interface IDatabaseConfig
    {
        string GetConnectionString();
        DbContextOptions<EagleEyeDbContext> GetDbContextOptions();
    }
}