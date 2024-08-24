
namespace Global.Logging.Settings
{
    /// <summary>
    /// Defines base settings for Azure services.
    /// </summary>
    public interface IAzureSettings
    {
        /// <summary>
        /// Gets or sets the connection string for Azure services.
        /// </summary>
        string? ConnectionString { get; set; }
    }

    /// <inheritdoc cref="IAzureSettings"/>
    public class AzureSettings : IAzureSettings
    {
        /// <inheritdoc/>
        public virtual string? ConnectionString { get; set; }
    }
}
