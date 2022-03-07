using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ReportsController : SignalBoxControllerBase
    {
        private readonly ReportWorkflows workflows;

        public ReportsController(ReportWorkflows workflows)
        {
            this.workflows = workflows;
        }

        /// <summary>Returns a list of available reports.</summary>
        [HttpGet]
        public async Task<IEnumerable<FileInformation>> ListReports()
        {
            return await workflows.ListReports();
        }

        /// <summary>Downloads a file.</summary>
        [HttpGet("download")]
        public async Task<FileResult> ListReports(string report)
        {
            var fileBytes = await workflows.DownloadReport(report);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, report);
        }

    }
}