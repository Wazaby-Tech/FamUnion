CREATE PROCEDURE [dbo].[spGetUserById]
	@userId NVARCHAR(100)
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
	WHERE UserId = @userId