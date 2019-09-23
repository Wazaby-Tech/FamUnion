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
		[dbo].[Reunion] (NOLOCK)
	WHERE
		IsActive = 1
	ORDER BY StartDate, Name
