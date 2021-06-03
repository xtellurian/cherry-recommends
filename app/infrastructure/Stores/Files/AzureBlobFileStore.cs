using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Files
{
    public class AzureBlobFileStore : IFileStore
    {
        private FileHosting hosting;
        private BlobServiceClient blobServiceClient;

        public AzureBlobFileStore(IOptions<FileHosting> options)
        {
            this.hosting = options.Value;
            this.blobServiceClient = new BlobServiceClient(hosting.ConnectionString);
        }

        public async Task<IEnumerable<FileInformation>> ListFiles(string subPath = null)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(hosting.ContainerName);

            var fileInfos = new List<FileInformation>();
            // List all blobs in the container
            await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: hosting.SubPath))
            {
                var name = blobItem.Name.Substring(hosting.SubPath?.Length ?? 0).TrimStart('/');
                fileInfos.Add(new FileInformation(name));
            }

            return fileInfos;
        }

        public async Task<byte[]> ReadAllBytes(string path)
        {
            MemoryStream memStream = new MemoryStream();
            await this.CopyTo(path, memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream.GetBuffer();
        }
        
        public async Task CopyTo(string path, Stream stream)
        {
            var fullPath = Path.Join(hosting.SubPath, path);
            var blobClient = new BlobClient(hosting.ConnectionString, hosting.ContainerName, fullPath);
            var download = await blobClient.DownloadAsync();
            await download.Value.Content.CopyToAsync(stream);
        }
    }
}