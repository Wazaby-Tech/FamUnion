CREATE PROCEDURE [dbo].[spGetAddressByReunionId]
	@reunionId uniqueidentifier
AS
	DECLARE @reunionEntityTypeId INT = (SELECT EntityTypeId FROM [dbo].[EntityType] WHERE EntityName = 'Reunion')
	
	EXEC [dbo].[spGetAddressByEntityTypeAndId] @entityTypeId = @reunionEntityTypeId, @entityId = @reunionId
