using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SignalBox.Core;
using SignalBox.Infrastructure.EntityFramework;

namespace SignalBox.Infrastructure
{
    public interface IEnvironmentService : IInterceptor
    {
#nullable enable
        long? CurrentEnvironmentId { get; }
        Task<Environment?> ReadCurrent(IEnvironmentStore store);
    }
}