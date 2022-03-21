USE SoftUni

--- 1. Employee Address ---
SELECT TOP(5) e.EmployeeID, e.JobTitle, a.AddressID, a.AddressText
	FROM Employees e
	JOIN Addresses a ON e.AddressID = a.AddressID
	ORDER BY a.AddressID


--- 2.Addresses with Towns ---
SELECT TOP(50) e.FirstName, e.LastName, t.Name, a.AddressText
	FROM Employees e
	JOIN Addresses a ON e.AddressID = a.AddressID
	JOIN Towns t ON t.TownID = a.TownID
	ORDER BY e.FirstName, e.LastName


--- 3.Sales Employee ---
SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name
	FROM Employees e
	JOIN Departments d ON d.DepartmentID = e.DepartmentID
	WHERE d.Name = 'Sales'
	ORDER BY e.EmployeeID


--- 4.Employee Departments --- 
SELECT TOP(5) e.EmployeeID, e.FirstName, e.Salary, d.Name
	FROM Employees e
	JOIN Departments d ON d.DepartmentID = e.DepartmentID
	WHERE e.Salary > 15000
	ORDER BY e.DepartmentID


--- 5.Employees Without Project ---
SELECT TOP(3) e.EmployeeID, e.FirstName
	FROM Employees e
	LEFT JOIN EmployeesProjects ep ON e.EmployeeID = ep.EmployeeID
	WHERE ep.ProjectID IS NULL
	ORDER BY e.EmployeeID


--- 6.Employees Hired After ---
SELECT e.FirstName, e.LastName, e.HireDate, d.Name AS [DeptName]
	FROM Employees e
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
	WHERE e.HireDate > '1999/01/01' 
		AND d.Name IN('Sales', 'Finance')
	ORDER BY e.HireDate


--- 7.Employees with Project ---
SELECT TOP(5) e.EmployeeID, e.FirstName, p.Name AS [ProjectName]
	FROM Employees e
	JOIN EmployeesProjects em ON e.EmployeeID = em.EmployeeID
	JOIN Projects p ON em.ProjectID = p.ProjectID
	WHERE p.StartDate > '2002-08-12' AND p.EndDate IS NULL
	ORDER BY e.EmployeeID


--- 8.Employee 24 ---
SELECT e.EmployeeID, e.FirstName,
	 CASE
		WHEN p.StartDate >= '2005-01-01' THEN NULL
		ELSE p.Name
	 END AS [ProjectName]
	FROM Employees e
	JOIN EmployeesProjects em ON e.EmployeeID = em.EmployeeID
	JOIN Projects p ON em.ProjectID = p.ProjectID
	WHERE e.EmployeeID = 24
	 

--- 9.Employee Manager ---
SELECT e.EmployeeID, e.FirstName, e.ManagerID, em.FirstName
	FROM Employees e
	JOIN Employees em ON e.ManagerID = em.EmployeeID
	WHERE e.ManagerID IN(3,7)
	ORDER BY e.EmployeeID


--- 10.Employee Summary ---
SELECT 
	TOP(50) 
	e.EmployeeID,
	e.FirstName + ' ' + e.LastName AS [EmployeeName],
	m.FirstName + ' ' + m.LastName AS [ManagerName],
	d.Name AS [DepartmentName]
	FROM Employees e
	JOIN Employees m ON e.ManagerID = m.EmployeeID
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
	ORDER BY EmployeeID


--- 11.Min Average Salary ---
SELECT TOP(1) 
	AVG(e.Salary) AS [MinAverageSalary]
	FROM Employees e
	GROUP BY e.DepartmentID
	ORDER BY MinAverageSalary


