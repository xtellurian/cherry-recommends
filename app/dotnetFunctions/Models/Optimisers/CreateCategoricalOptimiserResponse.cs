using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace SignalBox.Functions
{
    public class CreateCategoricalOptimiserResponse
    {
        [TableOutput("CategoricalOptimisers", Connection = "AzureWebJobsStorage")]
        public CategoricalOptimiserRecord Record { get; set; }


        [BlobOutput("{tenant}/categorical-optimisers/{id}.json", Connection = "AzureWebJobsStorage")]
        public byte[] OutputBlob { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }
}