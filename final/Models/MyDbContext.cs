using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> ops) : base(ops)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<TeacherCourse>().HasKey(pt => new { pt.CourseId, pt.TeacherId });

            mb.Entity<TeacherCourse>()
                .HasOne(bc => bc.Teacher)
                .WithMany(b => b.TeacherCourses)
                .HasForeignKey(bc => bc.TeacherId);

            mb.Entity<TeacherCourse>()
                .HasOne(bc => bc.Course)
                .WithMany(c => c.TeacherCourses)
                .HasForeignKey(bc => bc.CourseId);
            mb.Entity<TeacherCourse>().HasKey(pt => new { pt.Id });
        }
    }
}
