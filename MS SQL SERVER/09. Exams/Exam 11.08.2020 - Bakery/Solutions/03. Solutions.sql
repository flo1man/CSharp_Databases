USE Bakery

--2
INSERT INTO Distributors
	([Name], CountryId, AddressText, Summary)
		VALUES
	('Deloitte & Touche', 2, '6 Arch St #9757', 'Customizable neutral traveling'),
	('Congress Title', 13, '58 Hancock St', 'Customer loyalty'),
	('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery'),
	('General Color Co Inc', 21, '6185 Bohn St #72', 'Focus group'),
	('Beck Corporation', 23, '21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers
	(FirstName, LastName, Age, Gender, PhoneNumber, CountryId)
		VALUES
	('Francoise', 'Rautenstrauch',	15,	'M','0195698399',	5),
	('Kendra', 'Loud', 22,	'F','0063631526',	11),
	('Lourdes', 'Bauswell', 50,	'M','0139037043',	8),
	('Hannah', 'Edmison', 18,	'F','0043343686',	1),
	('Tom', 'Loeza', 31,	'M','0144876096',	23),
	('Queenie', 'Kramarczyk', 30,	'F','0064215793',	29),
	('Hiu', 'Portaro', 25,	'M','0068277755',	16),
	('Josefa', 'Opitz', 43,	'F','0197887645',	17)


--3
UPDATE Ingredients
	SET DistributorId = 35
	WHERE [Name] IN ('Bay Leaf', 'Paprika', 'Poppy') 

UPDATE Ingredients
	SET OriginCountryId = 14
	WHERE OriginCountryId = 8


--4
DELETE FROM Feedbacks
	WHERE CustomerId = 14 OR ProductId = 5


--5
SELECT [Name], Price, [Description]
	FROM Products
	ORDER BY Price DESC, [Name]


--6
SELECT ProductId, Rate, [Description], CustomerId, Age, Gender
	FROM Feedbacks f
	JOIN Customers c ON f.CustomerId = c.Id
	WHERE Rate < 5.0
	ORDER BY ProductId DESC, Rate
	

--7??
SELECT CONCAT(FirstName, ' ', LastName), PhoneNumber, Gender
	FROM Feedbacks f
	LEFT JOIN Customers c ON f.CustomerId = c.Id
	WHERE CustomerId IS NULL
	ORDER BY CustomerId


--8??
SELECT DISTINCT FirstName, Age, PhoneNumber
	FROM Customers c
	LEFT JOIN  Countries co ON c.CountryId = c.Id
	WHERE (Age >= 21 AND CHARINDEX('an', FirstName) > 0)
		OR (RIGHT(PhoneNumber, 2) = '38' AND co.[Name] != 'Greece')
	ORDER BY FirstName, Age DESC


--9
SELECT d.[Name], i.[Name], p.[Name], AVG(Rate)
	FROM Ingredients i
	JOIN Distributors d ON i.DistributorId = d.Id
	JOIN ProductsIngredients pin ON pin.IngredientId = i.Id
	JOIN Products p ON pin.ProductId = p.Id
	JOIN Feedbacks f ON f.ProductId = p.Id
	GROUP BY d.[Name], i.[Name], p.[Name]
	HAVING AVG(Rate) BETWEEN 5 AND 8
	ORDER BY d.[Name], i.[Name], p.[Name]


--10??
SELECT c.[Name], d.[Name],
		DENSE_RANK() OVER(PARTITION BY CountryId ORDER BY CountryId)
	FROM Countries c
	JOIN Distributors d ON d.CountryId = c.Id
	ORDER BY c.[Name], d.[Name]


--11
CREATE VIEW v_UserWithCountries AS
	SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [CustomerName], Age, Gender, co.[Name]
		FROM Customers c
		JOIN Countries co ON c.CountryId = co.Id

--SELECT TOP 5 * FROM v_UserWithCountries
	--ORDER BY Age


--12
CREATE TRIGGER tr_DelteRelationsOfProducts
ON Products FOR DELETE
AS
BEGIN
	DELETE FROM ProductsIngredients
		WHERE ProductId = deleted.Id

END