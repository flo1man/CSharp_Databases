USE [Geography]

--- Problem 12.	Countries Holding ‘A’ 3 or More Times ---
SELECT CountryName, IsoCode
	FROM Countries
	WHERE CountryName LIKE '%a%a%a%'
	ORDER BY IsoCode


--- Problem 13.Mix of Peak and River Names ---
SELECT p.PeakName, r.RiverName,
		LOWER(CONCAT(p.PeakName, SUBSTRING(r.RiverName, 2, LEN(r.RiverName)))) AS Mix
	FROM Peaks p, Rivers r
	WHERE SUBSTRING(p.PeakName, LEN(p.PeakName), 1) = LEFT(r.RiverName, 1)
	ORDER BY Mix
