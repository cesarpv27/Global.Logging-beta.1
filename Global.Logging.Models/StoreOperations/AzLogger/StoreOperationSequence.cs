
using System.Collections;

namespace Global.Logging.Models
{
    /// <inheritdoc cref="IStoreOperationSequence{TRawLog, TReadOnlyLog, TEx}"/>
    public sealed class StoreOperationSequence<TRawLog, TReadOnlyLog, TEx> : IStoreOperationSequence<TRawLog, TReadOnlyLog, TEx>, IEnumerable<StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>>
        where TRawLog : IRawLog<TEx>
        where TReadOnlyLog : IReadOnlyLog
        where TEx : Exception
    {
        List<StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>>? _storeOperationContainers;
        List<AddToTableOperation<TRawLog, TEx>>? _addToTableOperations;
        List<AddToBlobOperation<TRawLog, TEx>>? _addToBlobOperations;
        List<GetFromTableOperation<TReadOnlyLog>>? _getFromTableOperations;
        List<GetFromBlobOperation<TReadOnlyLog>>? _getFromBlobOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreOperationSequence{TRawLog, TReadOnlyLog, TEx}"/> class.
        /// </summary>
        public StoreOperationSequence() { }

        #region Private properties

        private List<StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>> StoreOperationContainers
        {
            get
            {
                if (_storeOperationContainers == null)
                    _storeOperationContainers = new List<StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>>();

                return _storeOperationContainers;
            }
        }

        private List<AddToTableOperation<TRawLog, TEx>> AddToTableOperations
        {
            get
            {
                if (_addToTableOperations == null)
                    _addToTableOperations = new List<AddToTableOperation<TRawLog, TEx>>();

                return _addToTableOperations;
            }
        }

        private List<GetFromTableOperation<TReadOnlyLog>> GetFromTableOperations
        {
            get
            {
                if (_getFromTableOperations == null)
                    _getFromTableOperations = new List<GetFromTableOperation<TReadOnlyLog>>();

                return _getFromTableOperations;
            }
        }

        private List<AddToBlobOperation<TRawLog, TEx>> AddToBlobOperations
        {
            get
            {
                if (_addToBlobOperations == null)
                    _addToBlobOperations = new List<AddToBlobOperation<TRawLog, TEx>>();

                return _addToBlobOperations;
            }
        }

        private List<GetFromBlobOperation<TReadOnlyLog>> GetFromBlobOperations
        {
            get
            {
                if (_getFromBlobOperations == null)
                    _getFromBlobOperations = new List<GetFromBlobOperation<TReadOnlyLog>>();

                return _getFromBlobOperations;
            }
        }

        #endregion

        /// <inheritdoc/>
        public int Count => StoreOperationContainers.Count;

        /// <inheritdoc/>
        public void AddNext(StoreOperationContainer<TRawLog, TReadOnlyLog, TEx> storeOperationContainer)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperationContainer, nameof(storeOperationContainer));

            StoreOperationContainers.Add(storeOperationContainer);

            switch (storeOperationContainer.StoreOperationCategory)
            {
                case StoreOperationCategory.AddToTable:
                    AddToTableOperations.Add(StoreOperationContainer.GetStoreOperationOrThrow<AddToTableOperation<TRawLog, TEx>>(storeOperationContainer));
                    break;
                case StoreOperationCategory.AddToBlob:
                    AddToBlobOperations.Add(StoreOperationContainer.GetStoreOperationOrThrow<AddToBlobOperation<TRawLog, TEx>>(storeOperationContainer));
                    break;
                case StoreOperationCategory.GetFromTable:
                    GetFromTableOperations.Add(StoreOperationContainer.GetStoreOperationOrThrow<GetFromTableOperation<TReadOnlyLog>>(storeOperationContainer));
                    break;
                case StoreOperationCategory.GetFromBlob:
                    GetFromBlobOperations.Add(StoreOperationContainer.GetStoreOperationOrThrow<GetFromBlobOperation<TReadOnlyLog>>(storeOperationContainer));
                    break;
                default:
                    throw new InvalidOperationException(StoreOperationConstants.GetUnsupportedStoreOperationCategoryMessage(storeOperationContainer.StoreOperationCategory, nameof(storeOperationContainer)));
            }
        }

        /// <inheritdoc/>
        public void AddNext(AddToTableOperation<TRawLog, TEx> operation)
        {
            AssertHelper.AssertNotNullOrThrow(operation, nameof(operation));

            AddNext(new StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>(operation));
        }

        /// <inheritdoc/>
        public void AddNext(AddToBlobOperation<TRawLog, TEx> operation)
        {
            AssertHelper.AssertNotNullOrThrow(operation, nameof(operation));

            AddNext(new StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>(operation));
        }

        /// <inheritdoc/>
        public void AddNext(GetFromTableOperation<TReadOnlyLog> operation)
        {
            AssertHelper.AssertNotNullOrThrow(operation, nameof(operation));

            AddNext(new StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>(operation));
        }

        /// <inheritdoc/>
        public void AddNext(GetFromBlobOperation<TReadOnlyLog> operation)
        {
            AssertHelper.AssertNotNullOrThrow(operation, nameof(operation));

            AddNext(new StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>(operation));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of store operation containers.
        /// This method provides a read-only enumeration of the store operation containers.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of <see cref="StoreOperationContainer{TRawLog, TReadOnlyLog, TEx}"/>.
        /// This method provides a read-only enumeration of the store operation containers.
        /// </summary>
        /// <returns>An enumerator for the collection of <see cref="StoreOperationContainer{TRawLog, TReadOnlyLog, TEx}"/>.</returns>
        public IEnumerator<StoreOperationContainer<TRawLog, TReadOnlyLog, TEx>> GetEnumerator()
        {
            foreach (var container in StoreOperationContainers)
                yield return container;
        }
        
        /// <inheritdoc/>
        public IEnumerable<AddToTableOperation<TRawLog, TEx>> EnumerateAddToTableOperations()
        {
            foreach (var operation in AddToTableOperations)
                yield return operation;
        }

        /// <inheritdoc/>
        public IEnumerable<AddToBlobOperation<TRawLog, TEx>> EnumerateAddToBlobOperations()
        {
            foreach (var operation in AddToBlobOperations)
                yield return operation;
        }

        /// <inheritdoc/>
        public IEnumerable<GetFromTableOperation<TReadOnlyLog>> EnumerateGetFromTableOperations()
        {
            foreach (var operation in GetFromTableOperations)
                yield return operation;
        }

        /// <inheritdoc/>
        public IEnumerable<GetFromBlobOperation<TReadOnlyLog>> EnumerateGetFromBlobOperations()
        {
            foreach (var operation in GetFromBlobOperations)
                yield return operation;
        }
    }
}
