CREATE PROCEDURE [dbo].[spDeleteEventById]
	@eventId uniqueidentifier
AS
	UPDATE a
	SET
		a.IsActive = 0,
		a.ModifiedBy = SUSER_SNAME(),
		a.ModifiedDate = SYSDATETIME()
	FROM [dbo].[Address] a
	JOIN [dbo].[EntityType] et on a.EntityType = et.EntityTypeId
	WHERE
		et.EntityName = 'Event' AND
		a.EntityId = @eventId

	UPDATE [dbo].[Event]
	SET
		IsActive = 0,
		ModifiedBy = SUSER_SNAME(),
		ModifiedDate = SYSDATETIME()
	WHERE
		EventId = @eventId

RETURN 0
