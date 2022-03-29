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

            // 3 
            //Console.WriteLine(GetEmployeesFullInformation(db));

            // 4 
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(db));

            // 5 
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(db));

            // 6 
            //Console.WriteLine(AddNewAddressToEmployee(db));

            // 7 
            //Console.WriteLine(GetEmployeesInPeriod(db));

            // 8 
            //Console.WriteLine(GetAddressesByTown(db));

            // 9 
            //Console.WriteLine(GetEmployee147(db));

            // 10 
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(db));

            // 11 
            //Console.WriteLine(GetLatestProjects(db));

            // 12 
            //Console.WriteLine(IncreaseSalaries(db));

            // 13 
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(db));

            // 14 
            //Console.WriteLine(DeleteProjectById(db));

            // 15 
            //Console.WriteLine(RemoveTown(db));
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var empProjects = context.EmployeesProjects
                .Where(x => x.ProjectId == 2)
                .ToList();

            foreach (var item in empProjects)
            {
                context.EmployeesProjects.Remove(item);
            }

            context.SaveChanges();

            var project = context.Projects.Find(2);
            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects.Take(10).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in projects)
            {
                sb.AppendLine(item.Name);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Include(x => x.Employees)
                .Include(x => x.Town)
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    Count = x.Employees.Where(e => e.Address.AddressText == x.AddressText).Count()
                    // Count = x.Employees.Count(),
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TownName)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.Count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .Include(x => x.Addresses)
                .FirstOrDefault(x => x.Name == "Seattle");

            var addresses = town.Addresses.Select(x => x.AddressId).ToList();

            var employees = context.Employees
                .Where(x => x.AddressId.HasValue && addresses.Contains(x.AddressId.Value))
                .ToList();

            foreach (var item in employees)
            {
                item.AddressId = null;
            }

            foreach (var item in addresses)
            {
                var addres = context.Addresses
                    .FirstOrDefault(x => x.AddressId == item);

                context.Addresses.Remove(addres);
            }

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{addresses.Count()} addresses in Seattle were deleted";
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                //.Where(x => EF.Functions.Like(x.FirstName, "sa%"))
                .Where(x => x.FirstName.StartsWith("Sa") || x.FirstName.StartsWith("sa"))
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

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(x => x.Departments)
                .Where(x => x.Department.Name == "Engineering"
                    || x.Department.Name == "Tool Design"
                    || x.Department.Name == "Marketing"
                    || x.Department.Name == "Information Services")
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            foreach (var emp in employees)
            {
                emp.Salary *= 1.12m;
            }

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
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
            var departments = context.Departments
                .Select(x => new
                {
                    x.Name,
                    x.Manager.FirstName,
                    x.Manager.LastName,
                    Count = x.Employees.Count(),
                    Employees = x.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()
                })
                .Where(x => x.Count > 5)
                .OrderBy(x => x.Count)
                .ThenBy(x => x.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in departments)
            {
                sb.AppendLine($"{item.Name} - {item.FirstName} {item.LastName}");

                foreach (var emp in item.Employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
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
                })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();


            sb.AppendLine($"{employee.EmployeeName} - {employee.Job}");

            foreach (var project in employee.ProjectName)
            {
                sb.AppendLine($"{project.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.
                        Any(p => p.Project.StartDate.Year >= 2001
                        && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFirstName = x.FirstName,
                    EmployeeLastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        StartDate = p.Project.StartDate,
                        EndDate = p.Project.EndDate == null ? "not finished" : p.Project.EndDate.ToString(),
                    })
                })
                .Take(10)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.EmployeeFirstName} {emp.EmployeeLastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");

                foreach (var project in emp.Projects)
                {
                    sb.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
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
                .Where(x => x.Department.Name == "Research and Development")
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
