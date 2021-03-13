CREATE PROCEDURE [dbo].[spGetUserByEmail]
	@email NVARCHAR(255)
AS
	SELECT 
		Id,
		UserId,
		FirstName,
		LastName,
		Email,
		PhoneNumber,
		AuthType
	FROM [dbo].[User]
	WHERE Email = @email