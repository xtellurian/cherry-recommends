using System.Threading.Tasks;
using Pulumi;

namespace Signalbox.Azure
{
    class Program
    {
        static Task<int> Main() => Deployment.RunAsync<AppStack>();
    }
}
