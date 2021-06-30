namespace Extensions.Hosting.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using static ExceptionFilterUtility;

    internal class ApplicationInitializationService : IHostedService
    {
        private readonly ILogger<ApplicationInitializationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationInitializationService(ILogger<ApplicationInitializationService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Application initialization starting");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var initializers = scope.ServiceProvider.GetRequiredService<IEnumerable<IApplicationInitializer>>();

                    foreach (var initializer in initializers)
                    {
                        _logger.LogInformation("{InitializerType} initialization starting", initializer.GetType());
                        try
                        {
                            await initializer.InitializeAsync();
                            _logger.LogInformation("{InitializerType} initialization completed", initializer.GetType());
                        }
                        catch (Exception e) when (False(() =>
                            _logger.LogError(e, "{InitializerType} initialization failed", initializer.GetType())))
                        {
                            throw;
                        }
                    }
                }

                _logger.LogInformation("Application initialization completed");
            }
            catch (Exception e) when (False(() => _logger.LogError(e, "Application initialization failed")))
            {
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}