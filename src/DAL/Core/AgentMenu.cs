using EagleEye.DAL.Interfaces;
using EagleEye.DAL.Models;

namespace EagleEye.DAL.Core
{
    public class AgentMenu
    {
        private readonly IAgentRepository _agentRepo;
        private readonly IAppLogger _logger;

        public AgentMenu(IAgentRepository agentRepo, IAppLogger logger)
        {
            _agentRepo = agentRepo;
            _logger = logger;
        }

        // Displays the main menu options
        public void DisplayMainMenu()
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
        public void DisplayAllAgents()
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
        public void AddNewAgent()
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
        public void UpdateAgentLocation()
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
        public void DeleteAgent()
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
        public void SearchAgents()
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
        public void DisplayStatusReport()
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
        public void AddMissionCount()
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