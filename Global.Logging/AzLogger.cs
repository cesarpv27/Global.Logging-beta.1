
namespace Global.Logging
{
    /// <summary>
    /// Logging service for Azure Table Service and Azure Blob Storage. This class is thread-safe, mutable and disposable.
    /// </summary>
    /// <inheritdoc cref="IAzLogger"/>
    public class AzLogger : AzLogger<Exception>, IAzLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzLogger"/> class.
        /// </summary>
        /// <param name="azLoggingService">The logging service used for logging.</param>
        /// <param name="logFactory">The factory used for creating log entities.</param>
        /// <param name="azLogFilter">The log filter, if provided. Default is <c>null</c>.</param>
        /// <param name="delegateContainer">A class that contains delegates for generating keys and names for Azure log entities. 
        /// Sets a property in <paramref name="delegateContainer"/> to null to reset it to the default behavior.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="azLoggingService"/> or <paramref name="logFactory"/> are null.</exception>
        public AzLogger(
            IAzLoggingService<LogEntity> azLoggingService,
            ILogFactory<ReadOnlyRawLog<Exception>, ReadOnlyLog, LogEntity, Exception> logFactory,
            IAzLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azLogFilter = default,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = default) : base(azLoggingService, logFactory, azLogFilter, delegateContainer)
        { }
    }

    /// <summary>
    /// Logging service for Azure Table Service and Azure Blob Storage. This class is thread-safe, mutable and disposable.
    /// </summary>
    /// <inheritdoc cref="IAzLogger{TEx}"/>
    public class AzLogger<TEx> : AzLogger<RawLog<TEx>, ReadOnlyRawLog<TEx>, ReadOnlyLog, LogEntity, TEx>, IAzLogger<TEx>
        where TEx : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzLogger{TEx}"/> class.
        /// </summary>
        /// <param name="azLoggingService">The logging service used for logging.</param>
        /// <param name="logFactory">The factory used for creating log entities.</param>
        /// <param name="azLogFilter">The log filter, if provided. Default is <c>null</c>.</param>
        /// <param name="delegateContainer">A class that contains delegates for generating keys and names for Azure log entities. 
        /// Sets a property in <paramref name="delegateContainer"/> to null to reset it to the default behavior.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="azLoggingService"/> or <paramref name="logFactory"/> are null.</exception>
        public AzLogger(
            IAzLoggingService<LogEntity> azLoggingService,
            ILogFactory<ReadOnlyRawLog<TEx>, ReadOnlyLog, LogEntity, TEx> logFactory,
            IAzLogFilter<ReadOnlyRawLog<TEx>, ReadOnlyLog>? azLogFilter = default,
            DelegateContainer<ReadOnlyRawLog<TEx>>? delegateContainer = default) : base(azLoggingService, logFactory, azLogFilter, delegateContainer)
        { }

        #region Custom sync AddToTable methods

        /// <inheritdoc/>
        public virtual AzTableResponse AddToTable(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToTable(
                RawLogFactory.Create<TEx>(
                    source,
                    severityLevel,
                    message),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists);
        }

        /// <inheritdoc/>
        public virtual AzTableResponse AddToTable(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToTable(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists);
        }
        
        /// <inheritdoc/>
        public virtual AzTableResponse AddToTable(
                string source,
                SeverityLevel severityLevel,
                string? message,
                string? category,
                Dictionary<string, string>? verbose,
                TEx? exception,
                string? callStack,
                string? callStackMethod = default,
                int? callStackLine = default,
                int? callStackColumn = default,
                bool? retryOnFailures = null,
                int? maxRetryAttempts = null,
                bool createTableIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToTable(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists);
        }

        /// <inheritdoc/>
        public virtual AzTableResponse AddToTable(
                string source,
                SeverityLevel severityLevel,
                string? message,
                string? category,
                Dictionary<string, string>? verbose,
                TEx? exception,
                ILogLabelSet? logLabelSet,
                string? callStack,
                string? callStackMethod = default,
                int? callStackLine = default,
                int? callStackColumn = default,
                bool? retryOnFailures = null,
                int? maxRetryAttempts = null,
                bool createTableIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToTable(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    logLabelSet,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists);
        }

        #endregion

        #region Custom async AddToTable methods

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddToTableAsync(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToTableAsync(
                RawLogFactory.Create<TEx>(
                    source,
                    severityLevel,
                    message),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists,
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddToTableAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToTableAsync(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists,
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddToTableAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToTableAsync(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists, 
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddToTableAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            ILogLabelSet? logLabelSet,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToTableAsync(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    logLabelSet,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createTableIfNotExists,
                cancellationToken);
        }

        #endregion

        #region Custom sync AddToBlob methods

        /// <inheritdoc/>
        public virtual AzBlobResponse AddToBlob(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToBlob(
                RawLogFactory.Create<TEx>(
                    source,
                    severityLevel,
                    message),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists);
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse AddToBlob(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToBlob(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists);
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse AddToBlob(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToBlob(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists);
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse AddToBlob(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            ILogLabelSet? logLabelSet,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return AddToBlob(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    logLabelSet,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists);
        }

        #endregion

        #region Custom async AddToBlob methods

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> AddToBlobAsync(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToBlobAsync(
                RawLogFactory.Create<TEx>(
                    source,
                    severityLevel,
                    message),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists,
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> AddToBlobAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToBlobAsync(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists, 
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> AddToBlobAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToBlobAsync(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists,
                cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> AddToBlobAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose,
            TEx? exception,
            ILogLabelSet? logLabelSet,
            string? callStack,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(source, nameof(source));

            return await AddToBlobAsync(
                RawLogFactory.Create(
                    source,
                    severityLevel,
                    message,
                    category,
                    verbose,
                    exception,
                    logLabelSet,
                    callStack,
                    callStackMethod,
                    callStackLine,
                    callStackColumn),
                retryOnFailures,
                maxRetryAttempts,
                createBlobContainerIfNotExists,
                cancellationToken);
        }

        #endregion
    }

    /// <summary>
    /// Logging service for Azure Table Service and Azure Blob Storage. This class is thread-safe, mutable and disposable.
    /// </summary>
    /// <inheritdoc cref="IAzLogger{TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx}"/>
    public class AzLogger<TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx> : IAzLogger<TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx>, IDisposable
        where TRawLog : IRawLog<TEx>
        where TReadOnlyRawLog : IReadOnlyRawLog
        where TReadOnlyLog : IReadOnlyLog
        where TLogEntity : class, ILogEntity
        where TEx : Exception
    {
        private bool _disposed = false;
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly TimeSpan _semaphoreSlimTimeOut;
        private int? _maxRetryAttempts;
        private bool _retryOnFailures;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzLogger{TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx}"/> class.
        /// </summary>
        /// <param name="azLoggingService">The logging service used for logging.</param>
        /// <param name="logFactory">The factory used for creating log entities.</param>
        /// <param name="azLogFilter">The log filter, if provided. Default is <c>null</c>.</param>
        /// <param name="delegateContainer">A class that contains delegates for generating keys and names for Azure log entities. 
        /// Sets a property in <paramref name="delegateContainer"/> to null to reset it to the default behavior.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="azLoggingService"/> or <paramref name="logFactory"/> are null.</exception>
        /// <remarks>If <paramref name="azLogFilter"/> is not specified, all logs are allowed for both writing or reading.</remarks>
        public AzLogger(
            IAzLoggingService<TLogEntity> azLoggingService,
            ILogFactory<TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx> logFactory,
            IAzLogFilter<TReadOnlyRawLog, TReadOnlyLog>? azLogFilter = default,
            DelegateContainer<TReadOnlyRawLog>? delegateContainer = default)
        {
            AssertHelper.AssertNotNullOrThrow(azLoggingService, nameof(azLoggingService));
            AssertHelper.AssertNotNullOrThrow(logFactory, nameof(logFactory));

            AzLoggingService = azLoggingService;

            LogFactory = logFactory;

            AzLogFilter = azLogFilter;

            _semaphoreSlim = new SemaphoreSlim(1, 1);
            _semaphoreSlimTimeOut = TimeSpan.FromSeconds(45);

            _retryOnFailures = true;

            if (delegateContainer != default)
                UpdateDelegates(delegateContainer);
        }

        #region Private properties

        /// <summary>
        /// The logging service in Azure Storage.
        /// </summary>
        private IAzLoggingService<TLogEntity> AzLoggingService { get; }

        /// <summary>
        /// The factory for creating log entities.
        /// </summary>
        private ILogFactory<TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx> LogFactory { get; }

        /// <summary>
        /// The filter for Azure Storage logs.
        /// </summary>
        private IAzLogFilter<TReadOnlyRawLog, TReadOnlyLog>? AzLogFilter { get; }

        /// <summary>
        /// Delegate that generates a partition key for Azure log entities.
        /// </summary>
        private GeneratePartitionKeyDel<TReadOnlyRawLog>? GeneratePartitionKeyDel { get; set; }

        /// <summary>
        /// Delegate that generates a row keys for Azure Storage entities.
        /// </summary>
        private GenerateRowKeyDel<TReadOnlyRawLog>? GenerateRowKeyDel { get; set; }

        /// <summary>
        /// Delegate for filling an instance of <see cref="IVerboseLabelSet"/> with specific data for categorization or identification purposes,  
        /// using data from the specified "Verbose" collection.
        /// The verbose label set will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.
        /// </summary>
        private FillVerboseLabelSetDel? FillVerboseLabelSetDel { get; set; }

        /// <summary>
        /// Delegate that generates an Azure Table names for storing log entities. 
        /// If <see cref="GenerateLogTableNameDel"/> is null, the table names will be generated using the default behavior:
        /// "Logger" appended with the current year, month, and the following suffix:
        /// "Low" if the "SeverityLevel" is <see cref="SeverityLevel.Info"/> or <see cref="SeverityLevel.Warning"/>, otherwise "High".
        /// <example>
        /// "Logger200101Low"
        /// "Logger200101High"
        /// </example>
        /// </summary>
        private GenerateLogTableNameDel<TReadOnlyRawLog>? GenerateLogTableNameDel { get; set; }

        /// <summary>
        /// Delegate that generates an Azure Blob Container names for storing log entities.
        /// If <see cref="GenerateLogBlobContainerNameDel"/> is null, the blob container name will be generated using the default behavior:
        /// "azlogs-" appeded with the current year and month.
        /// <example>
        /// "azlogs-200101"
        /// </example>
        /// </summary>
        private GenerateLogBlobContainerNameDel<TReadOnlyRawLog>? GenerateLogBlobContainerNameDel { get; set; }

        /// <summary>
        /// Delegate that generates an Azure Blob names for storing log entities.
        /// If <see cref="GenerateLogBlobNameDel"/> is null, the blob name will be generated using the default behavior:
        /// ILogEntity.SeverityLevel/ILogEntity.PartitionKey/ILogEntity.RowKey.json.
        /// <example>
        /// "Info/PartitionKey/RowKey.json"
        /// </example>
        /// </summary>
        private GenerateLogBlobNameDel<TReadOnlyRawLog>? GenerateLogBlobNameDel { get; set; }

        #endregion

        #region Sync & async Execute store operation sequence methods

        /// <inheritdoc/>
        public virtual ExGlobalResponse ExecuteSequence(StoreOperationSequence<TRawLog, TReadOnlyLog, TEx> storeOperationSequence)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperationSequence, nameof(storeOperationSequence));
            ArgumentOutOfRangeException.ThrowIfEqual(storeOperationSequence.Count, 0, nameof(storeOperationSequence));

            try
            {
                ResponseStatus responseStatus;
                ExGlobalResponse? response;
                foreach (var storeOperationContainer in storeOperationSequence)
                {
                    switch (storeOperationContainer.StoreOperationCategory)
                    {
                        case StoreOperationCategory.AddToTable:
                            responseStatus = Add(StoreOperationContainer.GetStoreOperationOrThrow<AddToTableOperation<TRawLog, TEx>>(storeOperationContainer));
                            break;
                        case StoreOperationCategory.AddToBlob:
                            responseStatus = Add(StoreOperationContainer.GetStoreOperationOrThrow<AddToBlobOperation<TRawLog, TEx>>(storeOperationContainer));
                            break;
                        case StoreOperationCategory.GetFromTable:
                            responseStatus = Get(StoreOperationContainer.GetStoreOperationOrThrow<GetFromTableOperation<TReadOnlyLog>>(storeOperationContainer));
                            break;
                        case StoreOperationCategory.GetFromBlob:
                            responseStatus = Get(StoreOperationContainer.GetStoreOperationOrThrow<GetFromBlobOperation<TReadOnlyLog>>(storeOperationContainer));
                            break;
                        default:
                            throw new InvalidOperationException(StoreOperationConstants.GetUnsupportedStoreOperationCategoryMessage(storeOperationContainer.StoreOperationCategory, nameof(storeOperationContainer)));
                    }
                    
                    response = GetStoreOperationResponseOrDefault(storeOperationContainer.StoreOperation, responseStatus, nameof(storeOperationSequence));
                    if (response != default)
                        return response;
                }

                return ExGlobalResponseFactory.CreateSuccessful();
            }
            catch (Exception ex)
            {
                return ExGlobalResponseFactory.CreateFailure(ex);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<ExGlobalResponse> ExecuteSequenceAsync(
            StoreOperationSequence<TRawLog, TReadOnlyLog, TEx> storeOperationSequence,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(storeOperationSequence, nameof(storeOperationSequence));
            ArgumentOutOfRangeException.ThrowIfEqual(storeOperationSequence.Count, 0, nameof(storeOperationSequence));

            try
            {
                ResponseStatus responseStatus;
                ExGlobalResponse? response;
                foreach (var storeOperationContainer in storeOperationSequence)
                {
                    switch (storeOperationContainer.StoreOperationCategory)
                    {
                        case StoreOperationCategory.AddToTable:
                            responseStatus = await AddAsync(StoreOperationContainer.GetStoreOperationOrThrow<AddToTableOperation<TRawLog, TEx>>(storeOperationContainer), cancellationToken);
                            break;
                        case StoreOperationCategory.AddToBlob:
                            responseStatus = await AddAsync(StoreOperationContainer.GetStoreOperationOrThrow<AddToBlobOperation<TRawLog, TEx>>(storeOperationContainer), cancellationToken);
                            break;
                        case StoreOperationCategory.GetFromTable:
                            responseStatus = await GetAsync(StoreOperationContainer.GetStoreOperationOrThrow<GetFromTableOperation<TReadOnlyLog>>(storeOperationContainer), cancellationToken);
                            break;
                        case StoreOperationCategory.GetFromBlob:
                            responseStatus = await GetAsync(StoreOperationContainer.GetStoreOperationOrThrow<GetFromBlobOperation<TReadOnlyLog>>(storeOperationContainer), cancellationToken);
                            break;
                        default:
                            throw new NotSupportedException(StoreOperationConstants.GetUnsupportedStoreOperationCategoryMessage(storeOperationContainer.StoreOperationCategory, nameof(storeOperationContainer)));
                    }

                    response = GetStoreOperationResponseOrDefault(storeOperationContainer.StoreOperation, responseStatus, nameof(storeOperationSequence));
                    if (response != default)
                        return response;
                }

                return ExGlobalResponseFactory.CreateSuccessful();
            }
            catch (Exception ex)
            {
                return ExGlobalResponseFactory.CreateFailure(ex);
            }
        }

        #endregion

        #region Sync add methods

        /// <inheritdoc/>
        public virtual AzTableResponse AddToTable(
            TRawLog rawLog,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(rawLog, nameof(rawLog));

            bool _semaphoreAcquired = false;
            try
            {
                CheckDisposedWaitForSemaphore();
                _semaphoreAcquired = true;

                var filterCreateResponse = FilterForWritingAndCreateEntities(rawLog);
                if (filterCreateResponse.Status == ResponseStatus.Failure)
                    return AzTableResponseFactory.CreateFrom(filterCreateResponse);

                return AzLoggingService.AddToTable(filterCreateResponse.Value.LogEntity,
                    GenerateLogTableName(filterCreateResponse.Value.ReadOnlyRawLog, filterCreateResponse.Value.LogEntity),
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts), createTableIfNotExists);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobResponse AddToBlob(
            TRawLog rawLog,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true)
        {
            AssertHelper.AssertNotNullOrThrow(rawLog, nameof(rawLog));

            bool _semaphoreAcquired = false;
            try
            {
                CheckDisposedWaitForSemaphore();
                _semaphoreAcquired = true;

                var filterCreateResponse = FilterForWritingAndCreateEntities(rawLog);
                if (filterCreateResponse.Status == ResponseStatus.Failure)
                    return AzBlobResponseFactory.CreateFrom(filterCreateResponse);

                return AzLoggingService.AddToBlob(filterCreateResponse.Value.LogEntity,
                    GenerateBlobContainerName(filterCreateResponse.Value.ReadOnlyRawLog, filterCreateResponse.Value.LogEntity),
                    GenerateBlobName(filterCreateResponse.Value.ReadOnlyRawLog, filterCreateResponse.Value.LogEntity),
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createBlobContainerIfNotExists);
            }
            catch (Exception ex)
            {
                return AzBlobResponseFactory.CreateFailure(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        #endregion

        #region Async add methods

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddToTableAsync(
            TRawLog rawLog,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(rawLog, nameof(rawLog));

            bool _semaphoreAcquired = false;
            try
            {
                await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
                _semaphoreAcquired = true;

                var filterCreateResponse = FilterForWritingAndCreateEntities(rawLog);
                if (filterCreateResponse.Status == ResponseStatus.Failure)
                    return AzTableResponseFactory.CreateFrom(filterCreateResponse);

                return await AzLoggingService.AddToTableAsync(filterCreateResponse.Value.LogEntity,
                    GenerateLogTableName(filterCreateResponse.Value.ReadOnlyRawLog, filterCreateResponse.Value.LogEntity),
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createTableIfNotExists,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                return AzTableResponseFactory.CreateFailure(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobResponse> AddToBlobAsync(
            TRawLog rawLog,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(rawLog, nameof(rawLog));

            bool _semaphoreAcquired = false;
            try
            {
                await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
                _semaphoreAcquired = true;

                var filterCreateResponse = FilterForWritingAndCreateEntities(rawLog);
                if (filterCreateResponse.Status == ResponseStatus.Failure)
                    return AzBlobResponseFactory.CreateFrom(filterCreateResponse);

                return await AzLoggingService.AddToBlobAsync(filterCreateResponse.Value.LogEntity,
                    GenerateBlobContainerName(filterCreateResponse.Value.ReadOnlyRawLog, filterCreateResponse.Value.LogEntity),
                    GenerateBlobName(filterCreateResponse.Value.ReadOnlyRawLog, filterCreateResponse.Value.LogEntity),
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createBlobContainerIfNotExists,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                return AzBlobResponseFactory.CreateFailure(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        #endregion

        #region Sync get methods

        /// <inheritdoc/>
        public virtual AzTableValueResponse<TReadOnlyLog> GetFromTable(
            string tableName,
            string partitionKey,
            string rowKey,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = false)
        {
            bool _semaphoreAcquired = false;
            try
            {
                CheckDisposedWaitForSemaphore();
                _semaphoreAcquired = true;

                return FilterForReading(AzTableValueResponseFactory.MapFrom(
                    AzLoggingService.GetFromTable(tableName, partitionKey, rowKey,
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createTableIfNotExists), 
                    ConvertTo));
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<TReadOnlyLog>(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public virtual AzBlobValueResponse<TReadOnlyLog> GetFromBlob(
            string blobContainerName,
            string blobName,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = false)
        {
            bool _semaphoreAcquired = false;
            try
            {
                CheckDisposedWaitForSemaphore();
                _semaphoreAcquired = true;

                return FilterForReading(AzBlobValueResponseFactory.MapFrom(
                    AzLoggingService.GetFromBlob(blobContainerName, blobName,
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createBlobContainerIfNotExists),
                    ConvertTo));
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TReadOnlyLog>(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        #endregion

        #region Async get methods

        /// <inheritdoc/>
        public virtual async Task<AzTableValueResponse<TReadOnlyLog>> GetFromTableAsync(
            string tableName,
            string partitionKey,
            string rowKey,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = false,
            CancellationToken cancellationToken = default)
        {
            bool _semaphoreAcquired = false;
            try
            {
                await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
                _semaphoreAcquired = true;

                return FilterForReading(AzTableValueResponseFactory.MapFrom(
                    await AzLoggingService.GetFromTableAsync(tableName, partitionKey, rowKey,
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createTableIfNotExists,
                    cancellationToken), 
                    ConvertTo));
            }
            catch (Exception ex)
            {
                return AzTableValueResponseFactory.CreateFailure<TReadOnlyLog>(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzBlobValueResponse<TReadOnlyLog>> GetFromBlobAsync(
            string blobContainerName,
            string blobName,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = false,
            CancellationToken cancellationToken = default)
        {
            bool _semaphoreAcquired = false;
            try
            {
                await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
                _semaphoreAcquired = true;

                return FilterForReading(AzBlobValueResponseFactory.MapFrom(
                    await AzLoggingService.GetFromBlobAsync(blobContainerName, blobName,
                    GetRetryOnFailuresOrDefault(retryOnFailures),
                    GetMaxRetryAttemptsOrDefault(maxRetryAttempts),
                    createBlobContainerIfNotExists,
                    cancellationToken),
                    ConvertTo));
            }
            catch (Exception ex)
            {
                return AzBlobValueResponseFactory.CreateFailure<TReadOnlyLog>(ex);
            }
            finally
            {
                if (_semaphoreAcquired)
                    _semaphoreSlim.Release();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates the delegates in the logger with the values from <paramref name="delegateContainer"/>, which are used for creating partition keys, row keys, 
        /// table names, blob container names, and blob names for log entities in Azure Storage.
        /// </summary>
        /// <param name="delegateContainer">A class that contains delegates for generating keys and names for Azure log entities.</param>
        private void UpdateDelegates(DelegateContainer<TReadOnlyRawLog> delegateContainer)
        {
            GeneratePartitionKeyDel = delegateContainer.GeneratePartitionKeyDel;
            GenerateRowKeyDel = delegateContainer.GenerateRowKeyDel;
            FillVerboseLabelSetDel = delegateContainer.FillVerboseLabelSetDel;
            GenerateLogTableNameDel = delegateContainer.GenerateLogTableNameDel;
            GenerateLogBlobContainerNameDel = delegateContainer.GenerateLogBlobContainerNameDel;
            GenerateLogBlobNameDel = delegateContainer.GenerateLogBlobNameDel;
        }

        private async Task CheckDisposedWaitForSemaphoreAsync(CancellationToken cancellationToken)
        {
            CheckDisposed();

            if (!await _semaphoreSlim.WaitAsync(_semaphoreSlimTimeOut, cancellationToken))
                throw new TimeoutException($"Failed to acquire the semaphore within the timeout of {_semaphoreSlimTimeOut.TotalSeconds} seconds.");
        }

        private void CheckDisposedWaitForSemaphore()
        {
            CheckDisposed();

            if (!_semaphoreSlim.Wait(_semaphoreSlimTimeOut))
                throw new TimeoutException($"Failed to acquire the semaphore within the timeout of {_semaphoreSlimTimeOut.TotalSeconds} seconds.");
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private ExGlobalValueResponse<(TReadOnlyRawLog ReadOnlyRawLog, TLogEntity LogEntity)> FilterForWritingAndCreateEntities(TRawLog rawLog)
        {
            var createReadOnlyRawLogResponse = LogFactory.Create(rawLog);
            if (createReadOnlyRawLogResponse.Status != ResponseStatus.Success)
                return ExGlobalValueResponseFactory.MapFromFailure<TReadOnlyRawLog, (TReadOnlyRawLog ReadOnlyRawLog, TLogEntity LogEntity)>(createReadOnlyRawLogResponse);

            var readOnlyRawLog = createReadOnlyRawLogResponse.Value!;
            var filterResponse = FilterForWriting(readOnlyRawLog);
            if (filterResponse.Status != ResponseStatus.Success)
                return ExGlobalValueResponseFactory.MapFromFailure<(TReadOnlyRawLog ReadOnlyRawLog, TLogEntity LogEntity)>(filterResponse);

            var createLogEntityResponse = LogFactory.Create(readOnlyRawLog, GeneratePartitionKey, GenerateRowKey, FillVerboseLabelSet);
            if (createLogEntityResponse.Status != ResponseStatus.Success)
                return ExGlobalValueResponseFactory.MapFromFailure<(TReadOnlyRawLog ReadOnlyRawLog, TLogEntity LogEntity)>(createLogEntityResponse);

            return ExGlobalValueResponseFactory.CreateSuccessful<(TReadOnlyRawLog ReadOnlyRawLog, TLogEntity LogEntity)>((readOnlyRawLog, createLogEntityResponse.Value!));
        }

        private GlobalResponse<Exception> FilterForWriting(TReadOnlyRawLog readOnlyRawLog)
        {
            try
            {
                if (AzLogFilter != null && AzLogFilter.IsWritingAllowed != null &&
                    !AzLogFilter.IsWritingAllowed(readOnlyRawLog))
                {
                    var response = GlobalResponseFactory.CreateFailure<Exception>();
                    response.AddMessageTransactionally(
                        AzLoggerConstants.WritingNotAllowedMessageKey,
                        AzLoggerConstants.WritingNotAllowedMessage);

                    return response;
                }

                return GlobalResponseFactory.CreateSuccessful<Exception>();
            }
            catch (Exception e)
            {
                return GlobalResponseFactory.CreateFailure(e);
            }
        }

        private AzTableValueResponse<TReadOnlyLog> FilterForReading(AzTableValueResponse<TReadOnlyLog> response)
        {
            if (response.Status != ResponseStatus.Failure)
            {
                var filterResponse = FilterForReading(response.Value!);
                if (filterResponse.Status != ResponseStatus.Success)
                    return AzTableValueResponseFactory.MapFromFailure<TReadOnlyLog>(filterResponse);
            }

            return response;
        }

        private AzBlobValueResponse<TReadOnlyLog> FilterForReading(AzBlobValueResponse<TReadOnlyLog> response)
        {
            if (response.Status != ResponseStatus.Failure)
            {
                var filterResponse = FilterForReading(response.Value!);
                if (filterResponse.Status != ResponseStatus.Success)
                    return AzBlobValueResponseFactory.MapFromFailure<TReadOnlyLog>(filterResponse);
            }

            return response;
        }

        private GlobalResponse<Exception> FilterForReading(TReadOnlyLog readOnlyLog)
        {
            try
            {
                if (AzLogFilter != null && AzLogFilter.IsReadingAllowed != null &&
                    !AzLogFilter.IsReadingAllowed(readOnlyLog))
                {
                    var response = GlobalResponseFactory.CreateFailure<Exception>();
                    response.AddMessageTransactionally(
                        AzLoggerConstants.ReadingNotAllowedMessageKey,
                        AzLoggerConstants.ReadingNotAllowedMessage);

                    return response;
                }

                return GlobalResponseFactory.CreateSuccessful<Exception>();
            }
            catch (Exception e)
            {
                return GlobalResponseFactory.CreateFailure(e);
            }
        }

        private bool GetRetryOnFailuresOrDefault(bool? retryOnFailures)
        {
            if (retryOnFailures != default)
                return retryOnFailures!.Value;

            return _retryOnFailures;
        }

        private int? GetMaxRetryAttemptsOrDefault(int? maxRetryAttempts)
        {
            if (maxRetryAttempts != default)
                return maxRetryAttempts;

            return _maxRetryAttempts;
        }

        private TReadOnlyLog ConvertTo(TLogEntity logEntity)
        {
            var response = LogFactory.Create(logEntity);
            if (response.Status == ResponseStatus.Failure)
                throw new Exception(AzLoggerConstants.GetMethodResponseStatusMessage(nameof(LogFactory.Create), nameof(LogFactory), response.Status));

            return response.Value!;
        }

        #endregion

        #region Private name generators

        /// <summary>
        /// Generates the partition key based on the specified <paramref name="readOnlyRawLog"/> using either 
        /// a custom delegate <see cref="GeneratePartitionKeyDel"/> or the default method <see cref="GenerateDefaultPartitionKey"/>.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log used to generate the partition key.</param>
        /// <returns>The generated partition key.</returns>
        private string GeneratePartitionKey(TReadOnlyRawLog readOnlyRawLog)
        {
            if (GeneratePartitionKeyDel != default)
                return GeneratePartitionKeyDel(readOnlyRawLog);

            return GenerateDefaultPartitionKey(readOnlyRawLog);
        }

        /// <summary>
        /// Generates the row key based on the specified <paramref name="readOnlyRawLog"/> using either 
        /// a custom delegate <see cref="GenerateRowKeyDel"/> or the default method <see cref="GenerateDefaultRowKey"/>.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log used to generate the row key.</param>
        /// <returns>The generated row key.</returns>
        private string GenerateRowKey(TReadOnlyRawLog readOnlyRawLog)
        {
            if (GenerateRowKeyDel != default)
                return GenerateRowKeyDel(readOnlyRawLog);

            return GenerateDefaultRowKey(readOnlyRawLog);
        }

        /// <summary>
        /// Fills an instance of <see cref="IVerboseLabelSet"/> with specific data for categorization or identification purposes,  
        /// using data from the specified "Verbose" collection.
        /// The verbose label set will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.
        /// </summary>
        private void FillVerboseLabelSet(
            VerboseLabelSet verboseLabelSet,
            ReadOnlyDictionary<string, string> verbose)
        {
            if (FillVerboseLabelSetDel != null)
                FillVerboseLabelSetDel(verboseLabelSet, verbose);
        }

        /// <summary>
        /// Generates an Azure Table name.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="logEntity">The log entity.</param>
        /// <returns>The generated Azure Table name.</returns>
        private string GenerateLogTableName(
            TReadOnlyRawLog readOnlyRawLog,
            TLogEntity logEntity)
        {
            return GenerateLogTableName(readOnlyRawLog, logEntity.PartitionKey, logEntity.RowKey);
        }

        /// <summary>
        /// Generates an Azure Table name based on the provided <paramref name="readOnlyRawLog"/>, using the <see cref="GenerateLogTableNameDel"/> when is specified, 
        /// otherwise the default <see cref="GenerateDefaultLogTableName"/> will be used.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
        /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
        /// <returns>The generated Azure Table name.</returns>
        private string GenerateLogTableName(
            TReadOnlyRawLog readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            if (GenerateLogTableNameDel != default)
                return GenerateLogTableNameDel(readOnlyRawLog, partitionKey, rowKey);

            return GenerateDefaultLogTableName(readOnlyRawLog, partitionKey, rowKey);
        }

        /// <summary>
        /// Generates an Azure Blob Container name.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="logEntity">The log entity.</param>
        /// <returns>The generated Azure Blob Container name.</returns>
        private string GenerateBlobContainerName(
            TReadOnlyRawLog readOnlyRawLog,
            TLogEntity logEntity)
        {
            return GenerateBlobContainerName(readOnlyRawLog, logEntity.PartitionKey, logEntity.RowKey);
        }

        /// <summary>
        /// Generates an Azure Blob Container name based on the provided <paramref name="readOnlyRawLog"/>, using the <see cref="GenerateLogBlobContainerNameDel"/> when is specified, 
        /// otherwise the default <see cref="GenerateDefaultBlobContainerName"/> will be used.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
        /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
        /// <returns>The generated Azure Blob Container name.</returns>
        private string GenerateBlobContainerName(
            TReadOnlyRawLog readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            if (GenerateLogBlobContainerNameDel != default)
                return GenerateLogBlobContainerNameDel(readOnlyRawLog, partitionKey, rowKey);

            return GenerateDefaultBlobContainerName(readOnlyRawLog, partitionKey, rowKey);
        }

        /// <summary>
        /// Generates an Azure Blob name.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="logEntity">The log entity.</param>
        /// <returns>The generated Azure Blob name.</returns>
        /// <returns></returns>
        private string GenerateBlobName(
            TReadOnlyRawLog readOnlyRawLog,
            TLogEntity logEntity)
        {
            return GenerateBlobName(readOnlyRawLog, logEntity.PartitionKey, logEntity.RowKey);
        }

        /// <summary>
        /// Generates an Azure Blob name based on the provided <paramref name="readOnlyRawLog"/>, using the <see cref="GenerateLogBlobNameDel"/> when is specified, 
        /// otherwise the default <see cref="GenerateDefaultBlobName"/> will be used.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
        /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
        /// <returns>The generated Azure Blob name.</returns>
        private string GenerateBlobName(
            TReadOnlyRawLog readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            if (GenerateLogBlobNameDel != default)
                return GenerateLogBlobNameDel(readOnlyRawLog, partitionKey, rowKey);

            return GenerateDefaultBlobName(readOnlyRawLog, partitionKey, rowKey);
        }

        #endregion

        #region Private store operations methods

        /// <summary>
        /// Gets the response for the store operation based on the sequence execution type and response status.
        /// </summary>
        /// <param name="storeOperation">The store operation.</param>
        /// <param name="responseStatus">The response status.</param>
        /// <param name="storeOperationSequenceParamName">The parameter name of the store operation sequence.</param>
        /// <returns>An <see cref="ExGlobalResponse"/> indicating the result of the operation.</returns>
        private ExGlobalResponse? GetStoreOperationResponseOrDefault(
            IStoreOperation storeOperation,
            ResponseStatus responseStatus,
            string storeOperationSequenceParamName)
        {
            switch (storeOperation.SequenceExecutionType)
            {
                case SequenceExecutionType.NextNever:
                    return ExGlobalResponseFactory.CreateSuccessful();
                case SequenceExecutionType.NextAlways:
                    break;
                case SequenceExecutionType.NextOnFails:
                    if (responseStatus != ResponseStatus.Failure)
                        return ExGlobalResponseFactory.CreateSuccessful();
                    break;
                case SequenceExecutionType.NextOnComplete:
                    if (responseStatus == ResponseStatus.Failure)
                        return ExGlobalResponseFactory.CreateSuccessful();
                    break;
                default:
                    throw new InvalidOperationException(AzLoggerConstants.NotSupportedStoreOperationType(responseStatus!.GetType().FullName, storeOperationSequenceParamName));
            }

            return default;
        }

        private ResponseStatus Add(AddToTableOperation<TRawLog, TEx> operation)
        {
            operation.SetResponse(AddToTable(operation.LogEntity, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateTableIfNotExists));

            return operation.Response!.Status;
        }

        private ResponseStatus Add(AddToBlobOperation<TRawLog, TEx> operation)
        {
            operation.SetResponse(AddToBlob(operation.LogEntity, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateBlobContainerIfNotExists));

            return operation.Response!.Status;
        }

        private async Task<ResponseStatus> AddAsync(
            AddToTableOperation<TRawLog, TEx> operation,
            CancellationToken cancellationToken)
        {
            operation.SetResponse(await AddToTableAsync(operation.LogEntity, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateTableIfNotExists, cancellationToken));

            return operation.Response!.Status;
        }

        private async Task<ResponseStatus> AddAsync(
            AddToBlobOperation<TRawLog, TEx> operation,
            CancellationToken cancellationToken)
        {
            operation.SetResponse(await AddToBlobAsync(operation.LogEntity, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateBlobContainerIfNotExists, cancellationToken));

            return operation.Response!.Status;
        }

        private ResponseStatus Get(GetFromTableOperation<TReadOnlyLog> operation)
        {
            operation.SetResponse(GetFromTable(operation.TableName, operation.PartitionKey, operation.RowKey, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateTableIfNotExists));

            return operation.Response!.Status;
        }

        private ResponseStatus Get(GetFromBlobOperation<TReadOnlyLog> operation)
        {
            operation.SetResponse(GetFromBlob(operation.BlobContainerName, operation.BlobName, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateBlobContainerIfNotExists));

            return operation.Response!.Status;
        }

        private async Task<ResponseStatus> GetAsync(
            GetFromTableOperation<TReadOnlyLog> operation,
            CancellationToken cancellationToken)
        {
            operation.SetResponse(await GetFromTableAsync(operation.TableName, operation.PartitionKey, operation.RowKey, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateTableIfNotExists, cancellationToken));

            return operation.Response!.Status;
        }

        private async Task<ResponseStatus> GetAsync(
            GetFromBlobOperation<TReadOnlyLog> operation,
            CancellationToken cancellationToken)
        {
            operation.SetResponse(await GetFromBlobAsync(operation.BlobContainerName, operation.BlobName, operation.RetryOnFailures, operation.MaxRetryAttempts, operation.CreateBlobContainerIfNotExists));

            return operation.Response!.Status;
        }

        #endregion

        #region Update params methods

        #region MaxRetryAttempts

        /// <inheritdoc/>
        public async Task<int?> GetMaxRetryAttemptsAsync(CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                return _maxRetryAttempts;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetMaxRetryAttemptsAsync(int? value, CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                _maxRetryAttempts = value;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        #endregion

        #region RetryOnFailures

        /// <inheritdoc/>
        public async Task<bool> GetRetryOnFailuresAsync(CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                return _retryOnFailures;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetRetryOnFailuresAsync(bool value, CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                _retryOnFailures = value;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        #endregion

        /// <inheritdoc/>
        public async Task UpdateDelegatesAsync(
            DelegateContainer<TReadOnlyRawLog> delegateContainer,
            CancellationToken cancellationToken = default)
        {
            AssertHelper.AssertNotNullOrThrow(delegateContainer, nameof(delegateContainer));

            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                UpdateDelegates(delegateContainer);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetGeneratePartitionKeyDelAsync(
            GeneratePartitionKeyDel<TReadOnlyRawLog>? generatePartitionKeyDel,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                GeneratePartitionKeyDel = generatePartitionKeyDel;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetGeneratePartitionKeyAsync(
            Func<TReadOnlyRawLog, string>? generatePartitionKey,
            CancellationToken cancellationToken = default)
        {
            if (generatePartitionKey != default)
                await SetGeneratePartitionKeyDelAsync(new GeneratePartitionKeyDel<TReadOnlyRawLog>(generatePartitionKey), cancellationToken);
            else
                await SetGeneratePartitionKeyDelAsync(default(GeneratePartitionKeyDel<TReadOnlyRawLog>?), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SetGenerateRowKeyDelAsync(
            GenerateRowKeyDel<TReadOnlyRawLog>? generateRowKeyDel,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                GenerateRowKeyDel = generateRowKeyDel;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetGenerateRowKeyAsync(
            Func<TReadOnlyRawLog, string>? generateRowKey,
            CancellationToken cancellationToken = default)
        {
            if (generateRowKey != default)
                await SetGenerateRowKeyDelAsync(new GenerateRowKeyDel<TReadOnlyRawLog>(generateRowKey), cancellationToken);
            else
                await SetGenerateRowKeyDelAsync(default(GenerateRowKeyDel<TReadOnlyRawLog>?), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SetFillVerboseLabelSetDelAsync(
            FillVerboseLabelSetDel? fillVerboseLabelSetDel,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                FillVerboseLabelSetDel = fillVerboseLabelSetDel;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetFillVerboseLabelSetAsync(
            Action<VerboseLabelSet, ReadOnlyDictionary<string, string>>? fillVerboseLabelSet,
            CancellationToken cancellationToken = default)
        {
            if (fillVerboseLabelSet != default)
                await SetFillVerboseLabelSetDelAsync(new FillVerboseLabelSetDel(fillVerboseLabelSet), cancellationToken);
            else
                await SetFillVerboseLabelSetDelAsync(default(FillVerboseLabelSetDel?), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SetGenerateLogTableNameDelAsync(
            GenerateLogTableNameDel<TReadOnlyRawLog>? generateLogTableNameDel, 
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                GenerateLogTableNameDel = generateLogTableNameDel;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetGenerateLogTableNameAsync(
            Func<TReadOnlyRawLog, string, string, string>? generateLogTableName,
            CancellationToken cancellationToken = default)
        {
            if (generateLogTableName != default)
                await SetGenerateLogTableNameDelAsync(new GenerateLogTableNameDel<TReadOnlyRawLog>(generateLogTableName), cancellationToken);
            else
                await SetGenerateLogTableNameDelAsync(default(GenerateLogTableNameDel<TReadOnlyRawLog>?), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SetGenerateLogBlobContainerNameDelAsync(
            GenerateLogBlobContainerNameDel<TReadOnlyRawLog>? generateLogBlobContainerNameDel,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                GenerateLogBlobContainerNameDel = generateLogBlobContainerNameDel;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetGenerateLogBlobContainerNameAsync(
            Func<TReadOnlyRawLog, string, string, string>? generateLogBlobContainerName,
            CancellationToken cancellationToken = default)
        {
            if (generateLogBlobContainerName != default)
                await SetGenerateLogBlobContainerNameDelAsync(new GenerateLogBlobContainerNameDel<TReadOnlyRawLog>(generateLogBlobContainerName), cancellationToken);
            else
                await SetGenerateLogBlobContainerNameDelAsync(default(GenerateLogBlobContainerNameDel<TReadOnlyRawLog>?), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SetGenerateLogBlobNameDelAsync(
            GenerateLogBlobNameDel<TReadOnlyRawLog>? generateLogBlobNameDel,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                GenerateLogBlobNameDel = generateLogBlobNameDel;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetGenerateLogBlobNameAsync(
            Func<TReadOnlyRawLog, string, string, string>? generateLogBlobName,
            CancellationToken cancellationToken = default)
        {
            if (generateLogBlobName != default)
                await SetGenerateLogBlobNameDelAsync(new GenerateLogBlobNameDel<TReadOnlyRawLog>(generateLogBlobName), cancellationToken);
            else
                await SetGenerateLogBlobNameDelAsync(default(GenerateLogBlobNameDel<TReadOnlyRawLog>?), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SetAzTableRetryOptionsAsync(
            AzRetryOptions? azRetryOptions,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                AzLoggingService.SetAzTableRetryOptions(azRetryOptions);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetAzBlobRetryOptionsAsync(
            AzRetryOptions? azRetryOptions,
            CancellationToken cancellationToken = default)
        {
            await CheckDisposedWaitForSemaphoreAsync(cancellationToken);
            try
            {
                AzLoggingService.SetAzBlobRetryOptions(azRetryOptions);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose the current instance.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _semaphoreSlim.Dispose();
                    }
                    catch
                    {
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Dispose the current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Public name generators

        /// <summary>
        /// Generates the default partition key based on the specified <paramref name="readOnlyRawLog"/>.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log used to generate the partition key.</param>
        /// <returns>The generated default partition key.</returns>
        public virtual string GenerateDefaultPartitionKey(TReadOnlyRawLog readOnlyRawLog)
        {
            return DateTime.Today.Day.ToString();
        }

        /// <summary>
        /// Generates the default row key based on the specified <paramref name="readOnlyRawLog"/>.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log used to generate the row key.</param>
        /// <returns>The generated default row key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the value of the property 'Source' in the <paramref name="readOnlyRawLog"/> is null or empty.</exception>
        public virtual string GenerateDefaultRowKey(TReadOnlyRawLog readOnlyRawLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));
            AssertHelper.AssertNotNullNotEmptyOrThrow(readOnlyRawLog.Source, nameof(readOnlyRawLog.Source));

            return string.Format(AzLoggerConstants.DefaultRowKeyFormat, readOnlyRawLog.SeverityLevel, readOnlyRawLog.Source, Guid.NewGuid());
        }

        /// <summary>
        /// Generates a default Azure Table name based on property 'SeverityLevel' in the specified <paramref name="readOnlyRawLog"/>, 
        /// using the default behavior:
        /// "Logger" appended with the current year, month, and the following suffix:
        /// "Low" if the <see cref="ILog.SeverityLevel"/> is <see cref="SeverityLevel.Info"/> or <see cref="SeverityLevel.Warning"/>, otherwise "High".
        /// <example>
        /// "Logger200101Low"
        /// "Logger200101High"
        /// </example>
        /// </summary>
        /// <remarks>The <paramref name="partitionKey"/> and the <paramref name="rowKey"/> are reserved for inherited classes that redefine the method.</remarks>
        /// <param name="readOnlyRawLog">The read-only raw log to check.</param>
        /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
        /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
        /// <returns>The generated Azure Table name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        public virtual string GenerateDefaultLogTableName(
            TReadOnlyRawLog readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));

            var tableName = "Logger" + DateTime.Today.ToString("yyyyMM");

            if (readOnlyRawLog.SeverityLevel < SeverityLevel.Error)
                tableName += "Low";
            else
                tableName += "High";

            return tableName;
        }

        /// <summary>
        /// Generates an default Azure Blob Container names for storing log entities based on the provided <paramref name="readOnlyRawLog"/>.
        /// If <see cref="GenerateLogBlobContainerNameDel"/> is null, the blob container name will be generated using the default behavior:
        /// "azlogs-" appeded with the current year and month.
        /// <example>
        /// "azlogs-200101"
        /// </example>
        /// </summary>
        /// <remarks>The <paramref name="readOnlyRawLog"/>, <paramref name="partitionKey"/>, and the <paramref name="rowKey"/> are reserved for inherited classes that redefine the method.</remarks>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
        /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
        /// <returns>The generated default Azure Blob Container name.</returns>
        public virtual string GenerateDefaultBlobContainerName(
            TReadOnlyRawLog readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            return $"{AzLoggerConstants.DefaultAzLogBlobContainerNamePrefix}-{DateTime.Today.ToString("yyyyMM")}";
        }

        /// <summary>
        /// Generates an default Azure Blob names for storing log entities based on the provided <paramref name="readOnlyRawLog"/>.
        /// If <see cref="GenerateLogBlobNameDel"/> is null, the blob name will be generated using the default behavior:
        /// ILogEntity.SeverityLevel/ILogEntity.PartitionKey/ILogEntity.RowKey.json.
        /// <example>
        /// "Info/PartitionKey/RowKey.json"
        /// </example>
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log.</param>
        /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
        /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
        /// <returns>The generated default Azure Blob name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="partitionKey"/> or <paramref name="rowKey"/> are null or empty.</exception>
        public virtual string GenerateDefaultBlobName(
            TReadOnlyRawLog readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));
            AzRepositoryAssertHelper.AssertPartitionKeyRowKeyOrThrow(partitionKey, rowKey);

            return $"{readOnlyRawLog.SeverityLevel}/{partitionKey}/{rowKey}.json";
        }

        #endregion
    }
}
