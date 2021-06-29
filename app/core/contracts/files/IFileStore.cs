using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IFileStore
    {
        Task<IEnumerable<FileInformation>> ListFiles(string subPath = null);
        Task<byte[]> ReadAllBytes(string path);
        Task<string> ReadAsString(string path);
        Task WriteFile(string contents, string name);
    }
}