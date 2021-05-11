using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class InMemoryTrackedUserStoreTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]
        public async Task CanStoreAndLoad()
        {
            var sut = new InMemoryTrackedUserStore();
            var externalId = Guid.NewGuid().ToString();
            var user = await sut.Create(new TrackedUser(externalId));

            var res = await sut.Read(user.Id);

            Assert.Equal(res.Id, user.Id);
            Assert.Equal(res.ExternalId, externalId);
        }

        [Fact]
        public async Task CanStore_Empty_TrackedUserEvents()
        {
            // Arrange
            // Setup a new user
            var userStore = new InMemoryTrackedUserStore();
            var sut = new InMemoryEventStore();
            var userId = Guid.NewGuid().ToString();
            var user = new TrackedUser(userId);
            await userStore.Create(user);

            // Act
            // Add some events, empty this time.
            var updatedTime = dt.Now;
            var trackedEvent = new TrackedUserEvent(userId, updatedTime, "Key", "Value");
            await sut.AddTrackedUserEvents(new List<TrackedUserEvent>() { trackedEvent });

            // Assert
            // Can get the properties
            var returnedEvents = await sut.ReadEventsForUser(userId);
            foreach (var e in returnedEvents)
            {
                Assert.Equal(userId, e.TrackedUserExternalId);
                Assert.Equal(updatedTime, e.Timestamp);
            }
        }

        [Fact]
        public async Task CanStore_TrackedUserEvents()
        {
            // Arrange
            // Setup a new user
            var userStore = new InMemoryTrackedUserStore();
            var sut = new InMemoryEventStore();
            var userId = Guid.NewGuid().ToString();
            var user = new TrackedUser(userId);
            await userStore.Create(user);

            // Act
            // Add some properties, empty this time.
            var updatedTime = dt.Now;

            var event1 = new TrackedUserEvent(userId, updatedTime, "a key", "a logical value");
            var event2 = new TrackedUserEvent(userId, updatedTime, "a second key", null, 42);
            await sut.AddTrackedUserEvents(new List<TrackedUserEvent> { event1, event2 });

            // Assert
            // Can get the events
            var returnedProperties = await sut.ReadEventsForUser(userId);
            foreach (var p in returnedProperties)
            {
                Assert.Equal(userId, p.TrackedUserExternalId);
                Assert.Equal(p.Timestamp, updatedTime);
            }
        }

        [Fact]
        public async Task CanFilterOnGet_TrackedUserEvents()
        {
            // Arrange
            // Setup a new user
            var userStore = new InMemoryTrackedUserStore();
            var sut = new InMemoryEventStore();
            var idMary = Guid.NewGuid().ToString();
            var idGary = Guid.NewGuid().ToString();
            var maryTracked = new TrackedUser(idMary);
            var garyTracked = new TrackedUser(idGary);
            await userStore.Create(maryTracked);
            await userStore.Create(garyTracked);

            var updatedTime = dt.Now;

            var maryProperties = new TrackedUserEvent(idMary, updatedTime, "name", "Mary");
            var garyProperties = new TrackedUserEvent(idMary, updatedTime, "name", "Gary");

            await sut.AddTrackedUserEvents(new List<TrackedUserEvent> { maryProperties, garyProperties });
            // Act
            var filteredProperties = (await sut.ReadEventsForKey("name", "Mary")).ToList();

            //assert
            Assert.Single(filteredProperties);
            foreach (var p in filteredProperties)
            {
                Assert.Equal("name", p.Key);
                Assert.Equal("Mary", p.LogicalValue);
            }

        }

    }
}
