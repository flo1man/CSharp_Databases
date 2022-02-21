--  Problem 01.Create Database --

CREATE DATABASE Minions --създаваме база данни 'Minions'
USE Minions

--  Problem 02.Create Tables --

CREATE TABLE Minions --създаваме таблица в 'Minions'
(
	Id INT PRIMARY KEY, -- задаваме на Id, че е ключът на таблицата, по който ще се номерират данните. Винаги по default е NOT NULL.
	[Name] NVARCHAR(30) NOT NULL,
	Age TINYINT
)

CREATE TABLE Towns --създаваме таблица в 'Minions'
(
	Id INT PRIMARY KEY,
	[Name] NVARCHAR(30) NOT NULL,
)

--  Problem 03.Alter Minions Table --

ALTER TABLE Minions -- променяме структурата на таблицата
ADD TownId INT FOREIGN KEY REFERENCES Towns(Id)
-- добавяме нова колона 'ТоwnId' в таблицата Minions, която прави референция с колоната 'Id' от таблицата Towns

--  Problem 04.Insert Records into Both Tables --

INSERT INTO Towns VALUES -- добавяме данни в таблицата
(1, 'Sofia'),
(2, 'Plovid'),
(3, 'Varna')

SELECT * FROM Towns -- извличаме цялата информация от таблицата

INSERT INTO Minions VALUES -- добавяме данни в таблицата
(1,'Kevin', 22, 1),
(2, 'Bob', 15, 3),
(3, 'Steward', NULL, 2)
	
SELECT * FROM Minions -- извличаме цялата информация от таблицата

--  Problem 05.Truncate Table Minions --

TRUNCATE TABLE Minions -- изтриваме цялата информация от таблицата/ EMPTY

--  Problem 06.Drop All Tables --

DROP TABLE Minions
DROP TABLE Towns
-- изтриваме таблицата / DELETE