USE Airport

--- 02.Insert ---
DECLARE @idx INT = 5;
WHILE (@idx <= 15)
	BEGIN
		INSERT INTO Passengers (Fullname, Email)
		SELECT FirstName + ' ' + LastName AS [FullName],
			   CONCAT(FirstName,LastName,'@gmail.com')
		FROM Pilots
		WHERE Id = @idx
		SET @idx += 1;
	END


--- 03.Update ---
UPDATE Aircraft
	SET Condition = 'A'
	WHERE Condition IN('C', 'B')
			AND (FlightHours = 0 OR FlightHours <= 100 OR FlightHours IS NULL)
			AND DATEPART(YEAR, [Year]) >= 2013


--- 04.Delete ---
ALTER TABLE FlightDestinations
	DROP CONSTRAINT FK__FlightDes__Passe__3B75D760

DELETE FROM Passengers
	WHERE LEN(FullName) >= 10


--- 05.Aircraft ---
SELECT Manufacturer, Model, FlightHours, Condition
	FROM Aircraft
	ORDER BY FlightHours DESC


--- 06.Pilots and Aircraft ---
SELECT FirstName, LastName, Manufacturer, Model,FlightHours
	FROM Pilots p
	JOIN PilotsAircraft pa ON pa.PilotId = p.Id
	JOIN Aircraft a ON pa.AircraftId = a.Id
	WHERE FlightHours < 304 AND FlightHours IS NOT NULL
	ORDER BY FlightHours DESC, FirstName


--- 07.Top 20 Flight Destinations ---
SELECT TOP(20) fd.Id,[Start],Fullname,AirportName,TicketPrice
	FROM FlightDestinations fd
	JOIN Airports a ON fd.AirportId = a.Id
	JOIN Passengers p ON fd.PassengerId = p.Id
	WHERE DATEPART(DAY, [Start]) % 2 = 0
	ORDER BY TicketPrice DESC, AirportName


--- 08.Number of Flights for Each Aircraft ---
SELECT  a.Id,Manufacturer,FlightHours,
		COUNT(AircraftId) AS [FlightDestinationsCount],
		ROUND(AVG(TicketPrice), 2) AS [AvgPrice]
	FROM Aircraft a
	JOIN FlightDestinations fd ON fd.AircraftId = a.Id
	GROUP BY  a.Id,AircraftId,Manufacturer,FlightHours
	HAVING COUNT(AircraftId) >= 2
	ORDER BY [FlightDestinationsCount] DESC,a.Id


--- 09.Regular Passengers ---
SELECT Fullname, COUNT(PassengerId), SUM(TicketPrice)
	FROM Passengers p
	JOIN FlightDestinations fd ON fd.PassengerId = p.Id
	WHERE SUBSTRING(FullName,2,1) = 'a'
	GROUP BY Fullname, PassengerId
	HAVING COUNT(PassengerId) > 1
	ORDER BY Fullname


--- 10.Full Info for Flight Destinations ---
SELECT AirportName, [Start], TicketPrice, Fullname, Manufacturer, Model
	FROM FlightDestinations fd
	JOIN Airports a ON fd.AirportId = a.Id
	JOIN Passengers p ON fd.PassengerId = p.Id
	JOIN Aircraft ai ON fd.AircraftId = ai.Id
	WHERE DATEPART(HOUR, [Start]) BETWEEN 6 AND 20
	AND TicketPrice > 2500
	ORDER BY Model


--- 11.Find all Destinations by Email Address ---
CREATE FUNCTION udf_FlightDestinationsByEmail(@email VARCHAR(MAX))
RETURNS INT
AS
BEGIN
	DECLARE @result INT = (
	SELECT COUNT(PassengerId)
		FROM Passengers p
		JOIN FlightDestinations fd ON fd.PassengerId = p.Id
		WHERE p.Email = @email
		GROUP BY PassengerId)
		IF (@result IS NULL)
			SET @result = 0;

	RETURN @result;
END

--SELECT dbo.udf_FlightDestinationsByEmail ('PierretteDunmuir@gmail.com')
--SELECT dbo.udf_FlightDestinationsByEmail('MerisShale@gmail.com')
--SELECT dbo.udf_FlightDestinationsByEmail('Montacute@gmail.com')


--- 12.Full Info for Airports ---
CREATE PROC usp_SearchByAirportName(@airportName VARCHAR(70))
AS
BEGIN
	SELECT AirportName,
	   Fullname,
	   CASE
			WHEN TicketPrice <= 400 THEN 'Low'
			WHEN TicketPrice BETWEEN 401 AND 1500 THEN 'Medium'
			ELSE 'High'
	   END AS [LevelOfTicketPrice],
	   Manufacturer,
	   Condition,
	   TypeName
	FROM Airports ai
	JOIN FlightDestinations fd ON fd.AirportId = ai.Id
	JOIN Passengers p ON fd.PassengerId = p.Id
	JOIN Aircraft a ON fd.AircraftId = a.Id
	JOIN AircraftTypes ait ON a.TypeId = ait.Id
	WHERE AirportName = @airportName
	ORDER BY Manufacturer,Fullname
END

--EXEC usp_SearchByAirportName 'Sir Seretse Khama International Airport'
