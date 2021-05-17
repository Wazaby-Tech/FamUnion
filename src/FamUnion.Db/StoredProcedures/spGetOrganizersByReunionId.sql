CREATE PROCEDURE [dbo].[spGetOrganizersByReunionId]
	@reunionId UNIQUEIDENTIFIER
AS
	SELECT
		u.UserId,
		u.FirstName,
		u.LastName,
		u.Email,
		u.PhoneNumber,
		u.AuthType
	FROM [dbo].[ReunionOrganizer] ro (NOLOCK)
		INNER JOIN [dbo].[User] u (NOLOCK) ON ro.UserId = u.Id AND ro.IsActive = 1
	WHERE ReunionId = @reunionId
