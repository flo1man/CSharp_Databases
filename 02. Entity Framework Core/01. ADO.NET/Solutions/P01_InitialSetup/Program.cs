using System;
using Microsoft.Data.SqlClient;

namespace P01_InitialSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //using (var connection = new SqlConnection
            //    ("Server=.;Integrated Security=true;Database=master;Encrypt=false"))
            //{
            //    connection.Open();

            //    //Create DATABASE
            //    string createQuery = "CREATE DATABASE MinionsDB";
            //    SqlCommand command = new SqlCommand(createQuery, connection);
            //    command.ExecuteNonQuery();
            //}

            using (var connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            {
                connection.Open();

                // Create Countries
                string tableCountries = "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY," +
                    " Name VARCHAR(100) NOT NULL)";
                SqlCommand tableCountriesCommand = new SqlCommand(tableCountries, connection);
                tableCountriesCommand.ExecuteNonQuery();

                // Create Towns
                string tableTowns = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY," +
                    " CountryCode INT REFERENCES Countries(Id) NOT NULL)";
                SqlCommand tableTownsCommand = new SqlCommand(tableTowns, connection);
                tableTownsCommand.ExecuteNonQuery();

                // Create Minions
                string tableMinions = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50) NOT NULL," +
                    " Age INT, TownId INT REFERENCES Towns(Id) NOT NULL)";
                SqlCommand tableMinionsCommand = new SqlCommand(tableMinions, connection);
                tableMinionsCommand.ExecuteNonQuery();

                // Create EvilnessFactors
                string tableEvilnessFactors = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY," +
                    " Name VARCHAR(100) NOT NULL)";
                SqlCommand tableEFCommand = new SqlCommand(tableEvilnessFactors, connection);
                tableEFCommand.ExecuteNonQuery();

                // Create Villains
                string tableVillains = "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY," +
                    " Name VARCHAR(100) NOT NULL, EvilnessFactorId INT REFERENCES EvilnessFactors(Id))";
                SqlCommand tableVillainsCommand = new SqlCommand(tableVillains, connection);
                tableVillainsCommand.ExecuteNonQuery();

                // Create MinionsVillains
                string tableMinionsVillains = "CREATE TABLE MinionsVillains(MinionId INT REFERENCES Minions(Id)," +
                    " VillainId INT REFERENCES Villains(Id), PRIMARY KEY(MinionId, VillainId))";
                SqlCommand tableMVCommand = new SqlCommand(tableMinionsVillains, connection);
                tableMVCommand.ExecuteNonQuery();
            }
        }
    }
}
