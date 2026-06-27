using FamUnion.Api.Controllers;
using FamUnion.Core.Interface;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Tests.Controllers
{
    public class ReunionsControllerTests
    {
        private readonly Mock<IReunionService> _service = new();
        private readonly Mock<ILogger<ReunionsController>> _logger = new();

        private ReunionsController CreateController() =>
            new ReunionsController(_service.Object, _logger.Object);

        [Fact]
        public async Task GetReunions_ReturnsOk_WithList()
        {
            var reunions = new List<Reunion> { new Reunion { Id = Guid.NewGuid(), Name = "Smith Reunion" } };
            _service.Setup(s => s.GetReunionsAsync()).ReturnsAsync(reunions);

            var result = await CreateController().GetReunions();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(reunions, ok.Value);
        }

        [Fact]
        public async Task GetReunions_Returns500_WhenServiceThrows()
        {
            _service.Setup(s => s.GetReunionsAsync()).ThrowsAsync(new Exception("db down"));

            var result = await CreateController().GetReunions();

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetManageReunions_ReturnsOk_WithList()
        {
            const string userId = "auth0|user1";
            var reunions = new List<Reunion> { new Reunion { Id = Guid.NewGuid(), Name = "Smith Reunion" } };
            _service.Setup(s => s.GetManageReunionsAsync(userId)).ReturnsAsync(reunions);

            var result = await CreateController().GetManageReunions(userId);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(reunions, ok.Value);
        }

        [Fact]
        public async Task GetManageReunions_Returns500_WhenServiceThrows()
        {
            _service.Setup(s => s.GetManageReunionsAsync(It.IsAny<string>())).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().GetManageReunions("user1");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetReunion_ReturnsOk_WhenReunionExists()
        {
            var id = Guid.NewGuid();
            var reunion = new Reunion { Id = id, Name = "Smith Reunion" };
            _service.Setup(s => s.GetReunionAsync(id)).ReturnsAsync(reunion);

            var result = await CreateController().GetReunion(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(reunion, ok.Value);
        }

        [Fact]
        public async Task GetReunion_ReturnsNotFound_WhenReunionIsNull()
        {
            var id = Guid.NewGuid();
            _service.Setup(s => s.GetReunionAsync(id)).ReturnsAsync((Reunion)null);

            var result = await CreateController().GetReunion(id);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetReunion_Returns500_WhenServiceThrows()
        {
            var id = Guid.NewGuid();
            _service.Setup(s => s.GetReunionAsync(id)).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().GetReunion(id);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task NewReunion_ReturnsBadRequest_WhenReunionIsInvalid()
        {
            // Empty name makes the reunion invalid after mapping
            var request = new NewReunionRequest
            {
                Name = "",
                UserId = "auth0|user1",
                StartDate = new DateTime(2024, 7, 4)
            };

            var result = await CreateController().NewReunion(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task NewReunion_ReturnsCreated_WhenValid()
        {
            var savedId = Guid.NewGuid();
            var request = new NewReunionRequest
            {
                Name = "Smith Reunion",
                UserId = "auth0|user1",
                StartDate = new DateTime(2024, 7, 4),
                EndDate = new DateTime(2024, 7, 6)
            };
            var savedReunion = new Reunion { Id = savedId, Name = "Smith Reunion" };
            _service.Setup(s => s.SaveReunionAsync(It.IsAny<Reunion>())).ReturnsAsync(savedReunion);
            _service.Setup(s => s.AddReunionOrganizer(It.IsAny<OrganizerRequest>())).Returns(Task.CompletedTask);

            var result = await CreateController().NewReunion(request);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task SaveReunion_Returns500_WhenReunionHasNoId()
        {
            var reunion = new Reunion { Id = null, Name = "Smith Reunion", ActionUserId = "auth0|user1" };

            var result = await CreateController().SaveReunion(reunion);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task SaveReunion_ReturnsOk_WhenValid()
        {
            var id = Guid.NewGuid();
            var reunion = new Reunion { Id = id, Name = "Smith Reunion", ActionUserId = "auth0|user1" };
            _service.Setup(s => s.SaveReunionAsync(reunion)).ReturnsAsync(reunion);

            var result = await CreateController().SaveReunion(reunion);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(reunion, ok.Value);
        }

        [Fact]
        public async Task CancelReunion_ReturnsOk_WhenSuccessful()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "auth0|user1" };
            _service.Setup(s => s.CancelReunionAsync(request)).Returns(Task.CompletedTask);

            var result = await CreateController().CancelReunion(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CancelReunion_Returns500_WhenServiceThrows()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "auth0|user1" };
            _service.Setup(s => s.CancelReunionAsync(request)).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().CancelReunion(request);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task OrganizerOperations_ReturnsOk_ForListAction()
        {
            var reunionId = Guid.NewGuid();
            var request = new OrganizerRequest { ReunionId = reunionId, Action = OrganizerAction.List, ActionUserId = "auth0|user1" };
            var organizers = new List<User> { new User { UserId = "auth0|user1" } };
            _service.Setup(s => s.GetReunionOrganizers(request)).ReturnsAsync(organizers);

            var result = await CreateController().OrganizerOperations(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(organizers, ok.Value);
        }

        [Fact]
        public async Task OrganizerOperations_ReturnsOk_ForAddAction()
        {
            var request = OrganizerRequest.AddOrganizerRequest(Guid.NewGuid(), "user@example.com", "auth0|user1");
            _service.Setup(s => s.AddReunionOrganizer(request)).Returns(Task.CompletedTask);

            var result = await CreateController().OrganizerOperations(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task OrganizerOperations_ReturnsOk_ForRemoveAction()
        {
            var request = OrganizerRequest.RemoveOrganizerRequest(Guid.NewGuid(), "user@example.com", "auth0|user1");
            _service.Setup(s => s.RemoveReunionOrganizer(request)).Returns(Task.CompletedTask);

            var result = await CreateController().OrganizerOperations(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task OrganizerOperations_Returns500_ForUnrecognizedAction()
        {
            var request = new OrganizerRequest { ReunionId = Guid.NewGuid(), Action = (OrganizerAction)99 };

            var result = await CreateController().OrganizerOperations(request);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
