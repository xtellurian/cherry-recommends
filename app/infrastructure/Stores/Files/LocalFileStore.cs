using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Files
{
    public class LocalFileStore : IReportFileStore
    {
        private string rootPath = "~";
        public LocalFileStore(string rootPath = null)
        {
            this.rootPath = rootPath ?? Environment.CurrentDirectory;
        }

        public Task<IEnumerable<FileInformation>> ListFiles(string path = null)
        {
            var fullPath = Path.Join(rootPath, path);
            var fullFilePaths = Directory.GetFiles(fullPath);
            return Task.FromResult(
                fullFilePaths.Select(_ => new FileInformation(_.Substring(rootPath.Length).TrimStart('/'))));
        }

        public Task<Stream> OpenStream(string path)
        {
            var fullPath = Path.Join(rootPath, path);
            return Task.FromResult<Stream>(File.OpenRead(fullPath));
        }

        public async Task<string> ReadAllText(string path)
        {
            var fullPath = Path.Join(rootPath, path);
            return await File.ReadAllTextAsync(fullPath);
        }

        public async Task<byte[]> ReadAllBytes(string path)
        {
            var fullPath = Path.Join(rootPath, path);
            return await File.ReadAllBytesAsync(fullPath);
        }

        public Task WriteFile(string contents, string name)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReadAsString(string path)
        {
            throw new NotImplementedException();
        }
    }
}