CREATE PROCEDURE [dbo].[spGetAddressByEventId]
	@eventId uniqueidentifier
AS
	DECLARE @eventEntityTypeId INT = (SELECT EntityTypeId FROM [dbo].[EntityType] WHERE EntityName = 'Event')
	
	EXEC [dbo].[spGetAddressByEntityTypeAndId] @entityTypeId = @eventEntityTypeId, @entityId = @eventId
RETURN 0
