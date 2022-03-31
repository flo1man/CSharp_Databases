using System;
using Demo.Models;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureDeleted(); // DELETE db
            db.Database.EnsureCreated(); // CREATE db

            var department = new Department
            {
                Name = "HR"
            };

            db.Employees.Add(new Employee
            {
                FirstName = "Ivan",
                LastName = "Kostov",
                StartWorkDate = DateTime.UtcNow,
                Salary = 100,
                Department = department,
            });

            db.SaveChanges();
        }
    }
}
