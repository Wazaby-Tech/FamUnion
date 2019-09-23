CREATE PROCEDURE [dbo].[spGetEventById]
	@id UNIQUEIDENTIFIER
AS
	SELECT
		EventId [Id],
		ReunionId,
		[Name],
		Details,
		StartTime,
		EndTime,
		AddressId,
		CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifiedDate
	FROM 
		[dbo].[Event] (NOLOCK)
	WHERE
		EventId = @id
