using Microsoft.EntityFrameworkCore;
using NStudents.Models.Entity;

namespace NStudents.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Majors> Majors { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Majors - Classes (1-N)
            modelBuilder.Entity<Classes>()
                .HasOne(c => c.majors)
                .WithMany(m => m.Classes)
                .HasForeignKey(c => c.MajorsId)
                .OnDelete(DeleteBehavior.Cascade);

            // Classes - Students (1-N)
            modelBuilder.Entity<Students>()
                .HasOne(s => s.Classes)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassesId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chuyển kiểu ngày sang UTC
            modelBuilder.Entity<Students>()
                .Property(s => s.NgaySinh)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder.Entity<Students>()
                .Property(s => s.NgayNhapHoc)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));


            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<User>(u => u.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

        }
    }
}
