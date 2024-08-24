
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a sequence of store operations managed within store operation containers.
    /// </summary>
    /// <typeparam name="TRawLog">The type of the raw log.</typeparam>
    /// <typeparam name="TReadOnlyLog">The type of the read-only log.</typeparam>
    /// <typeparam name="TEx">The type of the exception.</typeparam>
    public interface IStoreOperationSequence<TRawLog, TReadOnlyLog, TEx>
        where TRawLog : IRawLog<TEx>
        where TReadOnlyLog : IReadOnlyLog
        where TEx : Exception
    {
        /// <summary>
        /// The number of elements contained in the store operation sequence.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a store operation container to the sequence.
        /// </summary>
        /// <param name="storeOperationContainer">The store operation container to add to the next order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storeOperationContainer"/> is null.</exception>
        void AddNext(StoreOperationContainer<TRawLog, TReadOnlyLog, TEx> storeOperationContainer);

        /// <summary>
        /// Adds the specified store operation to the sequence for the next order in the store operation container.
        /// </summary>
        /// <param name="operation">The store operation to add to the next order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> is null.</exception>
        void AddNext(AddToTableOperation<TRawLog, TEx> operation);

        /// <summary>
        /// Adds the specified store operation to the sequence for the next order in the store operation container.
        /// </summary>
        /// <param name="operation">The store operation to add to the next order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> is null.</exception>
        void AddNext(AddToBlobOperation<TRawLog, TEx> operation);

        /// <summary>
        /// Adds the specified store operation to the sequence for the next order in the store operation container.
        /// </summary>
        /// <param name="operation">The store operation to add to the next order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> is null.</exception>
        void AddNext(GetFromTableOperation<TReadOnlyLog> operation);

        /// <summary>
        /// Adds the specified store operation to the sequence for the next order in the store operation container.
        /// </summary>
        /// <param name="operation">The store operation to add to the next order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> is null.</exception>
        void AddNext(GetFromBlobOperation<TReadOnlyLog> operation);

        /// <summary>
        /// Returns an enumerator that iterates through a collection of <see cref="AddToTableOperation{TRawLog, TEx}"/> operations.
        /// This method provides a read-only enumeration of the operations.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that can be used to iterate through the 
        /// collection of <see cref="AddToTableOperation{TRawLog, TEx}"/> operations.</returns>
        IEnumerable<AddToTableOperation<TRawLog, TEx>> EnumerateAddToTableOperations();

        /// <summary>
        /// Returns an enumerator that iterates through a collection of <see cref="AddToBlobOperation{TRawLog, TEx}"/> operations.
        /// This method provides a read-only enumeration of the operations.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that can be used to iterate through the 
        /// collection of <see cref="AddToBlobOperation{TRawLog, TEx}"/> operations.</returns>
        IEnumerable<AddToBlobOperation<TRawLog, TEx>> EnumerateAddToBlobOperations();

        /// <summary>
        /// Returns an enumerator that iterates through a collection of <see cref="GetFromTableOperation{TReadOnlyLog}"/> operations.
        /// This method provides a read-only enumeration of the operations.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that can be used to iterate through the 
        /// collection of <see cref="GetFromTableOperation{TReadOnlyLog}"/> operations.</returns>
        IEnumerable<GetFromTableOperation<TReadOnlyLog>> EnumerateGetFromTableOperations();

        /// <summary>
        /// Returns an enumerator that iterates through a collection of <see cref="GetFromBlobOperation{TReadOnlyLog}"/> operations.
        /// This method provides a read-only enumeration of the operations.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> that can be used to iterate through the 
        /// collection of <see cref="GetFromBlobOperation{TReadOnlyLog}"/> operations.</returns>
        IEnumerable<GetFromBlobOperation<TReadOnlyLog>> EnumerateGetFromBlobOperations();
    }
}
