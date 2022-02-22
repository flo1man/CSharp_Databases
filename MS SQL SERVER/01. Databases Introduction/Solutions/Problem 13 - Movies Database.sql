--  Problem 13.Movies Database --

CREATE DATABASE Movies
USE Movies

CREATE TABLE Directors
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	DirectorName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(500)
)

INSERT INTO Directors (DirectorName, Notes)
	VALUES
		('Ivan', 'Some notes'),
		('Kiril', 'Some notes'),
		('Vasil', 'Some notes'),
		('Anton', 'Some notes'),
		('Asen', 'Some notes')

CREATE TABLE Genres
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	GenreName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(500)
)

INSERT INTO Genres (GenreName, Notes)
	VALUES
		('Action', 'Some notes'),
		('Comedy', 'Some notes'),
		('Drama', 'Some notes'),
		('Sci-fi', 'Some notes'),
		('Fantasy', 'Some notes')

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CategotyName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(500)
)

INSERT INTO Categories (CategotyName, Notes)
	VALUES
		('Category1', 'Some notes'),
		('Category2', 'Some notes'),
		('Category3', 'Some notes'),
		('Category4', 'Some notes'),
		('Category5', 'Some notes')


CREATE TABLE Movies
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Title NVARCHAR(100) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL, -- правим връзка/референция между колоната и таблицата
	CopyrightYear DATE,
	[Length] INT,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL, -- правим връзка/референция между колоната и таблицата
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL, -- правим връзка/референция между колоната и таблицата
	Rating INT,
	Notes NVARCHAR(500)
)


INSERT INTO Movies
(Title, DirectorId, [Length], GenreId, CategoryId, Rating, Notes)
	VALUES
		('Harry Potter',1, 120, 1, 1, 6, 'Good movies'),
		('Harry Potter II',2, 120, 2, 2, 6, 'Good movies'),
		('World Z War',3, 140, 3, 3, 9, 'Good movies'),
		('Taxi 2',4, 120, 4, 4, 4, 'Good movies'),
		('Taxi 3',5, 120, 5, 5, 7, 'Good movies')

SELECT * FROM Movies