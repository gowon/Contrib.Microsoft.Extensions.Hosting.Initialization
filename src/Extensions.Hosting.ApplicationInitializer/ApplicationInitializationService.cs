namespace Extensions.Hosting.ApplicationInitializer
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal class ApplicationInitializationService : IHostedService
    {
        private readonly ILogger<ApplicationInitializationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationInitializationService(ILogger<ApplicationInitializationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application initialization");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var initializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IApplicationInitializer>>();

                    foreach (var initializer in initializers)
                    {
                        _logger.LogInformation("Starting initialization for {InitializerType}", initializer.GetType());
                        try
                        {
                            await initializer.InitializeAsync(cancellationToken);
                            _logger.LogInformation("Initialization for {InitializerType} completed", initializer.GetType());
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Initialization for {InitializerType} failed", initializer.GetType());
                            throw;
                        }
                    }
                }
                
                _logger.LogInformation("Application initialization completed");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Application initialization failed");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}