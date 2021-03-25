CREATE PROCEDURE [dbo].[spGetManageReunions]
	@userId NVARCHAR(100)
AS
	SELECT 
		r.ReunionId [Id],
		Name,
		Description,
		StartDate,
		EndDate,
		AddressId,
		r.CreatedBy,
		r.CreatedDate,
		r.ModifiedBy,
		r.ModifiedDate
	FROM
		[dbo].[Reunion] r (NOLOCK)
	JOIN [dbo].[ReunionOrganizer] ro (NOLOCK)
		ON ro.ReunionId = r.ReunionId
	JOIN [dbo].[User] u (NOLOCK)
		ON ro.UserId = u.Id
	WHERE
		r.IsActive = 1
		AND u.UserId = @userId
	ORDER BY StartDate, Name
