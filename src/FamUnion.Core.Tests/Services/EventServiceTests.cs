using FamUnion.Core.Interface;
using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<ILogger<EventService>> _logger = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IUserAccessRepository> _userAccessRepo = new();
        private readonly Mock<IEventRepository> _eventRepo = new();
        private readonly Mock<IAddressService> _addressService = new();

        private EventService CreateService() =>
            new EventService(_logger.Object, _userRepo.Object,
                _userAccessRepo.Object, _eventRepo.Object, _addressService.Object);

        [Fact]
        public async Task GetEventByIdAsync_ReturnsEventFromRepository()
        {
            var eventId = Guid.NewGuid();
            var expected = new Event { Id = eventId, Name = "BBQ" };
            _eventRepo.Setup(r => r.GetEventAsync(eventId)).ReturnsAsync(expected);
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync((Address)null);

            var result = await CreateService().GetEventByIdAsync(eventId);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetEventsByReunionIdAsync_ReturnsEventsFromRepository()
        {
            var reunionId = Guid.NewGuid();
            var expected = new List<Event> { new Event { Id = Guid.NewGuid(), Name = "BBQ" } };
            _eventRepo.Setup(r => r.GetEventsByReunionIdAsync(reunionId)).ReturnsAsync(expected);
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync((Address)null);

            var result = await CreateService().GetEventsByReunionIdAsync(reunionId);

            Assert.Same(expected, result);
        }

        [Fact]
        public async Task SaveEventAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            var @event = new Event { Id = Guid.NewGuid(), Name = "BBQ", ActionUserId = "bad-user" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("bad-user")).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => CreateService().SaveEventAsync(@event));
        }

        [Fact]
        public async Task SaveEventAsync_ThrowsUnauthorized_WhenUserLacksWriteAccess()
        {
            var eventId = Guid.NewGuid();
            var @event = new Event { Id = eventId, Name = "BBQ", ActionUserId = "auth0|user1" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessRepo.Setup(r => r.HasWriteAccessToEntity("auth0|user1", EntityType.Event, eventId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => CreateService().SaveEventAsync(@event));
        }

        [Fact]
        public async Task SaveEventAsync_SavesAddress_WhenLocationIsProvided()
        {
            var eventId = Guid.NewGuid();
            var location = new Address { Description = "Park", City = "Atlanta", State = "GA" };
            var @event = new Event { Id = eventId, Name = "BBQ", ActionUserId = "auth0|user1", Location = location };
            var savedEvent = new Event { Id = eventId, Name = "BBQ" };
            var savedAddress = new Address { Id = Guid.NewGuid(), Description = "Park", City = "Atlanta", State = "GA" };

            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessRepo.Setup(r => r.HasWriteAccessToEntity("auth0|user1", EntityType.Event, eventId)).ReturnsAsync(true);
            _eventRepo.Setup(r => r.SaveEventAsync(@event)).ReturnsAsync(savedEvent);
            _eventRepo.Setup(r => r.GetEventAsync(eventId)).ReturnsAsync(savedEvent);
            _addressService.Setup(s => s.SaveEntityAddressAsync(It.IsAny<SaveEntityAddressRequest>())).ReturnsAsync(savedAddress);
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync(savedAddress);

            await CreateService().SaveEventAsync(@event);

            _addressService.Verify(s => s.SaveEntityAddressAsync(It.IsAny<SaveEntityAddressRequest>()), Times.Once);
        }

        [Fact]
        public async Task CancelEventAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "bad-user" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("bad-user")).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => CreateService().CancelEventAsync(request));
        }

        [Fact]
        public async Task CancelEventAsync_ThrowsUnauthorized_WhenUserLacksWriteAccess()
        {
            var eventId = Guid.NewGuid();
            var request = new CancelRequest { EntityId = eventId, UserId = "auth0|user1" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessRepo.Setup(r => r.HasWriteAccessToEntity("auth0|user1", EntityType.Event, eventId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => CreateService().CancelEventAsync(request));
        }

        [Fact]
        public async Task CancelEventAsync_Succeeds_WhenUserHasWriteAccess()
        {
            var eventId = Guid.NewGuid();
            var request = new CancelRequest { EntityId = eventId, UserId = "auth0|user1" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessRepo.Setup(r => r.HasWriteAccessToEntity("auth0|user1", EntityType.Event, eventId))
                .ReturnsAsync(true);
            _eventRepo.Setup(r => r.CancelEventAsync(request)).Returns(Task.CompletedTask);

            await CreateService().CancelEventAsync(request);

            _eventRepo.Verify(r => r.CancelEventAsync(request), Times.Once);
        }
    }
}
