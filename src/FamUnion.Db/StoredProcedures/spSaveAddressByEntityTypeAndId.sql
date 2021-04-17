CREATE PROCEDURE [dbo].[spSaveAddressByEntityTypeAndId]	
	@userId nvarchar(100),
	@entityType nvarchar(100),
	@entityId uniqueidentifier,
	@description nvarchar(255),
	@line1 nvarchar(100),
	@line2 nvarchar(100),
	@city nvarchar(100),
	@state nvarchar(2),
	@zipcode nvarchar(5)
AS
	DECLARE @insertId uniqueidentifier = newid()
	DECLARE @entityTypeId INT = (SELECT EntityTypeId FROM [dbo].[EntityType] WHERE EntityName = @entityType)

	MERGE INTO [dbo].[Address] TARGET
	USING (
		SELECT 
			@insertId [AddressId],
			@entityId [EntityId],
			@description [Description],
			@line1 [Line1],
			@line2 [Line2],
			@city [City],
			@state [State],
			@zipcode [ZipCode],
			null [Latitude],
			null [Longitude],
			null [CreatedBy],
			null [CreatedDate],
			null [ModifiedBy],
			null [ModifiedDate]
	) SOURCE
	ON TARGET.EntityId = SOURCE.EntityId AND TARGET.EntityType = @entityTypeId AND TARGET.IsActive = 1
	WHEN NOT MATCHED 
	THEN
		INSERT (AddressId, EntityId, EntityType, Description, Line1, Line2, City, State, ZipCode, Latitude, Longitude, CreatedDate, CreatedBy)
		VALUES (@insertId, SOURCE.EntityId, @entityTypeId, SOURCE.Description, SOURCE.Line1, SOURCE.Line2, SOURCE.City, SOURCE.State, SOURCE.ZipCode, SOURCE.Latitude, SOURCE.Longitude, SYSDATETIME(), @userId)
	WHEN MATCHED AND TARGET.IsActive = 1
	THEN
		UPDATE SET
			TARGET.Description = SOURCE.Description,
			TARGET.Line1 = SOURCE.Line1,
			TARGET.Line2 = SOURCE.Line2,
			TARGET.City = SOURCE.City,
			TARGET.State = SOURCE.State,
			TARGET.ZipCode = SOURCE.ZipCode,
			TARGET.Latitude = SOURCE.Latitude,
			TARGET.Longitude = SOURCE.Longitude,
			TARGET.ModifiedBy = @userId,
			TARGET.ModifiedDate = SYSDATETIME();
