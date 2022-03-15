USE [Service]


--- 2.Insert ---
INSERT INTO Employees
	(FirstName,LastName,Birthdate,DepartmentId) 
		VALUES
	('Marlo', 'O''Malley','1958-9-21',1),
	('Niki', 'Stanaghan','1969-11-26',4),
	('Ayrton', 'Senna','1960-03-21',9),
	('Ronnie', 'Peterson','1944-02-14',9),
	('Giovanna', 'Amati','1959-07-20',5)


INSERT INTO Reports
(CategoryId,StatusId,OpenDate,CloseDate,[Description],UserId,EmployeeId)
	VALUES
(1,1,'2017-04-13',NULL,'Stuck Road on Str.133',6,2),
(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
(14,2,'2015-09-07',NULL,'Falling bricks on Str.58',5,2),
(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)


--- 3.Update ---
UPDATE Reports
	SET CloseDate = GETDATE()
	WHERE CloseDate IS NULL


--- 4.Delete ---
DELETE FROM Reports
	WHERE StatusId = 4


--- 5.Unassigned Reports ---
SELECT [Description], FORMAT(OpenDate, 'dd-MM-yyyy')
	FROM Reports
	WHERE EmployeeId IS NULL
	ORDER BY OpenDate, [Description]


--- 6.Reports and Categories ---
SELECT [Description], [Name] AS [CategoryName]
	FROM Reports r
	JOIN Categories c ON r.CategoryId = c.Id
	ORDER BY [Description], [Name]

	
--- 7.Most Reported Category ---
SELECT TOP(5) [Name] AS [CategoryName], COUNT(r.CategoryId) AS [ReportsNumber]
	FROM Reports r
	JOIN Categories c ON r.CategoryId = c.Id
	GROUP BY r.CategoryId, [Name]
	ORDER BY [ReportsNumber] DESC, [Name] 


--- 8.Birthday Report ---
SELECT Username, c.[Name] AS [CategoryName]
	FROM Users u
	JOIN Reports r ON r.UserId = u.Id
	JOIN Categories c ON r.CategoryId = c.Id
	WHERE DATEPART(MONTH, r.OpenDate) = DATEPART(MONTH, u.Birthdate) AND
		  DATEPART(DAY, r.OpenDate) = DATEPART(DAY, u.Birthdate)
	ORDER BY Username,[CategoryName]


--- 9.Users per Employee ---
SELECT CONCAT(e.FirstName, ' ', e.LastName) AS [FullName], COUNT(DISTINCT r.UserId) AS [UsersCount]
	FROM Employees e
	LEFT JOIN Reports r 
	ON r.EmployeeId = e.Id
	GROUP BY CONCAT(e.FirstName, ' ', e.LastName)
	ORDER BY UsersCount DESC, FullName ASC


--- 10.Full info ---
SELECT DISTINCT
	  CASE
		WHEN em.FirstName IS NULL OR em.LastName IS NULL THEN 'None'
		ELSE em.FirstName + ' ' + em.LastName
	  END AS [Employee],
	  ISNULL(d.[Name], 'None') AS [Department],
	  ISNULL(c.[Name], 'None') AS [Category],
	  r.[Description] AS [Description],
	  FORMAT(r.OpenDate, 'dd.MM.yyyy') AS [OpenDate],
	  s.[Label] AS [Status],
	  u.[Name] AS [User]
    FROM Employees em
    JOIN Departments d ON d.Id = em.DepartmentId
    JOIN Categories c ON c.DepartmentId = d.Id
    JOIN Reports r ON r.EmployeeId = em.Id
    JOIN [Status] s ON r.StatusId = s.Id
    JOIN Users u ON r.UserId = u.Id
 ORDER BY [Employee] DESC, [Department], [Category], [Description],
	[OpenDate], [Status], [User]


--- 11.Hours to Complete ---
CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @result INT;
	IF (@StartDate IS NULL OR @EndDate IS NULL)
		SET @result = 0;
	ELSE
		SET @result = DATEDIFF(HOUR,@StartDate,@EndDate)

	RETURN @result;
END


--- 12.Assign Employee ---
CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
	BEGIN TRANSACTION
		DECLARE @EmpDepart INT = (
		SELECT DepartmentId FROM Employees WHERE Id = @EmployeeId)
		DECLARE @CategId INT = (
		SELECT CategoryId FROM Reports WHERE Id = @ReportId)
		DECLARE @ReportDepart INT = (
		SELECT DepartmentId FROM Categories WHERE Id = @CategId)
			IF (@EmpDepart <> @ReportDepart)
			BEGIN
				ROLLBACK;
				THROW 50001, 'Employee doesn''t belong to the appropriate department!', 1
			END

			UPDATE Reports
			SET EmployeeId = @EmployeeId
			WHERE Id = @ReportId
	COMMIT
END			

--EXEC usp_AssignEmployeeToReport 30, 1
--EXEC usp_AssignEmployeeToReport 17, 2