
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a container for storing different types of store operations.
    /// </summary>
    /// <typeparam name="TRawLog">The type of the raw log.</typeparam>
    /// <typeparam name="TReadOnlyLog">The type of the read-only log.</typeparam>
    /// <typeparam name="TEx">The type of the exception.</typeparam>
    public sealed class StoreOperationContainer<TRawLog, TReadOnlyLog, TEx> : IStoreOperationContainer
        where TRawLog : IRawLog<TEx>
        where TReadOnlyLog : IReadOnlyLog
        where TEx : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreOperationContainer{TRawLog, TReadOnlyLog, TEx}"/> class with an <paramref name="storeOperation"/>.
        /// </summary>
        /// <param name="storeOperation">The store operation to add to the container.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="storeOperation"/> is null.</exception>
        public StoreOperationContainer(AddToTableOperation<TRawLog, TEx> storeOperation)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperation, nameof(storeOperation));

            TypeStoreOperationPair = new KeyValuePair<Type, IStoreOperation>(storeOperation.GetType(), storeOperation);
            StoreOperationCategory = StoreOperationCategory.AddToTable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreOperationContainer{TRawLog, TReadOnlyLog, TEx}"/> class with an <paramref name="storeOperation"/>.
        /// </summary>
        /// <param name="storeOperation">The store operation to add to the container.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="storeOperation"/> is null.</exception>
        public StoreOperationContainer(AddToBlobOperation<TRawLog, TEx> storeOperation)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperation, nameof(storeOperation));

            TypeStoreOperationPair = new KeyValuePair<Type, IStoreOperation>(storeOperation.GetType(), storeOperation);
            StoreOperationCategory = StoreOperationCategory.AddToBlob;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFromTableOperation{TReadOnlyLog}"/> class with an <paramref name="storeOperation"/>.
        /// </summary>
        /// <param name="storeOperation">The store operation to add to the container.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="storeOperation"/> is null.</exception>
        public StoreOperationContainer(GetFromTableOperation<TReadOnlyLog> storeOperation)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperation, nameof(storeOperation));

            TypeStoreOperationPair = new KeyValuePair<Type, IStoreOperation>(storeOperation.GetType(), storeOperation);
            StoreOperationCategory = StoreOperationCategory.GetFromTable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFromBlobOperation{TReadOnlyLog}"/> class with an <paramref name="storeOperation"/>.
        /// </summary>
        /// <param name="storeOperation">The store operation to add to the container.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="storeOperation"/> is null.</exception>
        public StoreOperationContainer(GetFromBlobOperation<TReadOnlyLog> storeOperation)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperation, nameof(storeOperation));

            TypeStoreOperationPair = new KeyValuePair<Type, IStoreOperation>(storeOperation.GetType(), storeOperation);
            StoreOperationCategory = StoreOperationCategory.GetFromBlob;
        }

        /// <summary>
        /// Gets the store operation as a key-value pair where the key is the type of the operation and the value is the operation instance.
        /// </summary>
        private KeyValuePair<Type, IStoreOperation> TypeStoreOperationPair { get; }

        /// <inheritdoc/>
        public IStoreOperation StoreOperation => TypeStoreOperationPair.Value;

        /// <inheritdoc/>
        public bool TryGetStoreOperation<T>(out T? value) where T : IStoreOperation
        {
            if (TypeStoreOperationPair.Value is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public Type GetStoreOperationType()
        {
            return TypeStoreOperationPair.Key;
        }

        /// <inheritdoc/>
        public StoreOperationCategory StoreOperationCategory { get; }
    }

    /// <summary>
    /// Represents a store operation container.
    /// </summary>
    public static class StoreOperationContainer
    {
        /// <summary>
        /// Gets the store operation from the specified store operation container, or throws an exception if the operation is not found. 
        /// The caller must ensure that the <typeparamref name="T"/> is the same type as the store operation contained in the <paramref name="storeOperationContainer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the store operation.</typeparam>
        /// <param name="storeOperationContainer">The store operation container that holds the store operation.</param>
        /// <returns>The store operation of the specified type.</returns>
        /// <exception cref="Exception">Thrown if the store operation of the specified type is not found in the container.</exception>
        public static T GetStoreOperationOrThrow<T>(IStoreOperationContainer storeOperationContainer)
            where T : IStoreOperation
        {
            if (storeOperationContainer.TryGetStoreOperation(out T? storeOperation))
                return storeOperation!;

            throw new Exception(StoreOperationConstants.GetStoreOperationTypeNotFound(typeof(T).Name));
        }
    }
}
