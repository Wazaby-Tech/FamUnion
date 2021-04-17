CREATE PROCEDURE [dbo].[spGetEventsByReunionId]
	@reunionId UNIQUEIDENTIFIER
AS
	SELECT
		EventId [Id],
		ReunionId,
		[Name],
		Details,
		StartTime,
		EndTime,
		AttireType,
		AddressId,
		CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifiedDate
	FROM 
		[dbo].[Event] (NOLOCK)
	WHERE
		ReunionId = @reunionId
	ORDER BY
		[StartTime], [Name]
