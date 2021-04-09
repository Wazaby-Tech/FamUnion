CREATE PROCEDURE [dbo].[spCancelEventById]
	@userId nvarchar(100),
	@eventId uniqueidentifier
AS
	UPDATE a
	SET
		a.IsActive = 0,
		a.ModifiedBy = @userId,
		a.ModifiedDate = SYSDATETIME()
	FROM [dbo].[Address] a
	JOIN [dbo].[EntityType] et on a.EntityType = et.EntityTypeId
	WHERE
		et.EntityName = 'Event' AND
		a.EntityId = @eventId

	UPDATE [dbo].[Event]
	SET
		IsActive = 0,
		ModifiedBy = @userId,
		ModifiedDate = SYSDATETIME()
	WHERE
		EventId = @eventId
