using Demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        // ConnectionString
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=EFCoreDemo;Integrated Security=true");
            }
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<EmployeeInClub> EmployeesInClub { get; set; }

        // For FLUENT API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasOne(x => x.Employee)
                .WithOne(x => x.Address); // ONE TO ONE

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.DepartmentId); // ONE TO MANY

            modelBuilder.Entity<EmployeeInClub>()
                .HasKey(x => new { x.EmployeeId, x.ClubId }); // TWO Primary keys / composite key

            modelBuilder.Entity<Employee>().Property(x => x.FirstName)
                .IsRequired(); // NOT NULL

            modelBuilder.Entity<Employee>().Property(x => x.LastName)
                .IsRequired()  // NOT NULL
                .HasMaxLength(20);

            modelBuilder.Entity<Employee>().Property(x => x.FirstName)
                .HasMaxLength(20);

            modelBuilder.Entity<Employee>()
                .HasKey(x => new { x.Id, x.EID }); // TWO Primary keys / composite key

            modelBuilder.Entity<Employee>()
                .Ignore(x => x.FullName);

            modelBuilder.Entity<Employee>()
                .ToTable("People", "company"); // RENAME Table and schema

            modelBuilder.Entity<Employee>().Property(x => x.StartWorkDate)
                .HasColumnName("StartedOn")
                .HasColumnName("date"); // avoid this METHOD!!!!

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Department) // required
                .WithMany(x => x.Employees) // optional (inverse method)
                .HasForeignKey(x => x.DepartmentId) // db column name (optional)
                .OnDelete(DeleteBehavior.Restrict); // default is CASCADE !!!
        }
    }
}
