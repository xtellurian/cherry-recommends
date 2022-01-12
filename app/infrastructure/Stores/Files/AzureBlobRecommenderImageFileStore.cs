using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Files
{
    public class AzureBlobRecommenderImageFileStore : AzureBlobFileStore, IRecommenderImageFileStore
    {
        public AzureBlobRecommenderImageFileStore(IOptions<RecommenderImageFileHosting> options) : base(options)
        {
        }
    }
}