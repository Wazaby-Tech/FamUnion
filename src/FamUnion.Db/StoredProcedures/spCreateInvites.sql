CREATE PROCEDURE [dbo].[spCreateInvites]
	@invites [dbo].[udfInviteType] READONLY,
	@userId NVARCHAR(255)
AS
	MERGE INTO [dbo].[ReunionInvite] TARGET
	USING @invites SOURCE
	ON SOURCE.ReunionId = TARGET.ReunionId
		AND SOURCE.Email = TARGET.Email
	WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (InviteId, ReunionId, Email, Name, CreatedBy, CreatedDate)
		VALUES (NEWID(), SOURCE.ReunionId, SOURCE.Email, SOURCE.Name, @userId, SYSDATETIME());
