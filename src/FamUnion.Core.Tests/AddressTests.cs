using FamUnion.Core.Model;
using Xunit;

namespace FamUnion.Core.Tests
{
    public class AddressTests
    {
        [Theory]
        [InlineData("Test Address", "Jacksonville", "FL")]
        public void ValidAddressPasses(string description, string city, string state)
        {
            Address address = new Address
            {
                Description = description,
                City = city,
                State = state
            };

            Assert.True(address.IsValid());
        }

        [Theory]
        [InlineData("Test Address", "Jacksonville", "")]
        [InlineData("Test Address", "", "FL")]
        [InlineData("", "Jacksonville", "FL")]
        public void InvalidAddressFails(string description, string city, string state)
        {
            Address address = new Address
            {
                Description = description,
                City = city,
                State = state
            };

            Assert.False(address.IsValid());
        }
    }
}
