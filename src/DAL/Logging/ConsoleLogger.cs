using EagleEye.DAL.Interfaces;

namespace EagleEye.DAL.Logging
{
    // Console logger implementation
    public class ConsoleLogger : IAppLogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message, Exception? ex = null)
        {
            Console.WriteLine($"Error: {message}");
            if (ex?.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }
}