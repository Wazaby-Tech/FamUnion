CREATE PROCEDURE [dbo].[spSaveReunionAddress]
	@reunionId UNIQUEIDENTIFIER,
	@description NVARCHAR(255),
	@line1 NVARCHAR(100),
	@line2 NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(2),
	@zipcode NVARCHAR(5)
AS
	EXEC [dbo].[spSaveAddressByEntityTypeAndId] @entityType = 'Reunion', @entityId = @reunionId,
		@description = @description, @line1 = @line1, @line2 = @line2, @city = @city, @state = @state, @zipcode = @zipcode

	EXEC [dbo].[spGetAddressByReunionId] @reunionId = @reunionId;
