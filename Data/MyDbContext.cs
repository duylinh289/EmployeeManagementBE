using EmployeeManagementBE.Data;
using EmployeeManagementBE.DTO.Student;
using Microsoft.EntityFrameworkCore;

namespace RepositoryCodeFirstCore.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreCard>()
                .HasKey(s => new { s.StudentCode, s.SubjectId });

            modelBuilder.Entity<StudentClass>()
                .HasKey(s => new { s.StudentCode, s.ClassId });

            modelBuilder.Entity<Student>()
                    .HasMany(s => s.Subjects)
                    .WithMany(c => c.Students)
                    .UsingEntity<ScoreCard>(
                        j => j.HasOne(ss => ss.Subject).WithMany(),
                        j => j.HasOne(ss => ss.Student).WithMany())
                    .ToTable("ScoreCard");
            modelBuilder.Entity<Students_RankDTO>().HasNoKey();
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Student>? Students { get; set; }
        public DbSet<Students_RankDTO>? StudentRank { get; set; }
        public DbSet<Subject>? Subjects { get; set; }
        public DbSet<Class>? Classes { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<ScoreCard>? ScoreCards { get; set; }
        public DbSet<StudentClass>? StudentClasses { get; set; }
        public DbSet<TaskList>? TaskList { get; set; }
        public DbSet<EmployeeImportTmp>? EmployeeImportTmp { get; set; }
    }
}
