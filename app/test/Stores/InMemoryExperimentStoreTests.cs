using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class InMemoryExperimentStoreTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]
        public async Task CanStoreAndLoad()
        {
            // arrange
            var sut = new InMemoryExperimentStore();
            // act
            var experiment = await sut.Create(new Experiment());

            var res = await sut.Read(experiment.Id);

            Assert.Equal(res.Id, experiment.Id);
        }

        [Fact]
        public async Task CanStoreAndLoadListOfIds()
        {
            // arrange
            var sut = new InMemoryExperimentStore();

            //act
            var experiment = await sut.Create(new Experiment());
            var metas = await sut.List();
            Assert.Contains(experiment.Id, metas.Select(_ => _.Id));
        }
    }
}
