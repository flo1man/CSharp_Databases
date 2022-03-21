USE Diablo

--- 13.*Scalar Function: Cash in User Games Odd Rows ---
CREATE FUNCTION ufn_CashInUsersGames(@GameName NVARCHAR(50))
RETURNS TABLE 
AS
RETURN SELECT
	SUM(Cash) AS SumCash
	FROM
	(SELECT 
		g.Id AS [GameId],
		g.[Name] AS [Game Name],
		ug.Cash AS [Cash],
		ROW_NUMBER() OVER (PARTITION BY g.[Name] ORDER BY ug.Cash DESC) AS [RowNumber]
		FROM UsersGames as ug 
		JOIN Games as g ON ug.GameId = g.Id
		WHERE g.Name = @GameName) as Query
	WHERE [RowNumber] % 2 <> 0

--SELECT * FROM dbo.ufn_CashInUsersGames ('Love in a mist')