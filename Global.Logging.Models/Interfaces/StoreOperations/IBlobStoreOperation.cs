
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a blob store operation.
    /// </summary>
    public interface IBlobStoreOperation
    {
        /// <summary>
        /// If true, create the Azure Blob Container if it does not exist.
        /// </summary>
        bool CreateBlobContainerIfNotExists { get; set; }
    }
}
