CREATE PROCEDURE [dbo].[spDeleteReunionById]
	@reunionId uniqueidentifier
AS
	UPDATE a
	SET
		a.IsActive = 0,
		a.ModifiedBy = SUSER_SNAME(),
		a.ModifiedDate = SYSDATETIME()
	FROM [dbo].[Address] a
	JOIN [dbo].[EntityType] et on a.EntityType = et.EntityTypeId
	WHERE
		et.EntityName = 'Reunion' AND
		a.EntityId = @reunionId


	UPDATE [dbo].[Reunion]
	SET
		IsActive = 0,
		ModifiedBy = SUSER_SNAME(),
		ModifiedDate = SYSDATETIME()
	WHERE
		ReunionId = @reunionId
