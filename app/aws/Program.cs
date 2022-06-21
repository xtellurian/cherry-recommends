using System.Threading.Tasks;
using Pulumi;

namespace Cherry.CloudInfrastructure.AWS
{
    class Program
    {
        static Task<int> Main() => Deployment.RunAsync<AwsInfrastructure>();
    }
}
