--  Problem 07. Create Table People--

USE Minions

CREATE TABLE People
(
	Id INT PRIMARY KEY IDENTITY NOT NULL, --'IDENTITY' = Auto Incrementing
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY(MAX),
	Height DECIMAL(3,2),
	[Weight] DECIMAL(3,2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People([Name],Height,[Weight],Gender,Birthdate,Biography)
	VALUES
		('Pesho',1.50, 80.00, 'm', '05.03.2000', 'Biography for person'),
		('Ivan',1.70, 70.60, 'm', '07.03.1999', 'Biography for person'),
		('Maria',1.80, 67.50, 'f', '01.03.2003', 'Biography for person'),
		('Petar',1.90, 87.10, 'm', '02.03.2006', 'Biography for person'),
		('Kiril',1.78, 89.30, 'm', '04.03.2001', 'Biography for person')

--Problem 8.Create Table Users --

CREATE TABLE Users
(
	Id BIGINT PRIMARY KEY IDENTITY NOT NULL,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX)
	CHECK(DATALENGTH(ProfilePicture) <= 900 * 1024), -- проверява дали е по-малко или равно от 900KB
	LastLoginTime DATETIME2,
	IsDeleted BIT NOT NULL
)


INSERT INTO Users(Username,[Password],LastLoginTime,IsDeleted)
	VALUES
		('Ivan1', '123456','01.01.2021', 0),
		('Kiril1', '1239456','03.03.2022', 0),
		('Megan1', '1235456','02.02.2021', 1),
		('Anton1', '12324456','02.04.2011', 0),
		('Asen1', '12456','07.07.2022', 1)

				--DELETE FROM Users WHERE Id = 5 ==> Изтриваме ред с Id 5
				--SET IDENTITY_INSERT Users ON ==> Включваме ръчно добавяне
				--INSERT INTO Users(Id, Username,[Password],LastLoginTime,IsDeleted)
				--	VALUES
				--			(5, 'Pesho55',123456,'05.21.2020',0) ==> Добавяме реда, като му задаваме изрично Id
				--SET IDENTITY_INSERT Users OFF ==> Изключваме ръчно добавяне

--Problem 9.Change Primary Key --

ALTER TABLE Users -- разревашаме промяна в таблицата
DROP CONSTRAINT PK__Users__3214EC07BE785D08 -- премахваме primary key-a от Id колоната

ALTER TABLE Users -- разревашаме промяна в таблицата
ADD CONSTRAINT PK_Users_IdUsername
PRIMARY KEY (Id, Username) -- добавя на Primary Key за две полета

-- Problem 10.Add Check Constraint --

ALTER TABLE Users -- разрешаваме промяна в таблицата
ADD CONSTRAINT CK_Users_PasswordLenght -- добавяме ограничение
CHECK(LEN([Password]) >= 5) -- за дължина на паролата

 -- Problem 11.Set Default Value of a Field --

ALTER TABLE Users
ADD CONSTRAINT DF_Users_LastLoginTime
DEFAULT GETDATE() FOR LastLoginTime

 -- Problem 12.Set Unique Field -- 

 ALTER TABLE Users
 DROP CONSTRAINT PK_Users_IdUsername

 ALTER TABLE Users
 ADD CONSTRAINT [PK_Users_Id]
 PRIMARY KEY (Id)

 ALTER TABLE Users
 ADD CONSTRAINT CK_Users_UsernameLenght
 CHECK(LEN(Username) >= 3)