using FamUnion.Api.Controllers;
using FamUnion.Core.Interface.Services;
using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using Xunit;

namespace FamUnion.Core.Tests.Controllers
{
    public class AttendeesControllerTests
    {
        private readonly Mock<ILogger<AttendeesController>> _logger = new();
        private readonly Mock<IAttendeeService> _attendeeService = new();

        private AttendeesController CreateController() =>
            new AttendeesController(_logger.Object, _attendeeService.Object);

        private static string ValidEncodedInvite()
        {
            var info = new InviteInfo
            {
                InviteId = Guid.NewGuid(),
                ReunionId = Guid.NewGuid(),
                ExpiresAt = DateTime.Today.AddDays(7),
                InviteEmail = "guest@example.com"
            };
            // Encode returns URL-encoded base64; controller receives URL-decoded value from ASP.NET routing
            return HttpUtility.UrlDecode(info.Encode());
        }

        [Fact]
        public async Task Invite_ReturnsOk_WhenInviteIsFound()
        {
            var encodedInvite = ValidEncodedInvite();
            var invite = new AttendeeInvite { ReunionId = Guid.NewGuid(), Name = "John Smith", Email = "guest@example.com" };
            _attendeeService.Setup(s => s.GetInviteAsync(It.IsAny<InviteInfo>())).ReturnsAsync(invite);

            var result = await CreateController().Invite(encodedInvite);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(invite, ok.Value);
        }

        [Fact]
        public async Task Invite_ReturnsNotFound_WhenInviteDoesNotExist()
        {
            var encodedInvite = ValidEncodedInvite();
            _attendeeService.Setup(s => s.GetInviteAsync(It.IsAny<InviteInfo>())).ReturnsAsync((AttendeeInvite)null);

            var result = await CreateController().Invite(encodedInvite);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Invite_Returns500_WhenExceptionIsThrown()
        {
            _attendeeService.Setup(s => s.GetInviteAsync(It.IsAny<InviteInfo>())).ThrowsAsync(new Exception("fail"));

            // A corrupted base64 string will throw inside InviteInfo.Decode
            var result = await CreateController().Invite("not-valid-base64!!!");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task AddInvites_ReturnsOk_WhenSuccessful()
        {
            var request = new BulkAttendeeRequest();
            _attendeeService.Setup(s => s.AddAttendees(request)).Returns(Task.CompletedTask);

            var result = await CreateController().AddInvites(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddInvites_Returns500_WhenServiceThrows()
        {
            var request = new BulkAttendeeRequest();
            _attendeeService.Setup(s => s.AddAttendees(request)).ThrowsAsync(new Exception("fail"));

            var result = await CreateController().AddInvites(request);

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetAttendeesByReunionId_ReturnsOk_WhenGuidIsValid()
        {
            var reunionId = Guid.NewGuid();
            var attendees = new List<AttendeeInvite> { new AttendeeInvite { Name = "John Smith", Email = "john@example.com" } };
            _attendeeService.Setup(s => s.GetAttendeesByReunion(reunionId)).ReturnsAsync(attendees);

            var result = await CreateController().GetAttendeesByReunionId(reunionId.ToString());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(attendees, ok.Value);
        }

        [Fact]
        public async Task GetAttendeesByReunionId_Returns500_WhenGuidIsInvalid()
        {
            var result = await CreateController().GetAttendeesByReunionId("not-a-guid");

            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }
    }
}
