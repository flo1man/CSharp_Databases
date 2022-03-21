using System;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // XML <coonection> <server>.</server> <database>SoftUni</database> </connection>
            // JSON { "server": "."; "database": "SoftUni" }

            using (var connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=SoftUni;Encrypt=false")) //Encrypt=false ???
            {
                connection.Open();
                string query1 = "UPDATE Employees SET Salary = Salary + 0.35";
                string query2 = "SELECT COUNT(*) FROM Employees";
                SqlCommand command1 = new SqlCommand(query1, connection);
                SqlCommand command2 = new SqlCommand(query2, connection);
                //var rowsAffected1 = command1.ExecuteNonQuery();
                //var rowsAffected2 = (int)command2.ExecuteScalar();

                //Console.WriteLine(rowsAffected1);
                //Console.WriteLine(rowsAffected2);


                var sqlCommand = new SqlCommand("SELECT * FROM Employees", connection);
                using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        Console.WriteLine($"{sqlReader["FirstName"]} {sqlReader["MiddleName"]} {sqlReader["LastName"]}");
                        //Console.WriteLine($"{sqlReader[1]} {sqlReader[3]} {sqlReader[2]}");
                    }
                }
            }

        }
    }
}
