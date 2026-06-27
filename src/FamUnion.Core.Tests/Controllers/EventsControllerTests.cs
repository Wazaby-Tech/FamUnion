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

namespace FamUnion.Core.Tests.Controllers
{
    public class EventsControllerTests
    {
        private readonly Mock<IEventService> _service = new();
        private readonly Mock<ILogger<EventsController>> _logger = new();

        private EventsController CreateController() =>
            new EventsController(_logger.Object, _service.Object);

        [Fact]
        public async Task GetEventById_ReturnsOk_WhenEventExists()
        {
            var id = Guid.NewGuid();
            var @event = new Event { Id = id, Name = "BBQ" };
            _service.Setup(s => s.GetEventByIdAsync(id)).ReturnsAsync(@event);

            var result = await CreateController().GetEventById(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(@event, ok.Value);
        }

        [Fact]
        public async Task GetEventById_ReturnsNotFound_WhenEventIsNull()
        {
            var id = Guid.NewGuid();
            _service.Setup(s => s.GetEventByIdAsync(id)).ReturnsAsync((Event)null);

            var result = await CreateController().GetEventById(id);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetEventById_Returns401_WhenUnauthorized()
        {
            var id = Guid.NewGuid();
            _service.Setup(s => s.GetEventByIdAsync(id)).ThrowsAsync(new UnauthorizedAccessException());

            var result = await CreateController().GetEventById(id);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(401, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetEventById_Returns500_WhenServiceThrows()
        {
            var id = Guid.NewGuid();
            _service.Setup(s => s.GetEventByIdAsync(id)).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().GetEventById(id);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetEventsByReunion_ReturnsOk_WithList()
        {
            var reunionId = Guid.NewGuid();
            var events = new List<Event> { new Event { Id = Guid.NewGuid(), Name = "BBQ" } };
            _service.Setup(s => s.GetEventsByReunionIdAsync(reunionId)).ReturnsAsync(events);

            var result = await CreateController().GetEventsByReunion(reunionId);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(events, ok.Value);
        }

        [Fact]
        public async Task GetEventsByReunion_Returns401_WhenUnauthorized()
        {
            var reunionId = Guid.NewGuid();
            _service.Setup(s => s.GetEventsByReunionIdAsync(reunionId)).ThrowsAsync(new UnauthorizedAccessException());

            var result = await CreateController().GetEventsByReunion(reunionId);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(401, statusResult.StatusCode);
        }

        [Fact]
        public async Task SaveEvent_ReturnsOk_WhenSuccessful()
        {
            var @event = new Event { Id = Guid.NewGuid(), Name = "BBQ", ReunionId = Guid.NewGuid() };
            _service.Setup(s => s.SaveEventAsync(@event)).ReturnsAsync(@event);

            var result = await CreateController().SaveEvent(@event);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(@event, ok.Value);
        }

        [Fact]
        public async Task SaveEvent_Returns401_WhenUnauthorized()
        {
            var @event = new Event { Id = Guid.NewGuid(), Name = "BBQ" };
            _service.Setup(s => s.SaveEventAsync(@event)).ThrowsAsync(new UnauthorizedAccessException());

            var result = await CreateController().SaveEvent(@event);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(401, statusResult.StatusCode);
        }

        [Fact]
        public async Task SaveEvent_ReturnsBadRequest_WhenEventIsInvalid()
        {
            // Invalid event: empty name
            var @event = new Event { Name = "" };
            _service.Setup(s => s.SaveEventAsync(@event)).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().SaveEvent(@event);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CancelEvent_ReturnsOk_WhenSuccessful()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "auth0|user1" };
            _service.Setup(s => s.CancelEventAsync(request)).Returns(Task.CompletedTask);

            var result = await CreateController().CancelEvent(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CancelEvent_Returns401_WhenUnauthorized()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "auth0|user1" };
            _service.Setup(s => s.CancelEventAsync(request)).ThrowsAsync(new UnauthorizedAccessException());

            var result = await CreateController().CancelEvent(request);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(401, statusResult.StatusCode);
        }

        [Fact]
        public async Task CancelEvent_Returns500_WhenServiceThrows()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "auth0|user1" };
            _service.Setup(s => s.CancelEventAsync(request)).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().CancelEvent(request);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
