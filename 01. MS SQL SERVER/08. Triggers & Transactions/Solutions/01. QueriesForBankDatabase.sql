USE Bank

--- 01.Create Table Logs ---
CREATE TABLE Logs
(
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT REFERENCES Accounts(Id) NOT NULL,
	OldSum MONEY NOT NULL,
	NewSum MONEY NOT NULL
)

CREATE TRIGGER tr_AddToLogsBalancedChanges
ON Accounts FOR UPDATE
AS
	INSERT INTO Logs(AccountId, OldSum, NewSum)
	SELECT i.AccountHolderId, d.Balance, i.Balance
		FROM inserted i
		JOIN deleted d ON d.Id = i.Id


--- 02.Create Table Emails ---
CREATE TABLE NotificationEmails
(
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT REFERENCES Accounts(Id) NOT NULL,
	[Subject] VARCHAR(MAX) NOT NULL,
	Body VARCHAR(MAX) NOT NULL
)

CREATE TRIGGER tr_AddToTableNotificationEmails
ON Logs AFTER INSERT
AS
	INSERT INTO NotificationEmails(Recipient,[Subject],Body)
	SELECT i.AccountId,
		   'Balance change for account: ' + CAST(i.AccountId AS varchar),
		   CONCAT('On ', CAST(GETDATE() AS varchar), ' your balance was changed from '
		   , i.OldSum, ' to ', i.NewSum, '.')
	   FROM inserted i
	

--- 03.Deposit Money ---
CREATE OR ALTER PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(18, 4))
AS
BEGIN
	IF (@MoneyAmount > 0 AND
	   (SELECT COUNT(*) FROM Accounts WHERE @AccountId = AccountHolderId) > 0)
		BEGIN
			UPDATE Accounts
				SET Balance += ROUND(@MoneyAmount, 4)
				WHERE Id = @AccountId
		END
	ELSE
		ROLLBACK;
		THROW 50001,'Invalid inputs', 16
END


--- 04.Withdraw Money ---
CREATE PROC usp_WithdrawMoney (@AccountId INT, @MoneyAmount DECIMAL(18, 4))
AS
BEGIN
	IF (@MoneyAmount > 0 AND
	   (SELECT COUNT(*) FROM Accounts WHERE @AccountId = AccountHolderId) > 0)
		BEGIN
			UPDATE Accounts
				SET Balance -= CAST(ROUND(@MoneyAmount, 4) AS DECIMAL(18,4))
				WHERE Id = @AccountId
		END
	ELSE
		ROLLBACK;
		THROW 50002,'Invalid inputs', 16
END


--- 05.Money Transfer ---
CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount MONEY)
AS
BEGIN
	DECLARE @SenderAmount DECIMAL(18,4) = 
	(SELECT Balance FROM Accounts WHERE @SenderId = Id)
	IF (@Amount > 0 AND @SenderAmount >= @Amount)
		BEGIN
			EXEC usp_WithdrawMoney @SenderId, @Amount
			EXEC usp_DepositMoney @ReceiverId, @Amount
		END
END