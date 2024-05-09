using Microsoft.EntityFrameworkCore;
using Task_ejabisoft.Models.Entity;
using Task_ejabisoft.Models.EntityConfigurations;

namespace Task_ejabisoft.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EmployeeEntityConfigurations());
            modelBuilder.ApplyConfiguration(new JobEntityConfigurations());
        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<EmployeeJob> EmployeeJobs { get; set; }
    }
}
