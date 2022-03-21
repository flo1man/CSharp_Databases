USE Bitbucket

--- 02.Insert ---
INSERT INTO [Files]
	([Name], [Size], ParentId, CommitId)
	VALUES
	('Trade.idk',2598.0,1,1),
	('menu.net',9238.31,2,2),
	('Administrate.soshy',1246.93,3,3),
	('Controller.php',7353.15,4,4),
	('Find.java',9957.86,5,5),
	('Controller.json',14034.87,3,6),
	('Operate.xix',7662.92,7,7)

INSERT INTO Issues
	(Title,IssueStatus,RepositoryId,AssigneeId)
	VALUES
	('Critical Problem with HomeController.cs file','open',1,4),
	('Typo fix in Judge.html','open',4,3),
	('Implement documentation for UsersService.cs','closed',8,2),
	('Unreachable code in Index.cs','open',9,8)


--- 03.Update ---
UPDATE Issues
	SET IssueStatus = 'closed'
	WHERE AssigneeId = 6


--- 04.Delete ---
DELETE
	FROM Issues
	WHERE RepositoryId = (SELECT Id FROM Repositories WHERE Name = 'Softuni-Teamwork')

DELETE 
	FROM RepositoriesContributors
	WHERE RepositoryId = (SELECT Id FROM Repositories WHERE Name = 'Softuni-Teamwork')


--- 05.Commits ---
SELECT Id, [Message], RepositoryId, ContributorId
	FROM Commits
	ORDER BY Id, [Message], RepositoryId, ContributorId


--- 06.Front-end ---
SELECT Id, [Name], Size
	FROM Files
	WHERE Size > 1000 AND CHARINDEX('html', [Name], 0) > 0
	ORDER BY Size DESC, Id, [Name]


--- 07.Issue Assignment ---
SELECT i.Id, CONCAT(u.Username, ' : ', Title) AS [IssueAssignee]
	FROM Issues i
	JOIN Users u ON i.AssigneeId = u.Id
	ORDER BY i.Id DESC, [IssueAssignee]


--- 08.Single Files ---
SELECT 
	f1.Id,
    f1.Name,
	CONCAT(f1.Size, 'KB') as Size
	FROM Files as f1
	LEFT JOIN Files as f2 ON f1.Id = f2.ParentId
	WHERE f2.ParentId IS NULL
	ORDER BY f1.Id ASC, f1.Name ASC, [Size] DESC


--- 09.Commits in Repositories ---
SELECT TOP(5) r.Id, r.[Name],
	COUNT(c.RepositoryId) AS [Commits]
	FROM Commits c
	JOIN Repositories r ON c.RepositoryId = r.Id
	JOIN Users u ON u.Id = c.ContributorId
	JOIN Issues i ON r.Id = i.AssigneeId
	GROUP BY r.Id, r.[Name]
	ORDER BY r.Id, r.[Name],[Commits] DESC


SELECT TOP(5) r.Id, r.[Name], COUNT(*) AS [Commits]
	FROM Commits c
	JOIN Repositories r ON c.RepositoryId = r.Id
	JOIN RepositoriesContributors rc ON rc.RepositoryId = r.Id
	GROUP BY r.Id, r.[Name]
	ORDER BY [Commits] DESC, r.Id, r.[Name]

--- 10.Average Size ---
SELECT Username, AVG(Size) AS [Size]
	FROM Commits c
	JOIN Files f ON f.CommitId = C.Id
	JOIN Users u ON u.Id = c.ContributorId
	GROUP BY Username
	ORDER BY [Size] DESC, Username


--- 11.All User Commits ---
CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(MAX))
RETURNS INT
AS
BEGIN
	DECLARE @result INT = (
	SELECT COUNT(*) 
		FROM Users u
		JOIN Commits c ON c.ContributorId = u.Id
		WHERE u.Username = @username
		GROUP BY c.ContributorId);
	IF @result IS NULL
		SET @result = 0;

	RETURN @result;
END

--SELECT dbo.udf_AllUserCommits('UnderSinduxrein')


--- 12.Search for Files ---
CREATE PROC usp_SearchForFiles(@fileExtension VARCHAR(MAX))
AS
BEGIN
	SELECT Id, [Name], CONCAT(Size, 'KB') AS Size
		FROM Files
		WHERE CHARINDEX(@fileExtension, [Name], 0) > 0
		ORDER BY Id, [Name], Size
END

--EXEC usp_SearchForFiles 'txt'