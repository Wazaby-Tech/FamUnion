using FamUnion.Core.Model;
using FamUnion.Core.Request;
using FamUnion.Core.Utility;
using System;
using Xunit;
using static FamUnion.Core.Utility.Constants;

namespace FamUnion.Core.Tests
{
    public class NewReunionRequestMapperTests
    {
        [Fact]
        public void MapsBasicFieldsCorrectly()
        {
            var startDate = new DateTime(2024, 7, 4);
            var endDate = new DateTime(2024, 7, 6);
            var request = new NewReunionRequest
            {
                Name = "Smith Reunion",
                UserId = "auth0|user1",
                Description = "Annual gathering",
                StartDate = startDate,
                EndDate = endDate
            };

            var result = NewReunionRequestMapper.Map(request);

            Assert.Equal("Smith Reunion", result.Name);
            Assert.Equal("auth0|user1", result.ActionUserId);
            Assert.Equal("Annual gathering", result.Description);
            Assert.Equal(startDate, result.StartDate);
            Assert.Equal(endDate, result.EndDate);
        }

        [Fact]
        public void SetsAddressTypeToReunionWhenLocationProvided()
        {
            var request = new NewReunionRequest
            {
                Name = "Smith Reunion",
                UserId = "auth0|user1",
                Location = new Address { Description = "Park", City = "Atlanta", State = "GA" }
            };

            var result = NewReunionRequestMapper.Map(request);

            Assert.NotNull(result.Location);
            Assert.Equal(EntityType.Reunion, result.Location.AddressType);
        }

        [Fact]
        public void LocationIsNullWhenNotProvided()
        {
            var request = new NewReunionRequest
            {
                Name = "Smith Reunion",
                UserId = "auth0|user1",
                Location = null
            };

            var result = NewReunionRequestMapper.Map(request);

            Assert.Null(result.Location);
        }

        [Fact]
        public void MappedReunionHasNoId()
        {
            var request = new NewReunionRequest
            {
                Name = "Smith Reunion",
                UserId = "auth0|user1"
            };

            var result = NewReunionRequestMapper.Map(request);

            Assert.Null(result.Id);
        }

        [Fact]
        public void MappedReunionPreservesLocationFields()
        {
            var request = new NewReunionRequest
            {
                Name = "Smith Reunion",
                UserId = "auth0|user1",
                Location = new Address
                {
                    Description = "City Park",
                    City = "Atlanta",
                    State = "GA",
                    ZipCode = "30301"
                }
            };

            var result = NewReunionRequestMapper.Map(request);

            Assert.Equal("City Park", result.Location.Description);
            Assert.Equal("Atlanta", result.Location.City);
            Assert.Equal("GA", result.Location.State);
            Assert.Equal("30301", result.Location.ZipCode);
        }
    }
}
