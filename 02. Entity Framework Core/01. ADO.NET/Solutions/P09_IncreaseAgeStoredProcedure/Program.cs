using System;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace P09_IncreaseAgeStoredProcedure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] input = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection
               ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            {
                connection.Open();

                for (int i = 0; i < input.Length; i++)
                {
                    string uspQuery = $"EXEC usp_GetOlder {input[i]}";
                    SqlCommand uspCommand = new SqlCommand(uspQuery, connection);
                    uspCommand.ExecuteNonQuery();
                }

                string minionQuery = $@"SELECT Name, Age FROM Minions WHERE Id IN({string.Join(",", input)})";
                SqlCommand minionCommand = new SqlCommand(minionQuery, connection);
                var reader = minionCommand.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                }
            }
        }
    }
}
