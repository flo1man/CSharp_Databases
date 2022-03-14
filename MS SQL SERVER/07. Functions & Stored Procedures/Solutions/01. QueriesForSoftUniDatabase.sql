USE SoftUni

--- 1.Employees with Salary Above 35000 ---
CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
BEGIN
	SELECT FirstName, LastName
		FROM Employees
		WHERE Salary > 35000
END

--EXEC usp_GetEmployeesSalaryAbove35000


--- 2.Employees with Salary Above Number ---
CREATE PROC usp_GetEmployeesSalaryAboveNumber (@Number DECIMAL(18,4))
AS
BEGIN
	SELECT FirstName, LastName
		FROM Employees
		WHERE Salary >= @Number
END

--EXEC usp_GetEmployeesSalaryAboveNumber 48100


--- 3.Town Names Starting With --
CREATE PROC usp_GetTownsStartingWith(@StartString NVARCHAR(MAX))
AS
BEGIN
	SELECT [Name]
		FROM Towns
		WHERE LEFT([Name], LEN(@StartString)) = @StartString
END

--EXEC usp_GetTownsStartingWith 'b'


--- 4.Employees from Town ---
CREATE PROC usp_GetEmployeesFromTown(@TownName NVARCHAR(MAX))
AS
BEGIN
	SELECT FirstName, LastName
		FROM Employees e
		JOIN Addresses a ON e.AddressID = a.AddressID
		JOIN Towns t ON a.TownID = t.TownID
		WHERE t.[Name] = @TownName
END

--EXEC usp_GetEmployeesFromTown 'Sofia'


--- 5.Salary Level Function ---
CREATE FUNCTION ufn_GetSalaryLevel(@Salary DECIMAL(18,4))
RETURNS VARCHAR(20)
AS
BEGIN
	DECLARE @Level VARCHAR(20)
	IF (@Salary < 30000)
		SET @Level = 'Low'
	ELSE IF (@Salary BETWEEN 30000 AND 50000)
		SET @Level = 'Average'
	ELSE
		SET @Level = 'High'

	RETURN @Level;
END

--SELECT Salary, dbo.ufn_GetSalaryLevel(Salary) AS [Salary Level]
	--FROM Employees


--- 6.Employees by Salary Level ---
CREATE PROC usp_EmployeesBySalaryLevel(@LevelOfSalary VARCHAR(15))
AS
BEGIN
	SELECT FirstName, LastName
		FROM Employees
		WHERE dbo.ufn_GetSalaryLevel(Salary) = @LevelOfSalary
END

--EXEC usp_EmployeesBySalaryLevel 'High'


--- 7.Define Function ---
CREATE FUNCTION ufn_IsWordComprised
		(@Letters VARCHAR(MAX), @Words VARCHAR(MAX))
RETURNS INT
AS
BEGIN
	DECLARE @i INT = 0;
	WHILE (@i < LEN(@Words))
		BEGIN
			IF(CHARINDEX(SUBSTRING(@Words,@i + 1,1), @Letters) = 0)
				RETURN 0;
			SET @i += 1;
		END
	RETURN 1;
END

--SELECT dbo.ufn_Contains('oistmiahf','Sofia')


--- 8.* Delete Employees and Departments ---
CREATE PROCEDURE usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
	-- 01. Delete employees from EmployeesProjects
	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN (
	                     SELECT EmployeeID FROM Employees
	                     WHERE DepartmentID = @departmentId
						)
		            
	-- 02. Set ManagerId to NULL in Employees
	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN (
	                     SELECT EmployeeID FROM Employees
	                     WHERE DepartmentID = @departmentId
						)

	-- 03. Alter column ManagerID in Departments and make it NULLable
	ALTER TABLE Departments
	ALTER COLUMN ManagerID INT

	-- 04. Set ManagerId to NULL in Departments
	UPDATE Departments
	SET ManagerID = NULL
	WHERE ManagerID IN (
	                     SELECT EmployeeID FROM Employees
	                     WHERE DepartmentID = @departmentId
						)

   -- 05. Delete all employees from current department
   DELETE FROM Employees
   WHERE DepartmentID = @departmentId

   -- 06. Delete current department
   DELETE FROM Departments
   WHERE DepartmentID = @departmentId

   -- 07. Return 0 count If DELETE was succesfull
   SELECT 
		COUNT(*) 
		FROM Employees 
		WHERE DepartmentID = @departmentId


-- EXECUTE dbo.usp_DeleteEmployeesFromDepartment 16