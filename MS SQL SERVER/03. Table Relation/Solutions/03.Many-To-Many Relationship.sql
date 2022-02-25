CREATE DATABASE Relations
USE Relations

CREATE TABLE Students
(
	StudentID INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(20) NOT NULL
)


INSERT INTO Students
		([Name])
	VALUES
		('Mila'),
		('Toni'),
		('Ron')


CREATE TABLE Exams
(
	ExamID INT PRIMARY KEY IDENTITY(101, 1),
	[Name] NVARCHAR(50) NOT NULL
)


INSERT INTO Exams
		([Name])
	VALUES
		('SpringMVC'),
		('Neo4j'),
		('Oracle 11g')


CREATE TABLE StudentsExams
(
	 StudentID INT REFERENCES Students(StudentID),
	 ExamID INT REFERENCES Exams(ExamID),
	 CONSTRAINT PK_StudentsExams -- създаваме двоен PK, с връзка много към много
		PRIMARY KEY (StudentID, ExamID)
)

INSERT INTO StudentsExams
		(StudentID, ExamID)
	VALUES
		(1,101),
		(1,102),
		(2,101),
		(3,103),
		(2,102),
		(2,103)
