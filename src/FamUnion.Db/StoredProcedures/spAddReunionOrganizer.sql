CREATE PROCEDURE [dbo].[spAddReunionOrganizer]
	@reunionId UNIQUEIDENTIFIER,
	@userId NVARCHAR(100)
AS
	
MERGE INTO [dbo].[ReunionOrganizer] TARGET
USING (
	SELECT
		@reunionId [ReunionId],
		u.Id [OrganizerId]
	FROM [dbo].[User] u (NOLOCK)
	WHERE u.UserId = @userId
) SOURCE
ON SOURCE.ReunionId = TARGET.ReunionId
AND SOURCE.OrganizerId = TARGET.UserId
WHEN NOT MATCHED
THEN
	INSERT (ReunionId, UserId) VALUES (SOURCE.ReunionId, SOURCE.OrganizerId)
WHEN MATCHED AND IsActive <> 1
THEN
	UPDATE
		SET IsActive = 1;
