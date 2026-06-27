using FamUnion.Core.Interface;
using FamUnion.Core.Interface.Repository;
using FamUnion.Core.Interface.Services;
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
    public class ReunionServiceTests
    {
        private readonly Mock<IReunionRepository> _reunionRepo = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IUserAccessService> _userAccessService = new();
        private readonly Mock<IAddressService> _addressService = new();
        private readonly Mock<IEventService> _eventService = new();

        private ReunionService CreateService() =>
            new ReunionService(_reunionRepo.Object, _userRepo.Object,
                _userAccessService.Object, _addressService.Object, _eventService.Object);

        [Fact]
        public async Task GetReunionAsync_ReturnsReunionFromRepository()
        {
            var id = Guid.NewGuid();
            var expected = new Reunion { Id = id, Name = "Smith Reunion" };
            _reunionRepo.Setup(r => r.GetReunionAsync(id)).ReturnsAsync(expected);
            _userRepo.Setup(r => r.GetReunionOrganizers(id)).ReturnsAsync(new List<User>());
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync((Address)null);
            _eventService.Setup(s => s.GetEventsByReunionIdAsync(id)).ReturnsAsync(new List<Event>());

            var result = await CreateService().GetReunionAsync(id);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetManageReunionsAsync_DelegatesToRepository()
        {
            const string userId = "auth0|user1";
            var expected = new List<Reunion> { new Reunion { Id = Guid.NewGuid(), Name = "Smith Reunion" } };
            _reunionRepo.Setup(r => r.GetManageReunionsAsync(userId)).ReturnsAsync(expected);
            _userRepo.Setup(r => r.GetReunionOrganizers(It.IsAny<Guid>())).ReturnsAsync(new List<User>());
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync((Address)null);
            _eventService.Setup(s => s.GetEventsByReunionIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Event>());

            var result = await CreateService().GetManageReunionsAsync(userId);

            _reunionRepo.Verify(r => r.GetManageReunionsAsync(userId), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task SaveReunionAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            var reunion = new Reunion { Name = "Test", ActionUserId = "bad-user" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("bad-user")).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => CreateService().SaveReunionAsync(reunion));
        }

        [Fact]
        public async Task SaveReunionAsync_ThrowsUnauthorized_WhenUserLacksWriteAccess()
        {
            var reunionId = Guid.NewGuid();
            var reunion = new Reunion { Id = reunionId, Name = "Test", ActionUserId = "auth0|user1" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessService.Setup(s => s.HasWriteAccessToEntity("auth0|user1", EntityType.Reunion, reunionId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => CreateService().SaveReunionAsync(reunion));
        }

        [Fact]
        public async Task SaveReunionAsync_Succeeds_ForNewReunion_WithNullId()
        {
            var savedId = Guid.NewGuid();
            var reunion = new Reunion { Id = null, Name = "New Reunion", ActionUserId = "auth0|user1" };
            var savedReunion = new Reunion { Id = savedId, Name = "New Reunion", ActionUserId = "auth0|user1" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _reunionRepo.Setup(r => r.SaveReunionAsync(reunion)).ReturnsAsync(savedReunion);
            _reunionRepo.Setup(r => r.GetReunionAsync(savedId)).ReturnsAsync(savedReunion);
            _userRepo.Setup(r => r.GetReunionOrganizers(savedId)).ReturnsAsync(new List<User>());
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync((Address)null);
            _eventService.Setup(s => s.GetEventsByReunionIdAsync(savedId)).ReturnsAsync(new List<Event>());

            // Null ID → CheckUserWriteAccess returns true without calling userAccessService
            var result = await CreateService().SaveReunionAsync(reunion);

            Assert.Equal(savedId, result.Id);
            _userAccessService.Verify(s => s.HasWriteAccessToEntity(It.IsAny<string>(), It.IsAny<EntityType>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task SaveReunionAsync_SavesAddress_WhenLocationIsProvided()
        {
            var savedId = Guid.NewGuid();
            var location = new Address { Description = "Park", City = "Atlanta", State = "GA" };
            var reunion = new Reunion { Id = null, Name = "Test", ActionUserId = "auth0|user1", Location = location };
            var savedReunion = new Reunion { Id = savedId, Name = "Test" };
            var savedAddress = new Address { Id = Guid.NewGuid(), Description = "Park", City = "Atlanta", State = "GA" };

            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _reunionRepo.Setup(r => r.SaveReunionAsync(reunion)).ReturnsAsync(savedReunion);
            _reunionRepo.Setup(r => r.GetReunionAsync(savedId)).ReturnsAsync(savedReunion);
            _addressService.Setup(s => s.SaveEntityAddressAsync(It.IsAny<SaveEntityAddressRequest>())).ReturnsAsync(savedAddress);
            _addressService.Setup(s => s.GetEntityAddressAsync(It.IsAny<GetEntityAddressRequest>())).ReturnsAsync(savedAddress);
            _userRepo.Setup(r => r.GetReunionOrganizers(savedId)).ReturnsAsync(new List<User>());
            _eventService.Setup(s => s.GetEventsByReunionIdAsync(savedId)).ReturnsAsync(new List<Event>());

            await CreateService().SaveReunionAsync(reunion);

            _addressService.Verify(s => s.SaveEntityAddressAsync(It.IsAny<SaveEntityAddressRequest>()), Times.Once);
        }

        [Fact]
        public async Task CancelReunionAsync_ThrowsException_WhenUserIdIsInvalid()
        {
            var request = new CancelRequest { EntityId = Guid.NewGuid(), UserId = "bad-user" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("bad-user")).ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => CreateService().CancelReunionAsync(request));
        }

        [Fact]
        public async Task CancelReunionAsync_ThrowsUnauthorized_WhenUserLacksWriteAccess()
        {
            var reunionId = Guid.NewGuid();
            var request = new CancelRequest { EntityId = reunionId, UserId = "auth0|user1" };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessService.Setup(s => s.HasWriteAccessToEntity("auth0|user1", EntityType.Reunion, reunionId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => CreateService().CancelReunionAsync(request));
        }

        [Fact]
        public async Task AddReunionOrganizer_ThrowsException_WhenActionIsNotAdd()
        {
            var request = OrganizerRequest.RemoveOrganizerRequest(Guid.NewGuid(), "user@example.com", "auth0|user1");

            await Assert.ThrowsAsync<Exception>(() => CreateService().AddReunionOrganizer(request));
        }

        [Fact]
        public async Task RemoveReunionOrganizer_ThrowsException_WhenActionIsNotRemove()
        {
            var request = OrganizerRequest.AddOrganizerRequest(Guid.NewGuid(), "user@example.com", "auth0|user1");

            await Assert.ThrowsAsync<Exception>(() => CreateService().RemoveReunionOrganizer(request));
        }

        [Fact]
        public async Task GetReunionOrganizers_ThrowsException_WhenActionIsNotList()
        {
            var request = OrganizerRequest.AddOrganizerRequest(Guid.NewGuid(), "user@example.com", "auth0|user1");

            await Assert.ThrowsAsync<Exception>(() => CreateService().GetReunionOrganizers(request));
        }

        [Fact]
        public async Task GetReunionOrganizers_ThrowsUnauthorized_WhenUserLacksWriteAccess()
        {
            var reunionId = Guid.NewGuid();
            var request = new OrganizerRequest
            {
                ReunionId = reunionId,
                Action = OrganizerAction.List,
                ActionUserId = "auth0|user1"
            };
            _userRepo.Setup(r => r.ValidateUserIdAsync("auth0|user1")).ReturnsAsync(true);
            _userAccessService.Setup(s => s.HasWriteAccessToEntity("auth0|user1", EntityType.Reunion, reunionId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => CreateService().GetReunionOrganizers(request));
        }

        [Fact]
        public async Task AddReunionOrganizer_FirstOrganizer_BypassesAccessCheck_WhenEmailMatchesActionUser()
        {
            var reunionId = Guid.NewGuid();
            const string userId = "auth0|user1";
            var request = OrganizerRequest.AddOrganizerRequest(reunionId, userId, userId);
            var organizer = new User { UserId = userId, Email = "user@example.com" };

            _userRepo.Setup(r => r.GetReunionOrganizers(reunionId)).ReturnsAsync(new List<User>());
            _userRepo.Setup(r => r.ValidateUserIdAsync(userId)).ReturnsAsync(true);
            _userRepo.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(organizer);
            _reunionRepo.Setup(r => r.AddReunionOrganizer(reunionId, organizer.Email)).Returns(Task.CompletedTask);

            await CreateService().AddReunionOrganizer(request);

            _reunionRepo.Verify(r => r.AddReunionOrganizer(reunionId, organizer.Email), Times.Once);
            _userAccessService.Verify(s => s.HasWriteAccessToEntity(It.IsAny<string>(), It.IsAny<EntityType>(), It.IsAny<Guid>()), Times.Never);
        }
    }
}
