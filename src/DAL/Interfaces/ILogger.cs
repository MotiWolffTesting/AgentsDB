// Data Access Layer and Database Context for the Eagle Eye system
namespace EagleEye.DAL.Interfaces
{
    // Interface for logging
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex = null);
    }
}