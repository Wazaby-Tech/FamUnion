CREATE TABLE [dbo].[ReunionInvite]
(
	[ReunionId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_ReunionInvite_Reunion FOREIGN KEY REFERENCES [dbo].[Reunion]([ReunionId]),
	[Email] NVARCHAR(255) NOT NULL,
	[Name] NVARCHAR(255),
	[Status] INT NOT NULL CONSTRAINT DF_ReunionInvite_Status DEFAULT (0),
	CONSTRAINT PK_ReunionInvite PRIMARY KEY ([ReunionId],[Email])
)
