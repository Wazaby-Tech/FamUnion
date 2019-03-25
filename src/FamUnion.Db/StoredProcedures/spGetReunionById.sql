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
		[dbo].[Reunion]
	WHERE
		ReunionId = @id

RETURN 0
