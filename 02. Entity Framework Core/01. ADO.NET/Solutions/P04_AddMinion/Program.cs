using System;
using Microsoft.Data.SqlClient;

namespace P04_AddMinion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] minionInfo = Console.ReadLine().Split(" ",StringSplitOptions.RemoveEmptyEntries);
            string[] villainInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string town = minionInfo[3];
            string villainName = villainInfo[1];

            using (SqlConnection connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            {
                connection.Open();
                TownQuering(town, connection);

                VillainQuering(villainName, connection);

                MinionQuering(minionName, minionAge, town, villainName, connection);
            }
        }

        private static void MinionQuering(string minionName, int minionAge, string town, string villainName, SqlConnection connection)
        {
            string minionQuery = $@"SELECT COUNT(*) FROM Minions WHERE Name = '{minionName}'";
            SqlCommand minionCommand = new SqlCommand(minionQuery, connection);
            int minionOutput = (int)minionCommand.ExecuteScalar();

            if (minionOutput == 0)
            {
                string townIdQuery = $@"SELECT Id FROM Towns WHERE Name = '{town}'";
                SqlCommand townCommand = new SqlCommand(townIdQuery, connection);
                int townId = (int)townCommand.ExecuteScalar();

                minionQuery = $@"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', {minionAge}, {townId})";
                minionCommand = new SqlCommand(minionQuery, connection);
                minionCommand.ExecuteNonQuery();

                string villainIdQuery = $@"SELECT Id FROM Villains WHERE Name = '{villainName}'";
                SqlCommand villainCommand = new SqlCommand(villainIdQuery, connection);
                int villainId = (int)villainCommand.ExecuteScalar();

                string minionIdQuery = $@"SELECT Id FROM Minions WHERE Name = '{minionName}'";
                SqlCommand minionIdCommand = new SqlCommand(minionIdQuery, connection);
                int minionId = (int)minionIdCommand.ExecuteScalar();

                string insertQuery = $@"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({minionId}, {villainId})";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }

        private static void VillainQuering(string villainName, SqlConnection connection)
        {
            string villainQuery = $@"SELECT COUNT(*) FROM Villains WHERE Name = '{villainName}'";
            SqlCommand villainCommand = new SqlCommand(villainQuery, connection);
            int villainOutput = (int)villainCommand.ExecuteScalar();

            if (villainOutput == 0)
            {
                villainQuery = @$"INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('{villainName}', 4)";
                villainCommand = new SqlCommand(villainQuery, connection);
                villainCommand.ExecuteNonQuery();
                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static void TownQuering(string town, SqlConnection connection)
        {
            string townQuery = $@"SELECT COUNT(*) FROM Towns WHERE Name = '{town}'";
            SqlCommand townCommand = new SqlCommand(townQuery, connection);
            int townOutput = (int)townCommand.ExecuteScalar();

            if (townOutput == 0)
            {
                townQuery = @$"INSERT INTO Towns (CountryCode, Name) VALUES (2, '{town}')";
                townCommand = new SqlCommand(townQuery, connection);
                townCommand.ExecuteNonQuery();
                Console.WriteLine($"Town {town} was added to the database.");
            }
        }
    }
}
