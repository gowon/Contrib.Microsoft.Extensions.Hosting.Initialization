namespace Extensions.Hosting.ApplicationInitializer
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationInitialization(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // https://github.com/dotnet/runtime/issues/38751
            services.AddHostedService<ApplicationInitializationService>();
            return services;
        }

        public static IServiceCollection AddInitializer<TInitializer>(this IServiceCollection services)
            where TInitializer : class, IApplicationInitializer
        {
            return services
                .AddApplicationInitialization()
                .AddTransient<IApplicationInitializer, TInitializer>();
        }

        public static IServiceCollection AddInitializer<TInitializer>(this IServiceCollection services,
            TInitializer initializer)
            where TInitializer : class, IApplicationInitializer
        {
            if (initializer == null)
            {
                throw new ArgumentNullException(nameof(initializer));
            }

            return services
                .AddApplicationInitialization()
                .AddSingleton<IApplicationInitializer>(initializer);
        }

        public static IServiceCollection AddInitializer(this IServiceCollection services,
            Func<IServiceProvider, IApplicationInitializer> implementationFactory)
        {
            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            return services
                .AddApplicationInitialization()
                .AddTransient(implementationFactory);
        }

        public static IServiceCollection AddInitializer(this IServiceCollection services, Type initializerType)
        {
            if (initializerType == null)
            {
                throw new ArgumentNullException(nameof(initializerType));
            }

            return services
                .AddApplicationInitialization()
                .AddTransient(typeof(IApplicationInitializer), initializerType);
        }


        public static IServiceCollection AddInitializer(this IServiceCollection services,
            Action<IServiceProvider> initializer)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException(nameof(initializer));
            }

            return services
                .AddApplicationInitialization()
                .AddSingleton<IApplicationInitializer>(provider => new DelegateInitializer(initializer, provider));
        }
    }
}