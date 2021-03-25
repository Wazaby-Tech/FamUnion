﻿CREATE TABLE [dbo].[ReunionOrganizer]
(
	[ReunionId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_Reunion_ReunionId FOREIGN KEY REFERENCES [dbo].[Reunion]([ReunionId]),
	[UserId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_User_UserId FOREIGN KEY REFERENCES [dbo].[User]([Id]),
	[IsActive] BIT NOT NULL CONSTRAINT DF_ReunionOrganizer_IsActive DEFAULT(1)
)
