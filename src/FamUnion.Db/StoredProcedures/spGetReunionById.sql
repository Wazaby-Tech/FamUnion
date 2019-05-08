CREATE PROCEDURE [dbo].[spGetReunionById]
	@id uniqueidentifier
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
		ReunionId = @id
		AND IsActive = 1

RETURN 0
