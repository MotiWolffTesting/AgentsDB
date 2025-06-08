// Data Access Layer and Database Context for the Eagle Eye system
using Microsoft.EntityFrameworkCore;
using EagleEye.Models;
using DotNetEnv;

namespace EagleEye.DAL
{
    // Data Access Layer class for agent operations
    public class AgentDAL
    {
        // Connection string for the database
        private readonly string _connectionString;

        // Default constructor - loads connection string from .env file
        public AgentDAL()
        {
            // Load environment variables from .env file
            Env.Load();
            
            // Get connection string from environment variable
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
                ?? throw new InvalidOperationException("DATABASE_CONNECTION_STRING not found in .env file");
        }

        // Constructor with explicit connection string
        public AgentDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Creates and returns database context options
        private DbContextOptions<EagleEyeDbContext> GetDbContextOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EagleEyeDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);
            return optionsBuilder.Options;
        }

        // Adds a new agent to the database
        public void AddAgent(Agent agent)
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                context.Agents.Add(agent);
                context.SaveChanges();
                Console.WriteLine($"Agent {agent.CodeName} added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding agent: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        // Retrieves all agents from the database
        public List<Agent> GetAllAgents()
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                return context.Agents.OrderBy(a => a.CodeName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving agents: {ex.Message}");
                throw;
            }
        }

        // Updates an agent's location
        public void UpdateAgentLocation(int agentId, string newLocation)
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                var agent = context.Agents.Find(agentId);
                if (agent != null)
                {
                    agent.Location = newLocation;
                    context.SaveChanges();
                    Console.WriteLine($"Agent {agent.CodeName} location updated to {newLocation}");
                }
                else
                {
                    Console.WriteLine($"Agent with ID {agentId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating agent location: {ex.Message}");
                throw;
            }
        }

        // Deletes an agent from the database
        public void DeleteAgent(int agentId)
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                var agent = context.Agents.Find(agentId);
                if (agent != null)
                {
                    context.Agents.Remove(agent);
                    context.SaveChanges();
                    Console.WriteLine($"Agent {agent.CodeName} deleted successfully!");
                }
                else
                {
                    Console.WriteLine($"Agent with ID {agentId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting agent: {ex.Message}");
                throw;
            }
        }

        // Searches for agents by partial code name match
        public List<Agent> SearchAgentsByCode(string partialCode)
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                return context.Agents
                    .Where(a => a.CodeName.ToLower().Contains(partialCode.ToLower()))
                    .OrderBy(a => a.CodeName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching agents: {ex.Message}");
                throw;
            }
        }

        // Counts agents by their status
        public Dictionary<string, int> CountAgentsByStatus()
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                return context.Agents
                    .GroupBy(a => a.Status)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting agents by status: {ex.Message}");
                throw;
            }
        }

        // Adds to an agent's completed mission count
        public void AddMissionCount(int agentId, int missionsToAdd)
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            
            try
            {
                var agent = context.Agents.Find(agentId);
                if (agent != null)
                {
                    agent.MissionsCompleted += missionsToAdd;
                    context.SaveChanges();
                    Console.WriteLine($"Agent {agent.CodeName} now has {agent.MissionsCompleted} missions completed");
                }
                else
                {
                    Console.WriteLine($"Agent with ID {agentId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating mission count: {ex.Message}");
                throw;
            }
        }

        // Ensures the database is created and properly configured
        public void EnsureDatabaseCreated()
        {
            using var context = new EagleEyeDbContext(GetDbContextOptions());
            try
            {
                // Delete and recreate the database tables
                context.Database.EnsureDeleted();
                Console.WriteLine("Existing database tables deleted.");
                
                bool created = context.Database.EnsureCreated();
                if (created)
                {
                    Console.WriteLine("Database and tables created successfully with correct structure.");
                }
                
                // Test the connection
                var canConnect = context.Database.CanConnect();
                Console.WriteLine($"Database connection test: {(canConnect ? "Success" : "Failed")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recreating database: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }

    // Entity Framework database context for the Eagle Eye system
    public class EagleEyeDbContext : DbContext
    {
        public EagleEyeDbContext(DbContextOptions<EagleEyeDbContext> options) : base(options) { }

        // DbSet for agents table
        public DbSet<Agent> Agents { get; set; }

        // Configures the database model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Agent>(entity =>
            {
                entity.ToTable("agents");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("codename");

                entity.Property(e => e.RealName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("realname");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.MissionsCompleted)
                    .HasColumnName("missionscompleted")
                    .HasDefaultValue(0);

                // Create unique index on codename
                entity.HasIndex(e => e.CodeName).IsUnique();
            });
        }
    }
}