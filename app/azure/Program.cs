using System.Threading.Tasks;
using Pulumi;

namespace SignalBox.Azure
{
    class Program
    {
        static Task<int> Main() => Deployment.RunAsync<AppStack>();
    }
}
