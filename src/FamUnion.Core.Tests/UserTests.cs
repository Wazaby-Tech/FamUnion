using FamUnion.Core.Model;
using Xunit;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Tests
{
    public class UserTests
    {
        [Theory]
        [InlineData("auth0|user1", "user@example.com", UserAuthType.Auth0)]
        [InlineData("facebook|user1", "user@example.com", UserAuthType.Facebook)]
        [InlineData("google|user1", "user@example.com", UserAuthType.Google)]
        public void ValidUserPasses(string userId, string email, UserAuthType authType)
        {
            var user = new User { UserId = userId, Email = email, AuthType = authType };
            Assert.True(user.IsValid());
        }

        [Theory]
        [InlineData("", "user@example.com", UserAuthType.Auth0)]
        [InlineData("   ", "user@example.com", UserAuthType.Auth0)]
        [InlineData("auth0|user1", "", UserAuthType.Auth0)]
        [InlineData("auth0|user1", "   ", UserAuthType.Auth0)]
        [InlineData("auth0|user1", "user@example.com", UserAuthType.Unauthorized)]
        public void InvalidUserFails(string userId, string email, UserAuthType authType)
        {
            var user = new User { UserId = userId, Email = email, AuthType = authType };
            Assert.False(user.IsValid());
        }

        [Fact]
        public void DefaultUserIsInvalid()
        {
            var user = new User();
            Assert.False(user.IsValid());
        }
    }
}
