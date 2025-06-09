// Main program for the Eagle Eye Field Agent Management System
// This program provides a menu-driven interface for managing field agents
using EagleEye.DAL.Database;
using EagleEye.DAL.Interfaces;
using EagleEye.DAL.Logging;
using EagleEye.DAL.Models;
using EagleEye.DAL.Repositories;

namespace EagleEye.DAL.Core
{
    class Program
    {
        private static IAgentRepository _agentRepo = null!;
        private static IAppLogger _logger = null!;
        private static DatabaseInitializer _dbInitializer = null!;
        private static AgentMenu _menu = null!;

        static void Main(string[] args)
        {
            // Initialize dependencies
            var dbConfig = new DatabaseConfig();
            _logger = new ConsoleLogger();
            _agentRepo = new AgentDAL(dbConfig, _logger);
            _dbInitializer = new DatabaseInitializer(dbConfig, _logger);
            _menu = new AgentMenu(_agentRepo, _logger);

            Console.WriteLine("=== EAGLE EYE FIELD AGENT MANAGEMENT SYSTEM ===\n");

            try
            {
                // Initialize database and ensure it's created
                _dbInitializer.EnsureDatabaseCreated();
                _logger.LogInfo("Database connection established.\n");

                // Main program loop - continues until user chooses to exit
                bool running = true;
                while (running)
                {
                    _menu.DisplayMainMenu();
                    var choice = Console.ReadLine();

                    // Handle user menu selection
                    switch (choice)
                    {
                        case "1":
                            _menu.DisplayAllAgents();
                            break;
                        case "2":
                            _menu.AddNewAgent();
                            break;
                        case "3":
                            _menu.UpdateAgentLocation();
                            break;
                        case "4":
                            _menu.DeleteAgent();
                            break;
                        case "5":
                            _menu.SearchAgents();
                            break;
                        case "6":
                            _menu.DisplayStatusReport();
                            break;
                        case "7":
                            _menu.AddMissionCount();
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            _logger.LogInfo("\nInvalid choice. Please try again.");
                            break;
                    }

                    // Pause and clear screen before next menu display
                    if (running)
                    {
                        _logger.LogInfo("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Application error", ex);
            }

            _logger.LogInfo("\nThank you for using Eagle Eye. Press any key to exit...");
            Console.ReadKey();
        }
    }
}