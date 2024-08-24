
namespace Global.Logging.Infrastructure.Repositories
{
    /// <inheritdoc cref="IAzStorageRepository"/>
    public abstract class AzStorageRepository : IAzStorageRepository
    {
        /// <summary>
        /// Creates an instance of <typeparamref name="TClientOptions"/> if <see cref="AzRetryOptions"/> is specified;
        /// otherwise, returns <c>null</c>.
        /// </summary>
        /// <typeparam name="TClientOptions">The type of client options.</typeparam>
        /// <returns>Instance of <typeparamref name="TClientOptions"/> or <c>null</c>.</returns>
        protected TClientOptions? CreateClientOptionsOfGetDefault<TClientOptions>()
            where TClientOptions : ClientOptions, new()
        {
            if (AzRetryOptions == default)
                return default;

            var clientOptions = new TClientOptions();
            AzRetryOptions.CopyTo(clientOptions.Retry);

            return clientOptions;
        }

        /// <inheritdoc/>
        public virtual AzRetryOptions? AzRetryOptions { get; set; }
    }
}
