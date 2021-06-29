using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Files
{
    public class AzureBlobReportFileStore : AzureBlobFileStore, IReportFileStore
    {
        public AzureBlobReportFileStore(IOptions<ReportFileHosting> options) : base(options)
        {
        }
    }
}