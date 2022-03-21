USE Bank

--- 9.Find Full Name ---
CREATE PROC usp_GetHoldersFullName
AS
BEGIN
	SELECT FirstName + ' ' + LastName AS [Full Name]
		FROM AccountHolders
END

--EXEC usp_GetHoldersFullName


--- 10.People with Balance Higher Than ---
CREATE PROC usp_GetHoldersWithBalanceHigherThan(@number DECIMAL(18,2))
AS
BEGIN
	SELECT FirstName, LastName
		FROM AccountHolders ah
		JOIN Accounts ac ON ah.Id = ac.AccountHolderId
		GROUP BY FirstName, LastName
		HAVING SUM(ac.Balance) > @number
		ORDER BY FirstName, LastName
END

--EXEC usp_GetHoldersWithBalanceHigherThan 7000


--- 11.Future Value Function ---
CREATE FUNCTION ufn_CalculateFutureValue
	(@InitialSum DECIMAL(18,4), @YearlyRate FLOAT, @NumberOfYear INT)
RETURNS DECIMAL(18,4)
AS
BEGIN
	DECLARE	@Value DECIMAL(18,4);

	SET @Value = @InitialSum * (POWER((1 + @YearlyRate), @NumberOfYear))

	RETURN CAST(ROUND(@Value, 4) AS DECIMAL(18,4))
END

--SELECT dbo.ufn_CalculateFutureValue(1000,0.1,5)


--- 12.Calculating Interest ---
CREATE OR ALTER PROC usp_CalculateFutureValueForAccount
	(@AccountId INT, @InterestRate FLOAT)
AS
BEGIN
	SELECT a.Id, FirstName, LastName, Balance, dbo.ufn_CalculateFutureValue(Balance, @InterestRate, 5)
		FROM AccountHolders ah
		JOIN Accounts a ON ah.Id = a.AccountHolderId
	  WHERE a.Id = @AccountId
END

--EXEC usp_CalculateFutureValueForAccount '1', 0.1


