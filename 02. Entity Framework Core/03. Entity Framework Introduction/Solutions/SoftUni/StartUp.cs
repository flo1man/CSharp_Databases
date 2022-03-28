using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        // To use "Include()" you need to Install only -> Microsoft.EntityFrameworkCore

        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            // 3 ++
            //Console.WriteLine(GetEmployeesFullInformation(db));

            // 4 ++ 
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(db));

            // 5 ++
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(db));

            // 6 ++
            //Console.WriteLine(AddNewAddressToEmployee(db));

            // 7 ??
            Console.WriteLine(GetEmployeesInPeriod(db));

            // 8 ??
            //Console.WriteLine();

            // 9 ++
            //Console.WriteLine(GetEmployee147(db));

            // 10 ??
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(db));

            // 11 ++
            //Console.WriteLine(GetLatestProjects(db));

            // 12 ??
            //Console.WriteLine(IncreaseSalary(db));

            // 13 ++
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(db));

            // 14
            //Console.WriteLine();

            // 15 ??
            //Console.WriteLine(RemoveTown(db));
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var addresses = context.Addresses.Where(x => x.TownId == 4).ToList();

            var employees = context.Employees
                .Where(x => addresses.Any(a => a.AddressId == x.AddressId))
                .ToList();

            foreach (var item in employees)
            {
                item.AddressId = null;
            }


            var town = context.Towns.Find("Seattle");
            context.Towns.Remove(town);

            context.SaveChanges();

            return "";
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                //.Where(x => EF.Functions.Like(x.FirstName, "sa%"))
                .Where(x => x.FirstName.StartsWith("Sa"))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalary(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Salary,
                    Department = x.Departments
                     .Where(d => d.DepartmentId == x.DepartmentId 
                      && (d.Name == "Engineering" || d.Name == "Tool Design"
                      || d.Name == "Marketing" || d.Name == "Information Services"))
                })
                .Where(x => x.Department.Count() > 0)
                .ToList();

            return "";
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate).Take(10);

            StringBuilder sb = new StringBuilder();

            foreach (var item in projects.OrderBy(x => x.Name))
            {
                sb.AppendLine(item.Name);
                sb.AppendLine(item.Description);
                sb.AppendLine(item.StartDate.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            return "";
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(e => new
                {
                    EmployeeName = e.FirstName + " " + e.LastName,
                    Job = e.JobTitle,
                    EmployeesProjects = e.EmployeesProjects
                    .Where(x => x.EmployeeId == e.EmployeeId),
                    ProjectName = e.EmployeesProjects.Where(x => x.EmployeeId == e.EmployeeId)
                        .Select(p => new { Name = p.Project.Name })
                        .OrderBy(p => p.Name)
                        .ToList()
                });

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employee)
            {
                sb.AppendLine($"{emp.EmployeeName} - {emp.Job}");

                foreach (var project in emp.ProjectName)
                {
                    sb.AppendLine($"{project.Name}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            //var employees = context.Employees
            //    .Select(e => new
            //    {
            //        EmployeeName = e.FirstName + " " + e.LastName,
            //        EmployeeId = e.EmployeeId,
            //        EmployeesProjects = e.EmployeesProjects
            //        .Where(x => x.EmployeeId == e.EmployeeId),
            //        ProjectName = e.EmployeesProjects.Where(x => x.EmployeeId == e.EmployeeId)
            //            .Select(p => new 
            //            {   Name = p.Project.Name,
            //                Start = p.Project.StartDate,
            //                End = p.Project.EndDate == null ? "not finished" : p.Project.EndDate.ToString()
            //            })
            //    }).ToList();

            var employees = context.Employees
                .Include(x => x.Manager)
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Take(10)
                .ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.Manager.FirstName} {emp.Manager.LastName}");

                foreach (var project in emp.EmployeesProjects)
                {
                    Console.WriteLine($"--{project.Project.Name} - {project.Project.StartDate} - {project.Project.EndDate}");
                }
            }
            return "";
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(address);
            context.SaveChanges();

            var employee = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            employee.AddressId = address.AddressId;

            context.SaveChanges();

            var addresses = context.Employees
                .Select(x => new
                {
                    x.Address.AddressId,
                    x.Address.AddressText
                })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in addresses)
            {
                sb.AppendLine(item.AddressText);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Department.Name  == "Research and Development")
                .Select(x => new
                {
                    Department = x.Department.Name,
                    x.Salary,
                    x.FirstName,
                    x.LastName,
                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName} from {item.Department} - ${item.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(x => new { FirstName = x.FirstName, Salary = x.Salary })
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName);

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} - {item.Salary:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.Select(x => new
            {
                x.EmployeeId,
                x.FirstName,
                x.LastName,
                x.MiddleName,
                x.JobTitle,
                Salary = $"{x.Salary:f2}"
            }).OrderBy(x => x.EmployeeId);

            StringBuilder sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine($"{item.FirstName} {item.LastName}" +
                    $" {item.MiddleName} {item.JobTitle} {item.Salary}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
