namespace SampleConsoleApp
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Extensions.Hosting.Bootstrapper;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<Foo>();

                    services
                        .AddInitializer<FooInitializer>()
                        .AddInitializer(_ =>
                        {
                            Console.WriteLine("A second initializer.");
                            return Task.CompletedTask;
                        })
                        .AddInitializer(provider =>
                        {
                            var factory = provider.GetRequiredService<IHttpClientFactory>();
                            var httpClient = factory.CreateClient(nameof(SampleInitializer));
                            return new SampleInitializer(httpClient);
                        });

                    services.AddHttpClient<SampleInitializer>();
                })
                .Build();

            await host.StartAsync();
            await host.StopAsync();
        }
    }
}