USE Diablo

--- Problem 14.Games from 2011 and 2012 year ---
SELECT TOP(50) [Name]
	,FORMAT([Start], 'yyyy-MM-dd') AS [Start]
	FROM Games
	WHERE DATEPART(YEAR, [Start]) 
		BETWEEN 2011 AND 2012
	ORDER BY [Start], [Name]


--- Problem 15.	User Email Providers ---
SELECT [Username],
			SUBSTRING(Email, 
			 CHARINDEX('@', Email) + 1,
			  LEN(Email) - CHARINDEX('@', Email)) 
			   AS [Email Provider]
	FROM Users
	ORDER BY [Email Provider], [Username]


--- Problem 16.Get Users with IPAdress Like Pattern ---
SELECT Username, IpAddress
	FROM Users
	WHERE IpAddress LIKE '___.1_%._%.___'
	ORDER BY Username

-- Problem 17.Show All Games with Duration and Part of the Day --
SELECT [Name],
		CASE 
			WHEN DATEPART(HOUR, [Start]) >= 0
			AND DATEPART(HOUR, [Start]) < 12 THEN 'Morning'
			WHEN DATEPART(HOUR, [Start]) >= 12
			AND DATEPART(HOUR, [Start]) < 18 THEN 'Afternoon'
			ELSE 'Evening'
		END AS [Part of the Day],
		CASE 
			WHEN Duration <= 3 THEN 'Extra Short'
			WHEN Duration > 4 AND Duration <= 6 THEN 'Short'
			WHEN Duration > 6 THEN 'Long'
			ELSE 'Extra Long'
		END AS [Duration]
	FROM Games
	ORDER BY [Name], [Duration], [Part of the Day]