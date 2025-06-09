using EagleEye.DAL.Models;

namespace EagleEye.DAL.Interfaces
{
    public interface IAgentRepository
    {
        void AddAgent(Agent agent);
        List<Agent> GetAllAgents();
        void UpdateAgentLocation(int agentId, string newLocation);
        void DeleteAgent(int agentId);
        List<Agent> SearchAgentsByCode(string partialCode);
        Dictionary<string, int> CountAgentsByStatus();
        void AddMissionCount(int agentId, int missionsToAdd);
    }
}