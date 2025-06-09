using EagleEye.DAL.Interfaces;
using EagleEye.DAL.Database;

namespace EagleEye.DAL.Database
{
    // Database initializer
    public class DatabaseInitializer
    {
        private readonly DatabaseConfig _config;
        private readonly IAppLogger _logger;

        public DatabaseInitializer(DatabaseConfig config, IAppLogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public void EnsureDatabaseCreated()
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                bool created = context.Database.EnsureCreated();
                if (created)
                {
                    _logger.LogInfo("Database and tables created successfully with correct structure.");
                }
                else
                {
                    _logger.LogInfo("Database already exists.");
                }

                var canConnect = context.Database.CanConnect();
                _logger.LogInfo($"Database connection test: {(canConnect ? "Success" : "Failed")}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ensuring database exists", ex);
                throw;
            }
        }
    }
}