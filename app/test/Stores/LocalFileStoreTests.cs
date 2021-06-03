using System.Linq;
using System.Threading.Tasks;
using SignalBox.Infrastructure.Files;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class LocalFileStoreTests
    {

        [Fact]
        public async Task LocalFileStoreCanListFiles()
        {
            var sut = new LocalFileStore();
            var files = await sut.ListFiles();
            Assert.NotEmpty(files);

            var f = files.First();

            var stream1 = await sut.OpenStream(f.Name);
            Assert.True(stream1.CanRead);
        }
    }
}