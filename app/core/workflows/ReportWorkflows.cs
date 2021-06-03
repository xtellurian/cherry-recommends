using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class ReportWorkflows : IWorkflow
    {
        private readonly IFileStore fileStore;

        public ReportWorkflows(IFileStore fileStore)
        {
            this.fileStore = fileStore;
        }

        public async Task<IEnumerable<FileInformation>> ListReports()
        {
            return await fileStore.ListFiles();
        }

        public async Task<byte[]> DownloadReport(string reportName)
        {
            return await fileStore.ReadAllBytes(reportName);
        }
    }
}