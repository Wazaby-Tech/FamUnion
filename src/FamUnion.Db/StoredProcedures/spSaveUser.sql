CREATE PROCEDURE [dbo].[spSaveUser]
	@id UNIQUEIDENTIFIER,
	@userId NVARCHAR(100),
	@email NVARCHAR(255),
	@firstName NVARCHAR(100),
	@lastName NVARCHAR(100),
	@authType INT
AS
	DECLARE @insertId uniqueidentifier = ISNULL(@id, NEWID())

	MERGE INTO [dbo].[User] TARGET
	USING (
		SELECT 
			@id [Id],
			@userId [UserId],
			@email [Email],
			@firstName [FirstName],
			@lastName [LastName],
			@authType [AuthType],
			null [CreatedBy],
			null [CreatedDate],
			null [ModifiedBy],
			null [ModifiedDate]
	) SOURCE
	ON TARGET.UserId = SOURCE.UserId
	WHEN NOT MATCHED
	THEN
		INSERT (Id, UserId, Email, FirstName, LastName, AuthType, CreatedDate, CreatedBy)
		VALUES (@insertId, SOURCE.UserId, SOURCE.Email, SOURCE.FirstName, SOURCE.LastName, SOURCE.AuthType, SYSDATETIME(), SUSER_SNAME())
	WHEN MATCHED
	THEN
		UPDATE SET
			TARGET.Email = SOURCE.Email,
			TARGET.FirstName = SOURCE.FirstName,
			TARGET.LastName = SOURCE.LastName,
			TARGET.ModifiedBy = SUSER_SNAME(),
			TARGET.ModifiedDate = SYSDATETIME();

	EXEC [dbo].[spGetUserById] @userId = @userId;
