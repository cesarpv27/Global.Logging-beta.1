
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a table store operation.
    /// </summary>
    public interface ITableStoreOperation
    {
        /// <summary>
        /// If true, create the Azure table if it does not exist.
        /// </summary>
        bool CreateTableIfNotExists { get; set; }
    }
}
