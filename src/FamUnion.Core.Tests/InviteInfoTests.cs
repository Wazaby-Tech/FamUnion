using FamUnion.Core.Utility;
using System;
using System.Web;
using Xunit;

namespace FamUnion.Core.Tests
{
    public class InviteInfoTests
    {
        [Fact]
        public void ValidInviteInfoPasses()
        {
            var info = new InviteInfo
            {
                InviteId = Guid.NewGuid(),
                ReunionId = Guid.NewGuid(),
                ExpiresAt = DateTime.Today.AddDays(7),
                InviteEmail = "guest@example.com"
            };
            Assert.True(info.IsValid());
        }

        [Fact]
        public void EmptyReunionIdFails()
        {
            var info = new InviteInfo
            {
                InviteId = Guid.NewGuid(),
                ReunionId = Guid.Empty,
                ExpiresAt = DateTime.Today.AddDays(7),
                InviteEmail = "guest@example.com"
            };
            Assert.False(info.IsValid());
        }

        [Fact]
        public void DefaultExpiresAtFails()
        {
            var info = new InviteInfo
            {
                InviteId = Guid.NewGuid(),
                ReunionId = Guid.NewGuid(),
                ExpiresAt = DateTime.MinValue,
                InviteEmail = "guest@example.com"
            };
            Assert.False(info.IsValid());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void BlankEmailFails(string email)
        {
            var info = new InviteInfo
            {
                InviteId = Guid.NewGuid(),
                ReunionId = Guid.NewGuid(),
                ExpiresAt = DateTime.Today.AddDays(7),
                InviteEmail = email
            };
            Assert.False(info.IsValid());
        }

        [Fact]
        public void DefaultInviteInfoIsInvalid()
        {
            var info = new InviteInfo();
            Assert.False(info.IsValid());
        }

        [Fact]
        public void EncodeDecodeRoundTrip()
        {
            var original = new InviteInfo
            {
                InviteId = Guid.NewGuid(),
                ReunionId = Guid.NewGuid(),
                ExpiresAt = DateTime.Today.AddDays(7),
                InviteEmail = "roundtrip@example.com"
            };

            // Encode produces URL-encoded base64; ASP.NET URL-decodes path params before reaching the controller
            string encoded = original.Encode();
            string rawBase64 = HttpUtility.UrlDecode(encoded);
            var decoded = InviteInfo.Decode(rawBase64);

            Assert.Equal(original.InviteId, decoded.InviteId);
            Assert.Equal(original.ReunionId, decoded.ReunionId);
            Assert.Equal(original.ExpiresAt.Date, decoded.ExpiresAt.Date);
            Assert.Equal(original.InviteEmail, decoded.InviteEmail);
        }
    }
}
