USE Geography

--- 12.Highest Peaks in Bulgaria ---
SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation
	FROM MountainsCountries mc
	JOIN Mountains m ON mc.MountainId = m.Id
	JOIN Peaks p ON m.Id = p.MountainId
	WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
	ORDER BY p.Elevation DESC


--- 13.Count Mountain Ranges ---
SELECT mc.CountryCode, COUNT(m.MountainRange)
	FROM MountainsCountries mc
	JOIN Mountains m ON mc.MountainId = m.Id
	WHERE mc.CountryCode IN('US', 'RU', 'BG')
	GROUP BY CountryCode


--- 14.Countries with Rivers ---
SELECT TOP(5) c.CountryName, r.RiverName
	FROM Countries c
	LEFT JOIN CountriesRivers cr ON c.CountryCode = cr.CountryCode
	LEFT JOIN Rivers r ON cr.RiverId = r.Id
	WHERE c.ContinentCode = 'AF' 
	ORDER BY c.CountryName


--- 15.*Continents and Currencies ---
SELECT ContinentCode, CurrencyCode, Total AS [CurrencyUsage]
	FROM
(
SELECT ContinentCode, CurrencyCode, COUNT(CurrencyCode) AS Total,
	DENSE_RANK() OVER(PARTITION BY ContinentCode ORDER BY COUNT(CurrencyCode) DESC) AS Ranked
	FROM Countries 
	GROUP BY ContinentCode, CurrencyCode
) AS Sub
WHERE Ranked = 1 AND Total > 1
ORDER BY ContinentCode


--- 16.Countries without Any Mountains ---
SELECT COUNT(*)
	FROM Countries c
	LEFT JOIN MountainsCountries mc ON mc.CountryCode = c.CountryCode
	WHERE mc.MountainId IS NULL


--- 17.Highest Peak and Longest River by Country ---
SELECT TOP(5) c.CountryName,
	MAX(p.Elevation) AS [HighestPeak],
	MAX(r.Length) AS [LongestRiver]
		FROM Countries c
		LEFT JOIN MountainsCountries mc ON c.CountryCode = mc.CountryCode
		LEFT JOIN Mountains m ON mc.MountainId = m.Id
		LEFT JOIN Peaks p ON m.Id = p.MountainId
		LEFT JOIN CountriesRivers cr ON c.CountryCode = cr.CountryCode
		LEFT JOIN Rivers r ON cr.RiverId = r.Id
		GROUP BY c.CountryName
		ORDER BY [HighestPeak] DESC, [LongestRiver] DESC, c.CountryName
	

--- 18.Highest Peak Name and Elevation by Country ---
SELECT TOP(5) c.CountryName AS [Country],
	ISNULL(p.PeakName, '(no highest peak)') AS [Highest Peak Name],
	  CASE
		WHEN COUNT(p.Elevation) = 0 THEN 0
		ELSE MAX(p.Elevation)
	 END AS [Highest Peak Elevation],
	ISNULL(m.MountainRange, '(no mountain)') AS [Mountaion]
	FROM Countries c
	LEFT JOIN MountainsCountries mc ON mc.CountryCode = c.CountryCode
	LEFT JOIN Mountains m ON mc.MountainId = m.Id
	LEFT JOIN Peaks p ON m.Id = p.MountainId
	GROUP BY c.CountryName, p.PeakName, m.MountainRange
	ORDER BY c.CountryName, [Highest Peak Name]


--SELECT TOP (5)
	--Country, [Highest Peak Name],[Highest Peak Elevation], Mountain
	--FROM (SELECT 
	--*,
	--DENSE_RANK () OVER (PARTITION BY Country ORDER BY [Highest Peak Elevation] DESC) AS [Rank]
	--FROM (SELECT 
	--	Countries.CountryName AS Country,
	--	CASE
	--		WHEN Peaks.PeakName IS NULL THEN '(no highest peak)'
	--		ELSE Peaks.PeakName
	--	END AS [Highest Peak Name],
	--	CASE
	--		WHEN Peaks.Elevation IS NULL THEN '0'
	--		ELSE Peaks.Elevation
	--	END AS [Highest Peak Elevation],
	--	CASE
	--		WHEN Mountains.MountainRange IS NULL THEN '(no mountain)' 
	--		ELSE Mountains.MountainRange 
	--	END AS Mountain
	--	FROM Countries
	--	LEFT JOIN MountainsCountries ON Countries.CountryCode = MountainsCountries.CountryCode
	--	LEFT JOIN Mountains ON MountainsCountries.MountainId = Mountains.Id
	--	LEFT JOIN Peaks ON Mountains.Id = Peaks.MountainId)  AS RankingQuery) as FinalQuery
--WHERE [Rank] = 1
--ORDER BY Country ASC, [Highest Peak Name] ASC