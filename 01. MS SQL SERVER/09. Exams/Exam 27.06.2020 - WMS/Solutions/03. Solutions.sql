USE WMS

---02.Insert ---
INSERT INTO Clients
	(FirstName, LastName, Phone)
		VALUES
	('Teri', 'Ennaco', '570-889-5187'),
	('Merlyn', 'Lawler', '201-588-7810'),
	('Georgene', 'Montezuma', '925-615-5185'),
	('Jettie', 'Mconnell', '908-802-3564'),
	('Lemuel', 'Latzke', '631-748-6479'),
	('Melodie', 'Knipp', '805-690-1682'),
	('Candida', 'Corbley', '908-275-8357')

INSERT INTO Parts
	(SerialNumber,[Description],Price,VendorId)
		VALUES
	('WP8182119','Door Boot Seal',117.86,2),
	('W10780048','Suspension Rod',42.81,1),
	('W10841140','Silicone Adhesive',6.77,4),
	('WPY055980','High Temperature Adhesive',13.94,3)


---03.Update---
UPDATE Jobs
	SET MechanicId = 3, [Status] = 'In Progress'
	WHERE [Status] = 'Pending'


---04.Delete---
DELETE FROM OrderParts
	WHERE OrderId = (SELECT OrderId FROM Orders WHERE OrderId = 19)

DELETE FROM Orders
	WHERE OrderId = (SELECT OrderId FROM Orders WHERE OrderId = 19)


---05.Mechanic Assignments---
SELECT FirstName + ' ' + LastName AS [Mechanic],
	   [Status],
	   IssueDate
	FROM Mechanics m
	JOIN Jobs j ON j.MechanicId = m.MechanicId
	ORDER BY m.MechanicId, IssueDate, JobId


---06.Current Clients---
SELECT FirstName + ' ' + LastName AS [Mechanic],
	   DATEDIFF(DAY,  IssueDate, '2017-04-24') AS [Days going],
	   [Status]
	FROM Clients c
	JOIN Jobs j ON j.ClientId = c.ClientId
	WHERE [Status] != 'Finished'
	ORDER BY [Days going] DESC, c.ClientId


---07.Mechanic Performance---
SELECT FirstName + ' ' + LastName AS [Mechanic],
		AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Average Days]
	FROM Mechanics m
	JOIN Jobs j ON j.MechanicId = m.MechanicId
	GROUP BY FirstName,LastName, m.MechanicId
	ORDER BY m.MechanicId


---08.Available Mechanics---
SELECT CONCAT(FirstName,' ',LastName) AS Available
       FROM Mechanics m
  LEFT JOIN Jobs j ON j.MechanicId = m.MechanicId
     WHERE  j.JobId IS NULL 
	        OR 'Finished' = ALL(SELECT j.[Status]
								FROM Jobs j  
								WHERE j.MechanicId = m.MechanicId)	  
   GROUP BY FirstName,LastName,m.MechanicId
   ORDER BY m.MechanicId


---09.Past Expenses---
SELECT j.JobId, ISNULL(SUM(Price * Quantity), 0) AS [Total]
	FROM Jobs j
	LEFT JOIN Orders o ON o.JobId = j.JobId
	LEFT JOIN OrderParts op ON op.OrderId = o.OrderId
	LEFT JOIN Parts p ON op.PartId = p.PartId
	WHERE [Status] = 'Finished'
	GROUP BY j.JobId
	ORDER BY [Total] DESC, j.JobId


---10.Missing Parts---
SELECT 
	p.PartId,
	p.Description,
	ISNULL(pn.Quantity, 0) AS Required,
	ISNULL(p.StockQty, 0) AS [In Stock],
	ISNULL(IIF(o.Delivered = 0,op.Quantity, 0), 0) AS Ordered
	FROM Parts AS p
    LEFT JOIN PartsNeeded AS pn ON pn.PartId = p.PartId
    LEFT JOIN OrderParts AS op ON op.PartId = p.PartId
    LEFT JOIN Jobs AS j ON j.JobId = pn.JobId
    LEFT JOIN Orders AS o ON o.JobId = j.JobId
	WHERE pn.Quantity > ISNULL(
								(p.StockQty + IIF(o.Delivered = 0,op.Quantity, 0))
								, 0)
    AND j.Status != 'Finished'
	ORDER BY p.PartId


---11.Place Order--- 
CREATE PROC usp_PlaceOrder(@JobId INT, @SerialNumber VARCHAR(50), @Quantity INT) AS
BEGIN
	DECLARE @JobStatus VARCHAR(MAX) = (SELECT Status FROM Jobs WHERE JobId = @JobId)
	DECLARE @JobExists BIT = (SELECT COUNT(JobId) FROM Jobs WHERE JobId = @JobId)
	DECLARE @PartExists BIT = (SELECT COUNT(SerialNumber) FROM Parts WHERE SerialNumber = @SerialNumber)

	IF(@Quantity <= 0)
	BEGIN;
		THROW 50012, 'Part quantity must be more than zero!' , 1
	END

	IF(@JobStatus = 'Finished')
	BEGIN;
		THROW 50011, 'This job is not active!', 1
	END

	IF(@JobExists = 0)
	BEGIN;
		THROW 50013, 'Job not found!', 1
	END

	IF(@PartExists = 0)
	BEGIN;
		THROW 50014, 'Part not found!', 1
	END

	DECLARE @OrderForJobExists INT = 
	(
		SELECT COUNT(o.OrderId) FROM Orders AS o
		WHERE o.JobId = @JobId AND o.IssueDate IS NULL
	)

	IF(@OrderForJobExists = 0)
	BEGIN
		INSERT INTO Orders VALUES
		(@JobId, NULL, 0)
	END

	DECLARE @OrderId INT = 
	(
		SELECT o.OrderId FROM Orders AS o
		WHERE o.JobId = @JobId AND o.IssueDate IS NULL
	)

	IF(@OrderId > 0 AND @PartExists = 1 AND @Quantity > 0)
	BEGIN
		DECLARE @PartId INT = (SELECT PartId FROM Parts WHERE SerialNumber = @SerialNumber)
		DECLARE @PartExistsInOrder INT = (SELECT COUNT(*) FROM OrderParts WHERE OrderId = @OrderId AND PartId = @PartId)

		IF(@PartExistsInOrder > 0)
		BEGIN
			UPDATE OrderParts
			SET Quantity += @Quantity
			WHERE OrderId = @OrderId AND PartId = @PartId
		END
		ELSE
		BEGIN
			INSERT INTO OrderParts VALUES
			(@OrderId, @PartId, @Quantity)
		END
	END
END


---12.Cost Of Order---
CREATE FUNCTION udf_GetCost(@jobId INT)
RETURNS DECIMAL(12,2)
AS
BEGIN
	DECLARE @result DECIMAL(12,2) = ISNULL((SELECT 
							SUM(p.Price) as Result
							FROM Orders as o
							JOIN OrderParts as op ON o.OrderId = op.OrderId
							JOIN Parts as p ON op.PartId = p.PartId
							WHERE o.JobId = @jobId), 0)

	RETURN @result;
END