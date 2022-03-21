USE SoftUni

-- 2.Find All Information about Departments-- 
SELECT * FROM Departments


-- 3.Find all Department Names --
SELECT [Name] FROM Departments


-- 4.Find Salary of Each Employee --
SELECT [FirstName],[LastName],[Salary] FROM Employees


-- 5.Find Full Name of Each Employee -- 
SELECT [FirstName],[MiddleName],[LastName] FROM Employees


-- 6.Find Email Address of Each Employee --
SELECT [FirstName] + '.' + [LastName] + '@softuni.bg'
AS [Full Email Address]
FROM Employees
/*
SELECT CONCAT(FirstName,'.', LastName, '@softuni.bg')
AS [Full Email Address] 
FROM Employees
*/


-- 7.Find All Different Employee’s Salaries --
SELECT DISTINCT Salary FROM Employees  -- дава всички различни редове/ премахва повтарящи се колони


-- 8.Find all Information about Employees --
SELECT * FROM Employees WHERE [JobTitle] = 'Sales Representative'


-- 9.Find Names of All Employees by Salary in Range --
SELECT [FirstName], [LastName], [JobTitle] 
FROM Employees
WHERE [Salary] >= 20000 AND [Salary] <= 30000
    --[Salary] BETWEEN 20000 AND 30000


-- 10.Find Names of All Employees  --
SELECT [FirstName] + ' ' + [MiddleName] + ' ' + [LastName] AS 'Full Name'
FROM Employees
WHERE [Salary] IN (25000, 14000, 12500, 23600) -- взимаме всички колни с желаните заплати
	/*[Salary] = 25000 OR
	  [Salary] = 14000 OR
	  [Salary] = 12500 OR
	  [Salary] = 23600 OR
	*/


-- 11.Find All Employees Without Manager --
SELECT [FirstName], [LastName]
FROM Employees
WHERE [ManagerID] IS NULL  -- когато искаме да сравним с NULL използваме ключовата думичка 'IS'


-- 12.Find All Employees with Salary More Than 50000 --
SELECT [FirstName], [LastName], [Salary]
FROM Employees
WHERE [Salary] > 50000
ORDER BY [Salary] DESC


-- 13.Find 5 Best Paid Employees --
SELECT TOP(5) [FirstName], [LastName] -- TOP(N) - взимаме първите N записа
FROM Employees
ORDER BY [Salary] DESC


-- CREATE VIEW --
CREATE VIEW v_UserWithHighestSalary AS
SELECT [FirstName], [LastName], [Salary] FROM Employees
WHERE [Salary] >= 40000

-- SELECT VIEW --
SELECT * FROM v_UserWithHighestSalary


-- 14.Find All Employees Except Marketing --
SELECT [FirstName], [LastName] FROM Employees
WHERE DepartmentId != 4



USE SoftUni

-- 2.Find All Information about Departments-- 
SELECT * 
	FROM Departments


-- 3.Find all Department Names --
SELECT [Name] 
	FROM Departments


-- 4.Find Salary of Each Employee --
SELECT [FirstName], [LastName], [Salary]
	FROM Employees

-- 5.Find Full Name of Each Employee -- 
SELECT [FirstName], [MiddleName], [LastName]
FROM Employees


-- 6.Find Email Address of Each Employee --
SELECT [FirstName] + '.' + [LastName] + '@softuni.bg' AS [Full Email Address]
	FROM Employees
--SELECT CONCAT(FirstName, '.', LastName, '@', 'softuni.bg') FROM Employees--


-- 7.Find All Different Employee’s Salaries --
SELECT DISTINCT [Salary] 
	FROM Employees


-- 8.Find all Information about Employees --
SELECT * 
	FROM Employees
	WHERE [JobTitle] = 'Sales Representative'


-- 9.Find Names of All Employees by Salary in Range --
SELECT [FirstName], [LastName], [JobTitle]
	FROM Employees
	WHERE [Salary] >= 20000 AND [Salary] <= 30000


-- 10.Find Names of All Employees  --
SELECT CONCAT([FirstName], ' ', [MiddleName], ' ', [LastName]) AS [Full Name]
	FROM Employees
	WHERE [Salary] IN (25000, 14000, 12500, 23600)

--SELECT CONCAT([FirstName], ' ', [MiddleName] + ' ', [LastName]) AS [Full Name]
--FROM Employees
--WHERE [Salary] IN (25000, 14000, 12500, 23600)
    -- OR --
--SELECT CONCAT(FirstName, ' ', ISNULL(MiddleName,''), ' ', LastName) AS [Full Name]  FROM Employees
--   WHERE Salary IN (25000, 14000, 12500, 23600)


-- 11.Find All Employees Without Manager --
SELECT [FirstName], [LastName]
	FROM Employees
	WHERE [ManagerID] IS NULL
	
	
-- 12.Find All Employees with Salary More Than 50000 --
SELECT [FirstName], [LastName], [Salary]
	FROM Employees
	WHERE [Salary] > 50000
	ORDER BY [Salary] DESC


-- 13.Find 5 Best Paid Employees --
SELECT TOP(5) [FirstName], [LastName]
	FROM Employees
	ORDER BY [Salary] DESC

SELECT [FirstName], [LastName]
	FROM Employees
	ORDER BY [Salary] DESC
	OFFSET 0 ROWS
	FETCH NEXT 5 ROWS ONLY


-- 14.Find All Employees Except Marketing --
SELECT [FirstName], [LastName]
	FROM Employees
	WHERE [DepartmentID] != 4 


-- 15.Sort Employees Table --
SELECT * FROM Employees
ORDER BY Salary DESC,
		 FirstName ASC,
		 LastName DESC,
		 MiddleName ASC


-- 16. Create View Employees with Salaries --
CREATE VIEW V_EmployeesSalaries AS
SELECT [FirstName], [LastName], [Salary] 
FROM Employees

SELECT * FROM V_EmployeesSalaries


-- 17.Create View Employees with Job Titles --
CREATE VIEW V_EmployeeNameJobTitle AS
	SELECT CONCAT([FirstName], ' ', 
	ISNULL([MiddleName] ,''), ' ', [LastName]) -- ISNULL - ако е празен string записваме empty space ('')
	AS [Full Name], [JobTitle]
	FROM Employees


-- 18.Distinct Job Titles --
SELECT DISTINCT JobTitle FROM Employees


-- 19.Find First 10 Started Projects --
SELECT TOP(10) * FROM Projects
ORDER BY [StartDate], [Name]


-- 20.Last 7 Hired Employees --
SELECT TOP(7) [FirstName],[LastName],[HireDate] 
FROM Employees
ORDER BY [HireDate] DESC


-- 21.Increase Salaries --
UPDATE Employees
SET Salary *= 1.12
WHERE DepartmentId IN (1, 2, 4, 11)

SELECT [Salary] 
	FROM Employees

