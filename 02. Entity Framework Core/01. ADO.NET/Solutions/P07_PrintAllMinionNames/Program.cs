using System;
using Microsoft.Data.SqlClient;

namespace P07_PrintAllMinionNames
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection
               ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            {
                connection.Open();

                string countQuery = $@"SELECT COUNT(*) FROM Minions";
                SqlCommand countCommand = new SqlCommand(countQuery, connection);
                int count = (int)countCommand.ExecuteScalar();

                for (int i = 1; i <= Math.Ceiling(count / 2.0); i++)
                {
                    string idQuery = $@"SELECT Name FROM Minions WHERE Id = {i}";
                    SqlCommand idCommand = new SqlCommand(idQuery, connection);
                    string minion = (string)idCommand.ExecuteScalar();
                    Console.WriteLine(minion);

                    if (Math.Ceiling(count / 2.0) != i)
                    {
                        string idQuery2 = $@"SELECT Name FROM Minions WHERE Id = {count - i + 1}";
                        SqlCommand idCommand2 = new SqlCommand(idQuery2, connection);
                        string minion2 = (string)idCommand2.ExecuteScalar();
                        Console.WriteLine(minion2);
                    }
                }
            }
        }
    }
}
