CREATE PROCEDURE [dbo].[spUserHasWriteAccessToEntity]
	@userId nvarchar(100),
	@entityType int,
	@entityId uniqueidentifier
AS

	-- Reunion Entity
	IF @entityType = 1
	BEGIN
		SELECT COUNT(1)
		FROM [dbo].[Reunion] r (NOLOCK)
		JOIN [dbo].[ReunionOrganizer] ro (NOLOCK)
			ON r.ReunionId = ro.ReunionId AND r.ReunionId = @entityId AND r.IsActive = 1 AND ro.IsActive = 1
		JOIN [dbo].[User] u (NOLOCK)
			ON ro.UserId = u.Id AND u.UserId = @userId
	END

	-- Event Entity
	IF @entityType = 2
	BEGIN
		SELECT COUNT(1)
			FROM [dbo].[Event] e (NOLOCK)
			JOIN [dbo].[Reunion] r (NOLOCK)
				ON e.ReunionId = r.ReunionId AND e.EventId = @entityId AND e.IsActive = 1
			JOIN [dbo].[ReunionOrganizer] ro (NOLOCK)
				ON r.ReunionId = ro.ReunionId and ro.IsActive = 1
			JOIN [dbo].[User] u (NOLOCK)
				ON ro.UserId = u.Id AND u.UserId = @userId	
	END