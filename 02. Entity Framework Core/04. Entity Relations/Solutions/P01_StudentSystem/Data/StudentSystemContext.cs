using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(x => x.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Course>()
                .Property(x => x.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Course>()
                .Property(x => x.Description)
                .IsUnicode(true);

            modelBuilder.Entity<Resource>()
                .Property(x => x.Name)
                .IsUnicode(true);

            modelBuilder.Entity<Resource>()
                .Property(x => x.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Homework>()
                .Property(x => x.Content)
                .IsUnicode(false);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(x => new {x.StudentId, x.CourseId});
        }
    }
}
