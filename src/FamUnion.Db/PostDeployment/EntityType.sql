;WITH EntityTypes AS
(
	SELECT i [EntityTypeId], n [EntityName]
	FROM
	(
		VALUES
		(1, 'Reunion'),
		(2, 'Event'),
		(3, 'Family')
	)a (i, n)
)
MERGE INTO [dbo].[EntityType] TARGET
USING EntityTypes SOURCE
ON SOURCE.EntityTypeId = TARGET.EntityTypeId
WHEN NOT MATCHED
	THEN
	INSERT (EntityTypeId, EntityName)
	VALUES (SOURCE.EntityTypeId, SOURCE.EntityName);