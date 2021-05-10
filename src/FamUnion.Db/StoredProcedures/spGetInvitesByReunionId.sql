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
	AND [ExpiresAt] >= SYSDATETIME()
	ORDER BY [Email]