CREATE PROCEDURE [dbo].[spGetReunions]
AS
	SELECT 
		ReunionId [Id],
		Name,
		Description,
		StartDate,
		EndDate,
		AddressId,
		CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifiedDate
	FROM
		[dbo].[Reunion]
	WHERE
		IsActive = 1
	ORDER BY StartDate, Name
RETURN 0
