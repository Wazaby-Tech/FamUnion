CREATE PROCEDURE [dbo].[spGetInvitesByReunionId]
	@reunionId UNIQUEIDENTIFIER
AS
	SELECT 
		[InviteId],
		[ReunionId], 
		[Email], 
		[Name], 
		[RsvpCount], 
		[ExpiresAt], 
		[Status]
	FROM [dbo].[ReunionInvite] (NOLOCK)
	WHERE [ReunionId] = @reunionId
	AND COALESCE([ExpiresAt], DATEADD(DAY, 1, SYSDATETIME())) >= SYSDATETIME()
	ORDER BY [Email]