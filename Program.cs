// Main program for the Eagle Eye Field Agent Management System
// This program provides a menu-driven interface for managing field agents
using EagleEye.Models;
using EagleEye.DAL;

namespace EagleEye
{
    class Program
    {
        // Data Access Layer instance for database operations
        private static AgentDAL _agentDAL = new AgentDAL();

        static void Main(string[] args)
        {
            Console.WriteLine("=== EAGLE EYE FIELD AGENT MANAGEMENT SYSTEM ===\n");

            try
            {
                // Initialize database and ensure it's created
                _agentDAL.EnsureDatabaseCreated();
                Console.WriteLine("Database connection established.\n");

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
                            Console.WriteLine("\nInvalid choice. Please try again.");
                            break;
                    }

                    // Pause and clear screen before next menu display
                    if (running)
                    {
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nApplication error: {ex.Message}");
            }

            Console.WriteLine("\nThank you for using Eagle Eye. Press any key to exit...");
            Console.ReadKey();
        }

        // Displays the main menu options
        static void DisplayMainMenu()
        {
            Console.WriteLine("=== MAIN MENU ===");
            Console.WriteLine("1. View All Agents");
            Console.WriteLine("2. Add New Agent");
            Console.WriteLine("3. Update Agent Location");
            Console.WriteLine("4. Delete Agent");
            Console.WriteLine("5. Search Agents");
            Console.WriteLine("6. Status Report");
            Console.WriteLine("7. Add Mission Count");
            Console.WriteLine("0. Exit");
            Console.Write("\nEnter your choice: ");
        }

        // Retrieves and displays all agents in the database
        static void DisplayAllAgents()
        {
            Console.WriteLine("\n=== ALL AGENTS ===");
            var agents = _agentDAL.GetAllAgents();
            if (agents.Count > 0)
            {
                foreach (var agent in agents)
                {
                    Console.WriteLine($"  ID: {agent.Id} | {agent}");
                }
            }
            else
            {
                Console.WriteLine("No agents found in the database.");
            }
        }

        // Prompts user for agent details and adds a new agent to the database
        static void AddNewAgent()
        {
            Console.WriteLine("\n=== ADD NEW AGENT ===");
            
            var agent = new Agent();
            
            Console.Write("Enter Code Name: ");
            agent.CodeName = Console.ReadLine() ?? "";
            
            Console.Write("Enter Real Name: ");
            agent.RealName = Console.ReadLine() ?? "";
            
            Console.Write("Enter Location: ");
            agent.Location = Console.ReadLine() ?? "";
            
            Console.Write("Enter Status (Active/Injured/Missing/Retired): ");
            agent.Status = Console.ReadLine() ?? "";
            
            Console.Write("Enter Missions Completed: ");
            if (int.TryParse(Console.ReadLine(), out int missions))
            {
                agent.MissionsCompleted = missions;
            }

            try
            {
                _agentDAL.AddAgent(agent);
                Console.WriteLine("\nAgent added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError adding agent: {ex.Message}");
            }
        }

        // Updates the location of an existing agent
        static void UpdateAgentLocation()
        {
            Console.WriteLine("\n=== UPDATE AGENT LOCATION ===");
            DisplayAllAgents();
            
            Console.Write("\nEnter Agent ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int agentId))
            {
                Console.Write("Enter new location: ");
                string newLocation = Console.ReadLine() ?? "";
                
                try
                {
                    _agentDAL.UpdateAgentLocation(agentId, newLocation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError updating location: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Agent ID.");
            }
        }

        // Deletes an agent from the database
        static void DeleteAgent()
        {
            Console.WriteLine("\n=== DELETE AGENT ===");
            DisplayAllAgents();
            
            Console.Write("\nEnter Agent ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int agentId))
            {
                try
                {
                    _agentDAL.DeleteAgent(agentId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError deleting agent: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Agent ID.");
            }
        }

        // Searches for agents by partial code name match
        static void SearchAgents()
        {
            Console.WriteLine("\n=== SEARCH AGENTS ===");
            Console.Write("Enter partial code name to search: ");
            string searchTerm = Console.ReadLine() ?? "";
            
            try
            {
                var results = _agentDAL.SearchAgentsByCode(searchTerm);
                if (results.Count > 0)
                {
                    Console.WriteLine("\nSearch Results:");
                    foreach (var agent in results)
                    {
                        Console.WriteLine($"  ID: {agent.Id} | {agent}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo agents found matching the search term.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError searching agents: {ex.Message}");
            }
        }

        // Displays a count of agents by their status
        static void DisplayStatusReport()
        {
            Console.WriteLine("\n=== AGENT STATUS REPORT ===");
            try
            {
                var statusCounts = _agentDAL.CountAgentsByStatus();
                foreach (var kvp in statusCounts)
                {
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value} agents");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError generating status report: {ex.Message}");
            }
        }

        // Adds to an agent's completed mission count
        static void AddMissionCount()
        {
            Console.WriteLine("\n=== ADD MISSION COUNT ===");
            DisplayAllAgents();
            
            Console.Write("\nEnter Agent ID: ");
            if (int.TryParse(Console.ReadLine(), out int agentId))
            {
                Console.Write("Enter number of missions to add: ");
                if (int.TryParse(Console.ReadLine(), out int missionsToAdd))
                {
                    try
                    {
                        _agentDAL.AddMissionCount(agentId, missionsToAdd);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nError updating mission count: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid number of missions.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Agent ID.");
            }
        }
    }
}