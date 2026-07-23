using Microsoft.EntityFrameworkCore;
using Task_management.Models;
namespace Task_management.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }
    }
}
