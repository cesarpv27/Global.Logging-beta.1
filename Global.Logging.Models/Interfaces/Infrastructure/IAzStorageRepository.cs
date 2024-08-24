
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a storage repository in Azure.
    /// </summary>
    public interface IAzStorageRepository
    {
        /// <summary>
        /// The set of options that can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new name, 
        /// such as a new table name for the Azure TableClient or a new blob container name for the BlobContainerClient.
        /// </summary>
        AzRetryOptions? AzRetryOptions { get; set; }
    }
}
