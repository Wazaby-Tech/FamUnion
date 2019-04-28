using FamUnion.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FamUnion.Core.Tests
{
    public class EventTests
    {
        [Theory]
        [InlineData("Test Event", "7/4/2019 08:00", "")]
        [InlineData("Test Event", "7/4/2019 08:00", "7/4/2019 10:00")]
        public void ValidEventPasses(string name, string start, string end)
        {
            Event @event = new Event();

            @event.Name = name;
            @event.StartTime = DateTimeOffset.Parse(start);
            @event.EndTime = string.IsNullOrEmpty(end) ? (DateTimeOffset?)null : DateTimeOffset.Parse(end);

            Assert.True(@event.IsValid());
        }

        [Theory]
        [InlineData("Test Event", "7/4/2019 08:00", "7/4/2019 07:00")]
        public void InvalidEventFails(string name, string start, string end)
        {
            Event @event = new Event();

            @event.Name = name;
            @event.StartTime = DateTimeOffset.Parse(start);
            @event.EndTime = string.IsNullOrEmpty(end) ? (DateTimeOffset?)null : DateTimeOffset.Parse(end);

            Assert.False(@event.IsValid());
        }
    }
}
