CREATE PROCEDURE [dbo].[spGetInvitesByReunionId]
	@reunionId UNIQUEIDENTIFIER
AS
	SELECT 
		[InviteId],
		[ReunionId], 
		[Email], 
		[Name], 
		[RsvpCount],  
		[Status]
	FROM [dbo].[ReunionInvite] (NOLOCK)
	WHERE [ReunionId] = @reunionId
	ORDER BY [Email]