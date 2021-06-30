namespace Extensions.Hosting.Bootstrapper
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    internal class DelegateInitializer : IApplicationInitializer
    {
        private readonly Func<IServiceProvider, Task> _initializer;
        private readonly IServiceProvider _serviceProvider;

        public DelegateInitializer(Func<IServiceProvider, Task> initializer, IServiceProvider serviceProvider)
        {
            _initializer = initializer;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                await _initializer.Invoke(scope.ServiceProvider);
            }
        }
    }
}