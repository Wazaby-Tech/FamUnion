CREATE TYPE [dbo].[udfInviteType] AS TABLE
(
	ReunionId UNIQUEIDENTIFIER,
    Email NVARCHAR(255),
    Name NVARCHAR(255)
)
