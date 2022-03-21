USE SoftUni

--- 08.Employees with Three Projects ---
CREATE PROC usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
BEGIN TRANSACTION
	DECLARE @countOfProjects INT =
	(SELECT COUNT(*) FROM Employees e 
	JOIN EmployeesProjects ep ON ep.EmployeeID = e.EmployeeID
	WHERE e.EmployeeID = @emloyeeId)

	IF (@countOfProjects >= 3)
		BEGIN
			ROLLBACK
			RAISERROR('The employee has too many projects!',16 ,1)
			RETURN
		END

	INSERT INTO EmployeesProjects 
		(EmployeeID, ProjectID) 
			VALUES
		(@emloyeeId, @projectID)
	COMMIT

--- 09.Delete Employees ---
CREATE TABLE Deleted_Employees
(
	EmployeeId INT PRIMARY KEY NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	MiddleName VARCHAR(50) NOT NULL,
	JobTitle VARCHAR(50) NOT NULL,
	DepartmentId INT REFERENCES Departments(DepartmentId) NOT NULL,
	Salary MONEY NOT NULL,
)

CREATE TRIGGER tr_AddDeleterEmployeesToTable
ON Employees FOR DELETE
AS
	INSERT INTO Deleted_Employees
	(FirstName,LastName,MiddleName,JobTitle,DepartmentId,Salary)
	SELECT FirstName,LastName,MiddleName,JobTitle,DepartmentID,Salary
		FROM deleted