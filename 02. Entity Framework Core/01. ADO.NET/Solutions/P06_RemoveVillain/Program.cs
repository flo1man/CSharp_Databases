using System;
using Microsoft.Data.SqlClient;

namespace P06_RemoveVillain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            { 
                connection.Open();

                string countQuery = $@"SELECT COUNT(*) FROM Villains WHERE Id = {inputId}";
                SqlCommand countCommand = new SqlCommand(countQuery, connection);
                int count = (int)countCommand.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("No such villain was found.");
                }
                else
                {
                    string minionCountQuery = $@"SELECT COUNT(*) FROM MinionsVillains WHERE VillainId = {inputId}";
                    SqlCommand minionCountCommand = new SqlCommand(minionCountQuery, connection);
                    int minionCount = (int)minionCountCommand.ExecuteScalar();

                    string changeToNull = $@"UPDATE MinionsVillains SET VillainId = NULL WHERE VillainId = {inputId}";
                    SqlCommand changeNullCommand = new SqlCommand(changeToNull, connection);
                    changeNullCommand.ExecuteNonQuery();

                    string nameOfVillain = $@"SELECT Name FROM Villains WHERE Id = {inputId}";
                    SqlCommand nameCommand = new SqlCommand(nameOfVillain, connection);
                    string name = (string)nameCommand.ExecuteScalar();

                    string deleteVillain = $@"DELETE FROM Villains WHERE Id = {inputId}";
                    SqlCommand deleteCommand = new SqlCommand(deleteVillain, connection);
                    deleteCommand.ExecuteNonQuery();

                    Console.WriteLine($"{name} was deleted.");
                    Console.WriteLine($"{minionCount} minions were released.");
                }
            }
        }
    }
}
