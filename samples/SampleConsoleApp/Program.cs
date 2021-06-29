namespace SampleConsoleApp
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Extensions.Hosting.ApplicationInitializer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddInitializer(provider =>
                        {
                            var logger = provider.GetRequiredService<ILogger<Program>>();
                            logger.LogInformation("Initializing stuff.");
                        })
                        .AddInitializer(provider =>
                        {
                            var factory = provider.GetRequiredService<IHttpClientFactory>();
                            var httpClient = factory.CreateClient(nameof(SampleInitializer));
                            return new SampleInitializer(httpClient);
                        });

                    services.AddHttpClient<SampleInitializer>();
                })
                .Build()
                .RunAsync();
        }
    }
}