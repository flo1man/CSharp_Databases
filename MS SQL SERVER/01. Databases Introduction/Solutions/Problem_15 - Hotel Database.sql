-- Problem 15.Hotel Database --

CREATE DATABASE Hotel
USE Hotel

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Title NVARCHAR(100) NOT NULL,
	Notes NVARCHAR(300)
)

INSERT INTO Employees
		(FirstName, LastName, Title)
	VALUES
		('Ivan', 'Kirilov', 'Manager'),
		('Mitko', 'Asenov', 'CEO'),
		('Dian', 'Kostadinov', 'CTO')

CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	PhoneNumber INT NOT NULL,
	EmergencyName NVARCHAR(30),
	EmergencyNumber INT,
	Notes NVARCHAR(300)
)

INSERT INTO Customers
		(FirstName, LastName, PhoneNumber)
	VALUES
		('Jivko', 'Petrov', 08712112),
		('Danail', 'Dimitrov', 089255182),
		('Zahari', 'Sheitanov', 089424623)


CREATE TABLE RoomStatus
(
	RoomStatus NCHAR(1) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(300)
)

INSERT INTO RoomStatus
		(RoomStatus)
	VALUES
		('B'),
		('C'),
		('G')


CREATE TABLE RoomTypes
(
	RoomType VARCHAR(10) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(300)
)

INSERT INTO RoomTypes
		(RoomType)
	VALUES
		('Single'),
		('Double'),
		('Family')


CREATE TABLE BedTypes
(
	BedType VARCHAR(20) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(300)
)

INSERT INTO BedTypes
		(BedType)
	VALUES
		('Small'),
		('Large'),
		('Extra Large')


CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY IDENTITY NOT NULL,
	RoomType VARCHAR(10) FOREIGN KEY REFERENCES RoomTypes(RoomType) NOT NULL,
	BedType VARCHAR(20) FOREIGN KEY REFERENCES BedTypes(BedType) NOT NULL,
	Rate INT NOT NULL,
	RoomStatus NCHAR(1) FOREIGN KEY REFERENCES RoomStatus(RoomStatus) NOT NULL,
	Notes NVARCHAR(300)
)

INSERT INTO Rooms
		(RoomType,BedType,Rate,RoomStatus)
	VALUES
		('Single', 'Large', 9, 'B'),
		('Double', 'Small', 8, 'C'),
		('Family', 'Extra Large', 6, 'G')


CREATE TABLE Payments
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	PaymentDate DATE NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	FirstDateOccupied DATE,
	LastDateOccupued DATE,
	TotalDays INT NOT NULL,
	AmountCharged DECIMAL(7,2) NOT NULL,
	TaxRate DECIMAL(4,2),
	TaxAmount DECIMAL(7,2) NOT NULL,
	PaymentTotal DECIMAL(7,2) NOT NULL,
	Notes NVARCHAR(300)
)


INSERT INTO Payments
		(EmployeeId,PaymentDate,AccountNumber,TotalDays,AmountCharged,TaxAmount,PaymentTotal)
	VALUES
		(1,'03.04.2021', 1, 7, 500, 50, 550),
		(2,'07.07.2019', 2, 7, 600, 50, 650),
		(3,'08.04.2020', 3, 7, 700, 50, 750)


CREATE TABLE Occupancies
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	DateOccupied DATE NOT NULL,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber) NOT NULL,
	RateApplied DECIMAL(2,1) NOT NULL,
	PhoneCharge BIT,
	Notes NVARCHAR(300)
)

INSERT INTO Occupancies
		(EmployeeId,DateOccupied,AccountNumber,RoomNumber,RateApplied)
	VALUES
		(1, '03.04.2022', 1, 1, 9.9),
		(2, '07.07.2021', 2, 3, 7.9),
		(3, '01.06.2020', 2, 3, 8.9)