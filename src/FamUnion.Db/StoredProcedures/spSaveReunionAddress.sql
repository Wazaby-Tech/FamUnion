CREATE PROCEDURE [dbo].[spSaveReunionAddress]
	@reunionId UNIQUEIDENTIFIER,
	@addressId UNIQUEIDENTIFIER,
	@description NVARCHAR(255),
	@line1 NVARCHAR(100),
	@line2 NVARCHAR(100),
	@city NVARCHAR(100),
	@state NVARCHAR(2),
	@zipcode NVARCHAR(5)
AS
	DECLARE @insertId uniqueidentifier = ISNULL(@addressId, newid())
	DECLARE @reunionEntityTypeId INT = (SELECT EntityTypeId FROM [dbo].[EntityType] WHERE EntityName = 'Reunion')

	MERGE INTO [dbo].[Address] TARGET
	USING (
		SELECT 
			@insertId [AddressId],
			@reunionId [EntityId],
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
	ON TARGET.AddressId = SOURCE.AddressId AND TARGET.EntityType = @reunionEntityTypeId
	WHEN NOT MATCHED 
	THEN
		INSERT (AddressId, EntityId, EntityType, Description, Line1, Line2, City, State, ZipCode, Latitude, Longitude, CreatedDate, CreatedBy)
		VALUES (@insertId, SOURCE.EntityId, @reunionEntityTypeId, SOURCE.Description, SOURCE.Line1, SOURCE.Line2, SOURCE.City, SOURCE.State, SOURCE.ZipCode, SOURCE.Latitude, SOURCE.Longitude, SYSDATETIME(), SUSER_SNAME())
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
			TARGET.ModifiedBy = SUSER_SNAME(),
			TARGET.ModifiedDate = SYSDATETIME();

	EXEC [dbo].[spGetAddressByReunionId] @reunionId = @reunionId;
RETURN 0
