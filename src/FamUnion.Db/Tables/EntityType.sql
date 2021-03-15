CREATE TABLE [dbo].[EntityType]
(
	[EntityTypeId] INT NOT NULL PRIMARY KEY,
	[EntityName] NVARCHAR(255) NOT NULL,
	[IsActive] BIT NOT NULL CONSTRAINT DF_EntitType_IsActive DEFAULT 1
)
