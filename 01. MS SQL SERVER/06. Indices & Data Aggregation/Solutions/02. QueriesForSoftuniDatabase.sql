USE SoftUni

--- 13.Departments Total Salaries ---
SELECT DepartmentID, SUM(Salary) AS [TotalSalary]
	FROM Employees
	GROUP BY DepartmentID


--- 14.Employees Minimum Salaries ---
SELECT DepartmentID, MIN(Salary) AS [MinimumSalary]
	FROM Employees
	WHERE DepartmentID IN(2,5,7) AND HireDate > '2000-01-01'
	GROUP BY DepartmentID


--- 15.Employees Average Salaries ---
SELECT * INTO [NewTable]
	FROM Employees
	WHERE Salary > 30000


DELETE
	FROM NewTable
	WHERE ManagerID = 42


UPDATE NewTable
	SET Salary += 5000
	WHERE DepartmentID = 1


SELECT DepartmentID, AVG(Salary) AS [AverageSalary]
	FROM NewTable
	GROUP BY DepartmentID


--- 16. Employees Maximum Salaries
SELECT DepartmentID, MAX(Salary)
	FROM Employees
	GROUP BY DepartmentID
	HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000


--- 17.Employees Count Salaries ---
SELECT COUNT(*)
	FROM Employees
	WHERE ManagerID IS NULL


--- 18.*3rd Highest Salary ---
SELECT 
	DepartmentID,
	Salary AS ThirdHighestSalary
	FROM (SELECT 
		DepartmentID,
		Salary,
		DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS [Rank]
		FROM Employees
		GROUP BY DepartmentID, Salary
        ) AS RankSalariesInDepartments
	WHERE [Rank] = 3


--- 19.**Salary Challenge ---
SELECT TOP(10)
	e.FirstName,
	e.LastName,
	e.DepartmentID
	FROM Employees as e
	WHERE e.Salary > 
		(SELECT 
		AVG(emp.Salary)
		FROM Employees AS emp
		GROUP BY emp.DepartmentID
		HAVING e.DepartmentID = emp.DepartmentID)
	ORDER BY e.DepartmentID
