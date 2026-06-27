using FamUnion.Api.Controllers;
using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<ILogger<UsersController>> _logger = new();

        private UsersController CreateController() =>
            new UsersController(_userRepo.Object, _logger.Object);

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task FindUserByEmail_ReturnsBadRequest_WhenEmailIsBlank(string email)
        {
            var result = await CreateController().FindUserByEmail(email);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task FindUserByEmail_ReturnsOkWithEmptyUser_WhenUserNotFound()
        {
            _userRepo.Setup(r => r.GetUserByEmailAsync("notfound@example.com")).ReturnsAsync((User)null);

            var result = await CreateController().FindUserByEmail("notfound@example.com");

            var ok = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(ok.Value);
            Assert.Equal("notfound@example.com", user.Email);
        }

        [Fact]
        public async Task FindUserByEmail_ReturnsOkWithUser_WhenUserExists()
        {
            var user = new User { UserId = "auth0|user1", Email = "found@example.com", AuthType = UserAuthType.Auth0 };
            _userRepo.Setup(r => r.GetUserByEmailAsync("found@example.com")).ReturnsAsync(user);

            var result = await CreateController().FindUserByEmail("found@example.com");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(user, ok.Value);
        }

        [Fact]
        public async Task FindUserByEmail_Returns500_WhenRepositoryThrows()
        {
            _userRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ThrowsAsync(new Exception("db error"));

            var result = await CreateController().FindUserByEmail("user@example.com");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetUserById_ReturnsBadRequest_WhenIdIsBlank(string id)
        {
            var result = await CreateController().GetUserById(id);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkWithEmptyUser_WhenUserNotFound()
        {
            _userRepo.Setup(r => r.GetUserByIdAsync("auth0|missing")).ReturnsAsync((User)null);

            var result = await CreateController().GetUserById("auth0|missing");

            var ok = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(ok.Value);
            Assert.Equal("auth0|missing", user.UserId);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkWithUser_WhenUserExists()
        {
            var user = new User { UserId = "auth0|user1", Email = "user@example.com", AuthType = UserAuthType.Auth0 };
            _userRepo.Setup(r => r.GetUserByIdAsync("auth0|user1")).ReturnsAsync(user);

            var result = await CreateController().GetUserById("auth0|user1");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(user, ok.Value);
        }

        [Fact]
        public async Task SaveUser_ReturnsBadRequest_WhenUserIsInvalid()
        {
            // User with Unauthorized auth type is invalid
            var user = new User { UserId = "auth0|user1", Email = "user@example.com", AuthType = UserAuthType.Unauthorized };

            var result = await CreateController().SaveUser(user);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task SaveUser_ReturnsOk_WhenUserIsValid()
        {
            var user = new User { UserId = "auth0|user1", Email = "user@example.com", AuthType = UserAuthType.Auth0 };
            _userRepo.Setup(r => r.SaveUserAsync(user)).ReturnsAsync(user);

            var result = await CreateController().SaveUser(user);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(user, ok.Value);
        }

        [Fact]
        public async Task SaveUser_Returns500_WhenRepositoryThrows()
        {
            var user = new User { UserId = "auth0|user1", Email = "user@example.com", AuthType = UserAuthType.Auth0 };
            _userRepo.Setup(r => r.SaveUserAsync(user)).ThrowsAsync(new Exception("db error"));

            var result = await CreateController().SaveUser(user);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
