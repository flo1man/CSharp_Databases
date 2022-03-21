using System;
using Microsoft.Data.SqlClient;

namespace P03_MinionNames
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            {
                connection.Open();
                int input = int.Parse(Console.ReadLine());
                string villainQuery = 
                    @$"SELECT 
	                     CASE
		                 WHEN (SELECT COUNT(*) FROM Villains WHERE Id = {input}) > 0 
		                    THEN (SELECT CONCAT('Villain: ', Name) FROM Villains WHERE Id = {input})
		                 ELSE 'No villain with ID {input} exists in the database.'
	                     END AS [Villian]";

                SqlCommand villainCommand = new SqlCommand(villainQuery, connection);
                string villainOuput = (string)villainCommand.ExecuteScalar();
                Console.WriteLine(villainOuput);

                string minionQuery =
                    $@"SELECT COUNT(*) FROM Villains WHERE Id = {input}";
                SqlCommand minionCommand = new SqlCommand(minionQuery, connection);
                int minionOuput = (int)minionCommand.ExecuteScalar();

                if (minionOuput == 0 && !villainOuput.StartsWith("No"))
                {
                    Console.WriteLine("(no minions)");
                }
                else if (minionOuput > 0)
                {
                    string readerQuery =
                        $@"SELECT Name, Age
	                        FROM Minions m
	                        JOIN MinionsVillains mv ON mv.MinionId = m.Id
	                        WHERE VillainId = {input}
	                        ORDER BY Name";
                    SqlCommand readerCommand = new SqlCommand(readerQuery, connection);
                    var reader = readerCommand.ExecuteReader();

                    int count = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{count}. {reader[0]} {reader[1]}");
                        count++;
                    }
                }
            }
        }
    }
}
