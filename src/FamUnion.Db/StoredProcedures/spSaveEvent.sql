CREATE PROCEDURE [dbo].[spSaveEvent]
	@userId nvarchar(100),
	@id uniqueidentifier NULL,
	@reunionId uniqueidentifier,
	@name nvarchar(100),
	@details nvarchar(4000),
	@starttime date,
	@endtime date
AS
	DECLARE @insertId uniqueidentifier = ISNULL(@id, newid())

	MERGE INTO [dbo].[Event] TARGET
	USING (
		SELECT 
			@id [EventId],
			@reunionId [ReunionId],
			@name [Name],
			@details [Details],
			@starttime [StartTime],
			@endtime [EndTime],
			null [AddressId],
			null [CreatedBy],
			null [CreatedDate],
			null [ModifiedBy],
			null [ModifiedDate]
	) SOURCE
	ON TARGET.EventId = SOURCE.EventId
	WHEN NOT MATCHED
	THEN
		INSERT (EventId, ReunionId, Name, Details, StartTime, EndTime, CreatedDate, CreatedBy)
		VALUES (@insertId, SOURCE.ReunionId, SOURCE.Name, SOURCE.Details, SOURCE.StartTime, SOURCE.EndTime, SYSDATETIME(), @userId)
	WHEN MATCHED AND TARGET.IsActive = 1
	THEN
		UPDATE SET
			TARGET.Name = SOURCE.Name,
			TARGET.ReunionId = SOURCE.ReunionId,
			TARGET.Details = SOURCE.Details,
			TARGET.StartTime = SOURCE.StartTime,
			TARGET.EndTime = SOURCE.EndTime,
			TARGET.ModifiedBy = @userId,
			TARGET.ModifiedDate = SYSDATETIME();

	EXEC [dbo].[spGetEventById] @id = @insertId;
