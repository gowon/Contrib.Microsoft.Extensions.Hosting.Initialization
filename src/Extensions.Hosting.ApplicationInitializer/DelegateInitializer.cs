namespace Extensions.Hosting.ApplicationInitializer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class DelegateInitializer : IApplicationInitializer
    {
        private readonly Action<IServiceProvider> _initializer;
        private readonly IServiceProvider _serviceProvider;

        public DelegateInitializer(Action<IServiceProvider> initializer, IServiceProvider serviceProvider)
        {
            _initializer = initializer;
            _serviceProvider = serviceProvider;
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            _initializer.Invoke(_serviceProvider);
            return Task.CompletedTask;
        }
    }
}