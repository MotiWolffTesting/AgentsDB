// Data Access Layer and Database Context for the Eagle Eye system
using EagleEye.DAL.Interfaces;
using EagleEye.DAL.Models;
using EagleEye.DAL.Database;

namespace EagleEye.DAL.Repositories
{
    // Data Access Layer class for agent operations
    public class AgentDAL : IAgentRepository
    {
        private readonly IDatabaseConfig _config;
        private readonly ILogger _logger;

        public AgentDAL(IDatabaseConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public void AddAgent(Agent agent)
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                context.Agents.Add(agent);
                context.SaveChanges();
                _logger.LogInfo($"Agent {agent.CodeName} added successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding agent", ex);
                throw;
            }
        }

        public List<Agent> GetAllAgents()
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                return context.Agents.OrderBy(a => a.CodeName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving agents", ex);
                throw;
            }
        }

        public void UpdateAgentLocation(int agentId, string newLocation)
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                var agent = context.Agents.Find(agentId);
                if (agent != null)
                {
                    agent.Location = newLocation;
                    context.SaveChanges();
                    _logger.LogInfo($"Agent {agent.CodeName} location updated to {newLocation}");
                }
                else
                {
                    _logger.LogInfo($"Agent with ID {agentId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating agent location", ex);
                throw;
            }
        }

        public void DeleteAgent(int agentId)
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                var agent = context.Agents.Find(agentId);
                if (agent != null)
                {
                    context.Agents.Remove(agent);
                    context.SaveChanges();
                    _logger.LogInfo($"Agent {agent.CodeName} deleted successfully!");
                }
                else
                {
                    _logger.LogInfo($"Agent with ID {agentId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting agent", ex);
                throw;
            }
        }

        public List<Agent> SearchAgentsByCode(string partialCode)
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                return context.Agents
                    .Where(a => a.CodeName.ToLower().Contains(partialCode.ToLower()))
                    .OrderBy(a => a.CodeName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error searching agents", ex);
                throw;
            }
        }

        public Dictionary<string, int> CountAgentsByStatus()
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                return context.Agents
                    .GroupBy(a => a.Status)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error counting agents by status", ex);
                throw;
            }
        }

        public void AddMissionCount(int agentId, int missionsToAdd)
        {
            using var context = new EagleEyeDbContext(_config.GetDbContextOptions());
            try
            {
                var agent = context.Agents.Find(agentId);
                if (agent != null)
                {
                    agent.MissionsCompleted += missionsToAdd;
                    context.SaveChanges();
                    _logger.LogInfo($"Agent {agent.CodeName} now has {agent.MissionsCompleted} missions completed");
                }
                else
                {
                    _logger.LogInfo($"Agent with ID {agentId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating mission count", ex);
                throw;
            }
        }
    }
}