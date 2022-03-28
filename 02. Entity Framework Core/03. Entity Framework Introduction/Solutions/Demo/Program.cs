using Demo.Models;
using System;
using System.Linq;

namespace Demo
{
    /*
        Entity Framework Core - Setup :
        Microsoft.EntityFrameworkCore.SqlServer
        Microsoft.EntityFrameworkCore.Design

        Commands for Database first method :

        dotnet ef dbcontext scaffold 
        "Server=.;Database={name};Integrated Security=true" 
        Microsoft.EntityFrameworkCore.SqlServer (optional) ->  -o Models

        Commands for Code first method :
        
        dotnet ef migrations add {MigrationName}
        dotnet ef migrations remove {MigrationName}

        -- Commit changes to the database
        dotnet ef database update
                or
        db.Database.Migrate();
    */

    public class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            var employees = db.Employees.ToList();

            employees[4].JobTitle = "Tool Designer";

            db.SaveChanges();
        }
    }
}
