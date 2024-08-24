
namespace Global.Logging.Extensions
{
    /// <summary>
    /// Provides extension methods for adding default Global Logging services to the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default Global Logging services configuration to the specified <see cref="IServiceCollection"/> 
        /// using the connection string retrieved from the <paramref name="getConnectionString"/>.
        /// </summary>
        /// <param name="services">The service collection to add the Global Logging services to.</param>
        /// <param name="getConnectionString">A function to get the connection string from the Service Provider.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="getConnectionString"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when the connection string retrieved from the <paramref name="getConnectionString"/> 
        /// is null, empty, or consists only of white-space characters.</exception>
        public static IServiceCollection AddDefaultGlobalLoggingServices(
            this IServiceCollection services,
            Func<IServiceProvider, string> getConnectionString)
        {
            AssertHelper.AssertNotNullOrThrow(services, nameof(services));
            AssertHelper.AssertNotNullOrThrow(getConnectionString, nameof(getConnectionString));

            services.AddSingleton(sp => new ConnectionStringProvider(getConnectionString, sp));

            services.AddTransient<IAzTableRepository<LogEntity>, AzTableRepository<LogEntity>>(sp =>
            {
                return new AzTableRepository<LogEntity>(sp.GetRequiredService<ConnectionStringProvider>().ConnectionString);
            });

            services.AddTransient<IAzBlobRepository<LogEntity>, AzBlobRepository<LogEntity>>(sp =>
            {
                return new AzBlobRepository<LogEntity>(sp.GetRequiredService<ConnectionStringProvider>().ConnectionString);
            });

            services.AddTransient<IAzTableService<LogEntity>, AzTableService<LogEntity>>();
            services.AddTransient<IAzBlobService<LogEntity>, AzBlobService<LogEntity>>();

            services.AddTransient<IAzLoggingService<LogEntity>, AzLoggingService<LogEntity>>();

            services.AddSingleton<ILogFactory<ReadOnlyRawLog<Exception>, ReadOnlyLog, LogEntity, Exception>, LogFactory<Exception>>();

            services.AddSingleton<IAzLogger, AzLogger>();

            return services;
        }

        /// <summary>
        /// Provides a connection string using lazy initialization.
        /// </summary>
        class ConnectionStringProvider
        {
            private readonly Lazy<string> _connectionString;

            /// <summary>
            /// Initializes a new instance of the <see cref="ConnectionStringProvider"/> class.
            /// </summary>
            /// <param name="getConnectionString">A function to get the connection string from the Service Provider.</param>
            /// <param name="serviceProvider">The Service Provider.</param>
            public ConnectionStringProvider(Func<IServiceProvider, string> getConnectionString, IServiceProvider serviceProvider)
            {
                _connectionString = new Lazy<string>(() => getConnectionString(serviceProvider));
            }

            /// <summary>
            /// Gets the connection string.
            /// </summary>
            public string ConnectionString => _connectionString.Value;
        }
    }
}
