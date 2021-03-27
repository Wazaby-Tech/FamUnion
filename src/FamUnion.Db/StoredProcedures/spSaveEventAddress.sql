CREATE PROCEDURE [dbo].[spSaveEventAddress]
	@userId nvarchar(100),
	@eventId UNIQUEIDENTIFIER,
	@description NVARCHAR(255),
	@line1 NVARCHAR(100),
	@line2 NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(2),
	@zipcode NVARCHAR(5)
AS
	EXEC [dbo].[spSaveAddressByEntityTypeAndId] @userId = @userId, @entityType = 'Event', @entityId = @eventId,
		@description = @description, @line1 = @line1, @line2 = @line2, @city = @city, @state = @state, @zipcode = @zipcode

	EXEC [dbo].[spGetAddressByEventId] @eventId = @eventId;
