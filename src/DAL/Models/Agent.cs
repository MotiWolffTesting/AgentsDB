// Agent model class representing a field agent in the system
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagleEye.DAL.Models
{
    // Represents a field agent with their details and status
    public class Agent
    {
        // Primary key for the agent
        [Key]
        public int Id { get; set; }

        // Agent's code name (e.g., "007")
        [Required]
        [StringLength(50)]
        public string CodeName { get; set; } = string.Empty;

        // Agent's real name
        [Required]
        [StringLength(100)]
        public string RealName { get; set; } = string.Empty;

        // Current location of the agent
        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        // Current status of the agent (Active/Injured/Missing/Retired)
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        // Number of missions completed by the agent
        public int MissionsCompleted { get; set; } = 0;

        // String representation of the agent
        public override string ToString()
        {
            return $"Agent [{CodeName}] - {RealName} | Location: {Location} | Status: {Status} | Missions: {MissionsCompleted}";
        }
    }

    // Enum defining possible agent statuses
    public enum AgentStatus
    {
        Active,
        Injured,
        Missing,
        Retired
    }
}