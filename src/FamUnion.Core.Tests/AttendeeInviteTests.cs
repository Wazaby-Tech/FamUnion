using FamUnion.Core.Model;
using System;
using Xunit;

namespace FamUnion.Core.Tests
{
    public class AttendeeInviteTests
    {
        [Fact]
        public void ValidInvitePasses()
        {
            var invite = new AttendeeInvite
            {
                ReunionId = Guid.NewGuid(),
                Name = "John Smith",
                Email = "john@example.com"
            };
            Assert.True(invite.IsValid());
        }

        [Fact]
        public void EmptyReunionIdFails()
        {
            var invite = new AttendeeInvite
            {
                ReunionId = Guid.Empty,
                Name = "John Smith",
                Email = "john@example.com"
            };
            Assert.False(invite.IsValid());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void BlankNameFails(string name)
        {
            var invite = new AttendeeInvite
            {
                ReunionId = Guid.NewGuid(),
                Name = name,
                Email = "john@example.com"
            };
            Assert.False(invite.IsValid());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void BlankEmailFails(string email)
        {
            var invite = new AttendeeInvite
            {
                ReunionId = Guid.NewGuid(),
                Name = "John Smith",
                Email = email
            };
            Assert.False(invite.IsValid());
        }

        [Fact]
        public void DefaultInviteIsInvalid()
        {
            var invite = new AttendeeInvite();
            Assert.False(invite.IsValid());
        }
    }
}
