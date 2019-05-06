CREATE PROCEDURE [dbo].[spDeleteAddressById]
	@addressId uniqueidentifier
AS
	UPDATE [dbo].[Address]
	SET 
		IsActive = 0,
		ModifiedBy = SUSER_SNAME(),
		ModifiedDate = SYSDATETIME()
	WHERE
		AddressId = @addressId

RETURN 0
