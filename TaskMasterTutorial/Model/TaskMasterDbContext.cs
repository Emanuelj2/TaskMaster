using System.Data.Entity;
using System.Security.Permissions; // Assuming you are using Entity Framework for database context

namespace TaskMasterTutorial.Model
{
    internal class TaskMasterDbContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
