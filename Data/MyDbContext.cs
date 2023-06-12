using EmployeeManagementBE.Data;
using Microsoft.EntityFrameworkCore;

namespace RepositoryCodeFirstCore.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Employee>()
            //                    .Property(e => e.DateOfBirth)
            //                    .HasColumnType("date");
        }
        public DbSet<Student>? Students { get; set; }
        public DbSet<Subject>? Subjects { get; set; }
        public DbSet<Class>? Classes { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<TaskList>? TaskList { get; set; }
        public DbSet<EmployeeImportTmp>? EmployeeImportTmp { get; set; }
    }
}
