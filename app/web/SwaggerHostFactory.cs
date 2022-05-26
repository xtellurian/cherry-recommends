using Microsoft.Extensions.Hosting;

namespace SignalBox.Web
{
    public class SwaggerHostFactory
    {
        public static IHost CreateHost()
        {
            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore#use-the-cli-tool-with-a-custom-host-configuration
            // Enables the swagger spec generation to happen using Development Environment
            return Program
                .CreateHostBuilder(System.Array.Empty<string>())
                .UseEnvironment("Development")
                .Build();
        }
    }
}
