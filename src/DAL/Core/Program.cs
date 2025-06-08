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
        private static ILogger _logger = null!;
        private static DatabaseInitializer _dbInitializer = null!;

        static void Main(string[] args)
        {
            // Initialize dependencies
            IDatabaseConfig dbConfig = new DatabaseConfig();
            _logger = new ConsoleLogger();
            _agentRepo = new AgentDAL(dbConfig, _logger);
            _dbInitializer = new DatabaseInitializer(dbConfig, _logger);

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
                    DisplayMainMenu();
                    var choice = Console.ReadLine();

                    // Handle user menu selection
                    switch (choice)
                    {
                        case "1":
                            DisplayAllAgents();
                            break;
                        case "2":
                            AddNewAgent();
                            break;
                        case "3":
                            UpdateAgentLocation();
                            break;
                        case "4":
                            DeleteAgent();
                            break;
                        case "5":
                            SearchAgents();
                            break;
                        case "6":
                            DisplayStatusReport();
                            break;
                        case "7":
                            AddMissionCount();
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

        // Displays the main menu options
        static void DisplayMainMenu()
        {
            _logger.LogInfo("=== MAIN MENU ===");
            _logger.LogInfo("1. View All Agents");
            _logger.LogInfo("2. Add New Agent");
            _logger.LogInfo("3. Update Agent Location");
            _logger.LogInfo("4. Delete Agent");
            _logger.LogInfo("5. Search Agents");
            _logger.LogInfo("6. Status Report");
            _logger.LogInfo("7. Add Mission Count");
            _logger.LogInfo("0. Exit");
            _logger.LogInfo("\nEnter your choice: ");
        }

        // Retrieves and displays all agents in the database
        static void DisplayAllAgents()
        {
            _logger.LogInfo("\n=== ALL AGENTS ===");
            var agents = _agentRepo.GetAllAgents();
            if (agents.Count > 0)
            {
                foreach (var agent in agents)
                {
                    _logger.LogInfo($"  ID: {agent.Id} | {agent}");
                }
            }
            else
            {
                _logger.LogInfo("No agents found in the database.");
            }
        }

        // Prompts user for agent details and adds a new agent to the database
        static void AddNewAgent()
        {
            _logger.LogInfo("\n=== ADD NEW AGENT ===");

            var agent = new Agent();

            _logger.LogInfo("Enter Code Name: ");
            agent.CodeName = Console.ReadLine() ?? "";

            _logger.LogInfo("Enter Real Name: ");
            agent.RealName = Console.ReadLine() ?? "";

            _logger.LogInfo("Enter Location: ");
            agent.Location = Console.ReadLine() ?? "";

            _logger.LogInfo("Enter Status (Active/Injured/Missing/Retired): ");
            agent.Status = Console.ReadLine() ?? "";

            _logger.LogInfo("Enter Missions Completed: ");
            if (int.TryParse(Console.ReadLine(), out int missions))
            {
                agent.MissionsCompleted = missions;
            }

            try
            {
                _agentRepo.AddAgent(agent);
                _logger.LogInfo("\nAgent added successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding agent", ex);
            }
        }

        // Updates the location of an existing agent
        static void UpdateAgentLocation()
        {
            _logger.LogInfo("\n=== UPDATE AGENT LOCATION ===");
            DisplayAllAgents();

            _logger.LogInfo("\nEnter Agent ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int agentId))
            {
                _logger.LogInfo("Enter new location: ");
                string newLocation = Console.ReadLine() ?? "";

                try
                {
                    _agentRepo.UpdateAgentLocation(agentId, newLocation);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error updating location", ex);
                }
            }
            else
            {
                _logger.LogInfo("\nInvalid Agent ID.");
            }
        }

        // Deletes an agent from the database
        static void DeleteAgent()
        {
            _logger.LogInfo("\n=== DELETE AGENT ===");
            DisplayAllAgents();

            _logger.LogInfo("\nEnter Agent ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int agentId))
            {
                try
                {
                    _agentRepo.DeleteAgent(agentId);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error deleting agent", ex);
                }
            }
            else
            {
                _logger.LogInfo("\nInvalid Agent ID.");
            }
        }

        // Searches for agents by partial code name match
        static void SearchAgents()
        {
            _logger.LogInfo("\n=== SEARCH AGENTS ===");
            _logger.LogInfo("Enter partial code name to search: ");
            string searchTerm = Console.ReadLine() ?? "";

            try
            {
                var results = _agentRepo.SearchAgentsByCode(searchTerm);
                if (results.Count > 0)
                {
                    _logger.LogInfo("\nSearch Results:");
                    foreach (var agent in results)
                    {
                        _logger.LogInfo($"  ID: {agent.Id} | {agent}");
                    }
                }
                else
                {
                    _logger.LogInfo("\nNo agents found matching the search term.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error searching agents", ex);
            }
        }

        // Displays a count of agents by their status
        static void DisplayStatusReport()
        {
            _logger.LogInfo("\n=== AGENT STATUS REPORT ===");
            try
            {
                var statusCounts = _agentRepo.CountAgentsByStatus();
                foreach (var kvp in statusCounts)
                {
                    _logger.LogInfo($"  {kvp.Key}: {kvp.Value} agents");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error generating status report", ex);
            }
        }

        // Adds to an agent's completed mission count
        static void AddMissionCount()
        {
            _logger.LogInfo("\n=== ADD MISSION COUNT ===");
            DisplayAllAgents();

            _logger.LogInfo("\nEnter Agent ID: ");
            if (int.TryParse(Console.ReadLine(), out int agentId))
            {
                _logger.LogInfo("Enter number of missions to add: ");
                if (int.TryParse(Console.ReadLine(), out int missionsToAdd))
                {
                    try
                    {
                        _agentRepo.AddMissionCount(agentId, missionsToAdd);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error updating mission count", ex);
                    }
                }
                else
                {
                    _logger.LogInfo("\nInvalid number of missions.");
                }
            }
            else
            {
                _logger.LogInfo("\nInvalid Agent ID.");
            }
        }
    }
}