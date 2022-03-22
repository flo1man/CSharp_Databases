using System;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace P08_IncreaseMinionAge
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
                    string minionQuery = $@"UPDATE Minions SET Age += 1 WHERE Id = {input[i]}";
                    SqlCommand minionCommand = new SqlCommand(minionQuery, connection);
                    minionCommand.ExecuteNonQuery();

                    string updateQuery = $@"UPDATE Minions SET Name = UPPER(SUBSTRING(Name,1,1)) + SUBSTRING(Name, 2, LEN(Name) - 1) WHERE Id = {input[i]}";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.ExecuteNonQuery();
                }

                string readerQuery = "SELECT Name, Age FROM Minions";
                SqlCommand readerCommand = new SqlCommand(readerQuery, connection);
                var reader = readerCommand.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} {reader[1]}");
                }
            }
        }
    }
}
