CREATE PROCEDURE [dbo].[spSaveReunion]
	@id uniqueidentifier NULL,
	@name nvarchar(100),
	@description nvarchar(4000),
	@startdate date,
	@enddate date
AS

DECLARE @insertId uniqueidentifier = ISNULL(@id, newid())

MERGE INTO [dbo].[Reunion] TARGET
USING (
	SELECT 
		@id [ReunionId],
		@name [Name],
		@description [Description],
		@startdate [StartDate],
		@enddate [EndDate],
		null [AddressId],
		null [CreatedBy],
		null [CreatedDate],
		null [ModifiedBy],
		null [ModifiedDate]
) SOURCE
ON TARGET.ReunionId = SOURCE.ReunionId
WHEN NOT MATCHED
THEN
	INSERT (ReunionId, Name, Description, StartDate, EndDate, CreatedDate, CreatedBy)
	VALUES (@insertId, SOURCE.Name, SOURCE.Description, SOURCE.StartDate, SOURCE.EndDate, SYSDATETIME(), SUSER_SNAME())
WHEN MATCHED
THEN
	UPDATE SET
		TARGET.Name = SOURCE.Name,
		TARGET.Description = SOURCE.Description,
		TARGET.StartDate = SOURCE.StartDate,
		TARGET.EndDate = SOURCE.EndDate,
		TARGET.ModifiedBy = SUSER_SNAME(),
		TARGET.ModifiedDate = SYSDATETIME();

EXEC [dbo].[spGetReunionById] @id = @insertId;
RETURN 0
