CREATE PROCEDURE [dbo].[spGetAddressByEntityTypeAndId]
	@entityTypeId INT,
	@entityId UNIQUEIDENTIFIER
AS
	SELECT
		a.AddressId [Id],
		@entityTypeId [AddressType],
		a.Description,
		a.EntityType,
		a.Line1,
		a.Line2,
		a.City,
		a.State,
		a.ZipCode,
		a.Latitude,
		a.Longitude,
		a.CreatedBy,
		a.CreatedDate,
		a.ModifiedBy,
		a.ModifiedDate
	FROM [dbo].[Address] a (NOLOCK) 
	WHERE 
		a.EntityType = @entityTypeId 
		AND a.EntityId = @entityId
		AND a.IsActive = 1
