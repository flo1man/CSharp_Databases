CREATE DATABASE Relations
USE Relations

CREATE TABLE Passports
(
	PassportID INT PRIMARY KEY IDENTITY(101,1),
	PassportNumber NVARCHAR(20) NOT NULL
)


INSERT INTO Passports
		(PassportNumber)
	VALUES
		('N34FG21B'),
		('K65LO4R7'),
		('ZE657QP2')


CREATE TABLE Persons
(
	PersonID INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(20) NOT NULL,
	Salary DECIMAL(7,2) NOT NULL,
	PassportID INT REFERENCES Passports(PassportID) UNIQUE NOT NULL
)


INSERT INTO Persons
		(FirstName,Salary,PassportID)
	VALUES
		('Roberto', 43300, 102),
		('Tom', 56100, 103),
		('Yana', 60200, 101)