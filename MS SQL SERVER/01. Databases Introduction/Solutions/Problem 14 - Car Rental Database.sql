-- Problem 14.Car Rental Database --

CREATE DATABASE CarRental
USE CarRental

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate INT,
	WeeklyRate INT,
	MonthlyRate INT NOT NULL,
	WeekendRate INT
)

INSERT INTO Categories 
		(CategoryName, MonthlyRate)
	VALUES
		('Category1', 9),
		('Category2', 8),
		('Category3', 7)

CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	PlateNumber NVARCHAR(15) NOT NULL,
	Manufacturer NVARCHAR(30) NOT NULL,
	Model NVARCHAR(30) NOT NULL,
	CarYear INT,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Doors INT,
	Picture VARBINARY(MAX),
	Conditional INT,
	Available BIT NOT NULL
)

INSERT INTO Cars
		(PlateNumber, Manufacturer, Model, CategoryId, Available)
	VALUES
		('CO5312XX', 'BMW', 'M3', 1, 1),
		('CA6666AB', 'Audi', 'A5', 2, 1),
		('X2022AK', 'BMW', 'M5', 3, 0)


CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Title NVARCHAR(100),
	Notes NVARCHAR(300)
)

INSERT INTO Employees
		(FirstName, LastName)
	VALUES
		('Ivan', 'Kirilov'),
		('Mitko', 'Asenov'),
		('Kiril', 'Petkov')


CREATE TABLE Customers
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	DriverLicenceNumber INT NOT NULL,
	FullName NVARCHAR(60) NOT NULL,
	[Address] NVARCHAR(150),
	City NVARCHAR(60) NOT NULL,
	ZIPCode INT,
	Notes NVARCHAR(300)
)

INSERT INTO Customers
		(DriverLicenceNumber, FullName, City)
	VALUES
		(123456, 'Jivko Ivanov', 'Sofia'),
		(654321, 'Zahari Kostov', 'Plovdiv'),
		(678912, 'Georgi Angelov', 'Varna')


CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	TankLevel INT,
	KilometrageStart INT,
	KilometrageEnd INT,
	TotalKilometrage INT NOT NULL,
	StartDate DATE,
	EndDate DATE,
	TotalDays INT NOT NULL,
	RateApplied INT,
	TaxRate INT,
	OrderStatus BIT NOT NULL,
	Notes NVARCHAR(300)
)

INSERT INTO RentalOrders
		(EmployeeId,CustomerId,CarId,TotalKilometrage,TotalDays, OrderStatus)
	VALUES
		(1,2,3,10000, 90, 1),
		(2,2,3,25000, 60, 0),
		(3,2,1,5800, 30, 1)

SELECT * FROM RentalOrders