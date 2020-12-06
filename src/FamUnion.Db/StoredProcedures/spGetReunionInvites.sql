CREATE PROCEDURE [dbo].[spGetReunionInvites]
	@reunionId UNIQUEIDENTIFIER
AS
	SELECT [ReunionId], [Email], [Name], [Status]
	FROM [dbo].[ReunionInvite] (NOLOCK)
	WHERE [ReunionId] = @reunionId
	ORDER BY [Email]
