
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a container for store operations.
    /// </summary>
    public interface IStoreOperationContainer
    {
        /// <summary>
        /// Gets the store operation.
        /// </summary>
        IStoreOperation StoreOperation { get; }

        /// <summary>
        /// Tries to get the store operation of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the store operation.</typeparam>
        /// <param name="value">When this method returns, contains the store operation if the operation is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the store operation is found; otherwise, <c>false</c>.</returns>
        bool TryGetStoreOperation<T>(out T? value) where T : IStoreOperation;

        /// <summary>
        /// Gets the type of the store operation contained in the continer.
        /// </summary>
        /// <returns>The type of the store operation contained in the continer.</returns>
        Type GetStoreOperationType();

        /// <summary>
        /// Gets the category of the store operation.
        /// </summary>
        StoreOperationCategory StoreOperationCategory { get; }
    }
}
