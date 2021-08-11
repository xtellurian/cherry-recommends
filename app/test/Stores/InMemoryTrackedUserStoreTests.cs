using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.EntityFramework;
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
            var commonId = Guid.NewGuid().ToString();
            var user = await sut.Create(new TrackedUser(commonId));

            var res = await sut.Read(user.Id);

            Assert.Equal(res.Id, user.Id);
            Assert.Equal(res.CommonUserId, commonId);
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
            var trackedEvent = new TrackedUserEvent(user, Guid.NewGuid().ToString(), updatedTime, null, null, "common_event_type", new DynamicPropertyDictionary
            {
                 {"a key", "a logical value"}
            });
            await sut.AddTrackedUserEvents(new List<TrackedUserEvent>() { trackedEvent });

            // Assert
            // Can get the properties
            var returnedEvents = await sut.ReadEventsForUser(userId);
            foreach (var e in returnedEvents)
            {
                Assert.Equal(userId, e.CommonUserId);
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

            var event1 = new TrackedUserEvent(user, Guid.NewGuid().ToString(), updatedTime, null, null, "common_event_type", new DynamicPropertyDictionary
            {
                 {"a key", "a logical value"}
            });
            var event2 = new TrackedUserEvent(user, Guid.NewGuid().ToString(), updatedTime, null, null, "common_event_type", new DynamicPropertyDictionary
            {
                 {"a key", "a logical value"}
            });

            await sut.AddTrackedUserEvents(new List<TrackedUserEvent> { event1, event2 });

            // Assert
            // Can get the events
            var returnedProperties = await sut.ReadEventsForUser(userId);
            foreach (var p in returnedProperties)
            {
                Assert.Equal(userId, p.CommonUserId);
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

            var maryProperties = new TrackedUserEvent(maryTracked, Guid.NewGuid().ToString(), updatedTime, null, null, "Set_Name", new DynamicPropertyDictionary
            {
                {"name", "Mary"}
            });

            var garyProperties = new TrackedUserEvent(maryTracked, Guid.NewGuid().ToString(), updatedTime, null, null, "Set_Name", new DynamicPropertyDictionary
            {
                {"name", "Gary"}
            });
            await sut.AddTrackedUserEvents(new List<TrackedUserEvent> { maryProperties, garyProperties });
            // Act
            var filteredEvents = (await sut.ReadEventsOfType("Set_Name")).ToList();

            //assert
            Assert.Equal(2, filteredEvents.Count);
            foreach (var e in filteredEvents)
            {
                e.Properties.ContainsKey("Name");
            }

        }

    }
}
