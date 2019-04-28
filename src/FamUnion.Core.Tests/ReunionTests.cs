using FamUnion.Core.Model;
using System;
using Xunit;

namespace FamUnion.Core.Tests
{
    public class ReunionTests
    {
        [Theory]
        [InlineData("Test Reunion", "", "")]
        [InlineData("Test Reunion", "4/15/2019", "")]
        [InlineData("Test Reunion", "", "4/17/2019")]
        [InlineData("Test Reunion", "4/15/2019", "4/17/2019")]
        public void ValidReunionPasses(string name, string startDate, string endDate)
        {
            Reunion reunion = new Reunion
            {
                Name = name,
                StartDate = string.IsNullOrEmpty(startDate) ? (DateTime?)null : DateTime.Parse(startDate),
                EndDate = string.IsNullOrEmpty(endDate) ? (DateTime?)null : DateTime.Parse(endDate)
            };

            Assert.True(reunion.IsValid());
        }

        [Theory]
        [InlineData("","","")]
        [InlineData("", "4/15/2019", "4/17/2019")]
        [InlineData("Test Reunion", "4/15/2019", "4/14/2019")]
        public void InvalidReunionFails(string name, string startDate, string endDate)
        {
            Reunion reunion = new Reunion
            {
                Name = name,
                StartDate = string.IsNullOrEmpty(startDate) ? (DateTime?)null : DateTime.Parse(startDate),
                EndDate = string.IsNullOrEmpty(endDate) ? (DateTime?)null : DateTime.Parse(endDate)
            };

            Assert.False(reunion.IsValid());
        }
    }
}
