-- Problem 16. Create SoftUni Database --

CREATE DATABASE SoftUni
USE SoftUni

CREATE TABLE Towns
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Name NVARCHAR(50) NOT NULL
)

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	AddressText NVARCHAR(300) NOT NULL,
	TownId INT FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] NVARCHAR(50) NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(30) NOT NULL,
	MiddleName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	JobTitle NVARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id),
	HireDate DATE NOT NULL, -- !!!!
	Salary DECIMAL(7,2) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
)

-- Problem 18.Basic Insert --

INSERT INTO Towns
		(Name)
	VALUES
		('Sofia'),
		('Plovdiv'),
		('Varna'),
		('Burgas')


INSERT INTO Departments
		(Name)
	VALUES
		('Engineering'),
		('Sales'),
		('Marketing'),
		('Software Development'),
		('Quality Assurance')


INSERT INTO Employees
		(FirstName,MiddleName,LastName,JobTitle,DepartmentId,HireDate,Salary)
	VALUES
		('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '02/01/2013', 3500.00),
		('Petar', 'Petrov', 'Petrov', 'Senior Engineer', 1, '03/02/2004', 4000.00),
		('Maria', 'Petrova', 'Ivanova', 'Intern', 5, '08/28/2016', 525.25),
		('Georgi', 'Teziev', 'Ivanov', 'CEO', 2, '12/09/2007', 3000.00),
		('Peter', 'Pan', 'Pan', 'Intern', 3, '08/28/2016', 599.88)


-- Problem 19.Basic Select All Fields --

SELECT * FROM Towns
SELECT * FROM Departments
SELECT * FROM Employees


-- Problem 20.Basic Select All Fields and Order Them --

SELECT * FROM Towns ORDER BY [Name] ASC -- сортиране във възходящ ред
SELECT * FROM Departments ORDER BY [Name] ASC -- сортиране във възходящ ред
SELECT * FROM Employees ORDER BY [Salary] DESC -- сортиране във низходящ ред


-- Problem 21.Basic Select Some Fields 

SELECT [Name] FROM Towns ORDER BY [Name] ASC
SELECT [Name] FROM Departments ORDER BY [Name] ASC
SELECT [FirstName],[LastName],[JobTitle],[Salary] FROM Employees ORDER BY [Salary] DESC


-- Problem 22.Increase Employees Salary --

UPDATE Employees SET Salary *= 1.10 -- презаписваме/ъпдейтваме 'Salary', като всеки един запис го увеличаваме с 10%
SELECT [Salary] FROM Employees


-- Problem 23.Decrease Tax Rate --

USE Hotel
UPDATE Payments SET TaxRate *= 0.97 -- презаписваме/ъпдейтваме 'TaxRate', като всеки един запис го намаляме с 3%
SELECT [TaxRate] FROM Payments


-- Problem 24.Delete All Records --

USE Hotel
TRUNCATE TABLE Occupancies
