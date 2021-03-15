;WITH EntityTypes AS
(
	SELECT i [EntityTypeId], n [EntityName], ia [IsActive]
	FROM
	(
		VALUES
		(1, 'Reunion', 1),
		(2, 'Event', 1),
		(3, 'Family', 0),
		(4, 'Lodging', 1)
	)a (i, n, ia)
)
MERGE INTO [dbo].[EntityType] TARGET
USING EntityTypes SOURCE
ON SOURCE.EntityTypeId = TARGET.EntityTypeId
WHEN NOT MATCHED
	THEN
	INSERT (EntityTypeId, EntityName, IsActive)
	VALUES (SOURCE.EntityTypeId, SOURCE.EntityName, SOURCE.IsActive)
WHEN MATCHED
	THEN
	UPDATE SET TARGET.IsActive = SOURCE.IsActive;