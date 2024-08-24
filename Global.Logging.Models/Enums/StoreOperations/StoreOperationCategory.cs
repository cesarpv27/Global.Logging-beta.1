
namespace Global.Logging.Models
{
    /// <summary>
    /// Defines the types of store operations that can be performed.
    /// </summary>
    public enum StoreOperationCategory
    {
        /// <summary>
        /// Represents an operation to add an entry to a table.
        /// </summary>
        AddToTable = 100,

        /// <summary>
        /// Represents an operation to add an entry to a blob.
        /// </summary>
        AddToBlob = 200,

        /// <summary>
        /// Represents an operation to retrieve an entry from a table.
        /// </summary>
        GetFromTable = 300,

        /// <summary>
        /// Represents an operation to retrieve an entry from a blob.
        /// </summary>
        GetFromBlob = 400,
    }
}
