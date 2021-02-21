CREATE PROCEDURE [dbo].[spGetManageReunions]
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
		AND CreatedBy = SUSER_SNAME()
	ORDER BY StartDate, Name
