CREATE PROCEDURE [dbo].[spUpdateInviteStatus]
	@inviteId UNIQUEIDENTIFIER,
	@status INT
AS
	UPDATE [dbo].[ReunionInvite]
	SET
		[Status] = @status,
		[ModifiedDate] = SYSDATETIME(),
		[ModifiedBy] = SUSER_SNAME()
	WHERE
		[InviteId] = @inviteId