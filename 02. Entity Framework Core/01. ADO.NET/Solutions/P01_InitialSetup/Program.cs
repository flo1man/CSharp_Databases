using System;
using Microsoft.Data.SqlClient;

namespace P01_InitialSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=master;Encrypt=false"))
            {
                connection.Open();

                //Create DATABASE
                string createQuery = "CREATE DATABASE MinionsDB";
                SqlCommand command = new SqlCommand(createQuery, connection);
                command.ExecuteNonQuery();
            }

            using (var connection = new SqlConnection
                ("Server=.;Integrated Security=true;Database=MinionsDB;Encrypt=false"))
            {
                connection.Open();

                // Create Table Countries
                string tableCountries = "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY," +
                    " Name VARCHAR(100) NOT NULL)";
                SqlCommand tableCountriesCommand = new SqlCommand(tableCountries, connection);
                tableCountriesCommand.ExecuteNonQuery();

                // Create Table Towns
                string tableTowns = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY," +
                    " CountryCode INT REFERENCES Countries(Id) NOT NULL)";
                SqlCommand tableTownsCommand = new SqlCommand(tableTowns, connection);
                tableTownsCommand.ExecuteNonQuery();

                // Create Table Minions
                string tableMinions = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50) NOT NULL," +
                    " Age INT, TownId INT REFERENCES Towns(Id) NOT NULL)";
                SqlCommand tableMinionsCommand = new SqlCommand(tableMinions, connection);
                tableMinionsCommand.ExecuteNonQuery();

                // Create Table EvilnessFactors
                string tableEvilnessFactors = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY," +
                    " Name VARCHAR(100) NOT NULL)";
                SqlCommand tableEFCommand = new SqlCommand(tableEvilnessFactors, connection);
                tableEFCommand.ExecuteNonQuery();

                // Create Table Villains
                string tableVillains = "CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY," +
                    " Name VARCHAR(100) NOT NULL, EvilnessFactorId INT REFERENCES EvilnessFactors(Id))";
                SqlCommand tableVillainsCommand = new SqlCommand(tableVillains, connection);
                tableVillainsCommand.ExecuteNonQuery();

                // Create Table MinionsVillains
                string tableMinionsVillains = "CREATE TABLE MinionsVillains(MinionId INT REFERENCES Minions(Id)," +
                    " VillainId INT REFERENCES Villains(Id), PRIMARY KEY(MinionId, VillainId))";
                SqlCommand tableMVCommand = new SqlCommand(tableMinionsVillains, connection);
                tableMVCommand.ExecuteNonQuery();

                //Insert Into Table Countries
                string insertIntoCountriesQuerryText =
                    @"INSERT INTO Countries (Name) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";
                SqlCommand insertIntoCountriesCommand = new SqlCommand(insertIntoCountriesQuerryText, connection);
                insertIntoCountriesCommand.ExecuteNonQuery();

                //Insert Into Table Towns
                string insertIntoTownsQuerryText =
                    @"INSERT INTO Towns (Name, CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";
                SqlCommand insertIntoTownsCommand = new SqlCommand(insertIntoTownsQuerryText, connection);
                insertIntoTownsCommand.ExecuteNonQuery();

                //Insert Into Table Minions
                string insertIntoMinionsQuerryText =
                    @"INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";
                SqlCommand insertIntoMinionsCommand = new SqlCommand(insertIntoMinionsQuerryText, connection);
                insertIntoMinionsCommand.ExecuteNonQuery();

                //Insert Into Table EvilnessFactors
                string insertIntoEvilnessFactorsQuerryText =
                    @"INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";
                SqlCommand insertIntoEvilnessFactorsCommand =
                    new SqlCommand(insertIntoEvilnessFactorsQuerryText, connection);
                insertIntoEvilnessFactorsCommand.ExecuteNonQuery();

                //Insert Into Table Villains
                string insertIntoVillainsQuerryText =
                    @"INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";
                SqlCommand insertIntoVillainsCommand = new SqlCommand(insertIntoVillainsQuerryText, connection);
                insertIntoVillainsCommand.ExecuteNonQuery();

                // Insert Into Table MinionsVillains
                string insertIntoMinionsVillainsQuerryText = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";
                SqlCommand insertIntoMinionsVillainsCommand =
                    new SqlCommand(insertIntoMinionsVillainsQuerryText, connection);
                insertIntoMinionsVillainsCommand.ExecuteNonQuery();
            }
        }
    }
}
