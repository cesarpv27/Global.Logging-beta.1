
namespace Global.Logging.Models
{
    /// <inheritdoc cref="IAzLogger{TEx}"/>
    public interface IAzLogger : IAzLogger<Exception> { }

    /// <inheritdoc cref="IAzLogger{TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx}"/>
    public interface IAzLogger<TEx> : IAzLogger<RawLog<TEx>, ReadOnlyRawLog<TEx>, ReadOnlyLog, LogEntity, TEx>
        where TEx : Exception
    {
        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzTableResponse AddToTable(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzTableResponse> AddToTableAsync(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzTableResponse AddToTable(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzTableResponse> AddToTableAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createTableIfNotExists = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzTableResponse AddToTable(
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
            bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzTableResponse> AddToTableAsync(
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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="logLabelSet">A set of label information used to populate the corresponding label properties (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzTableResponse AddToTable(
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
            bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Table service, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="logLabelSet">A set of label information used to populate the corresponding label properties (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzTableResponse> AddToTableAsync(
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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzBlobResponse AddToBlob(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzBlobResponse> AddToBlobAsync(
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzBlobResponse AddToBlob(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzBlobResponse> AddToBlobAsync(
            string source,
            SeverityLevel severityLevel,
            string? message,
            string? category,
            Dictionary<string, string>? verbose = default,
            TEx? exception = default,
            bool? retryOnFailures = null,
            int? maxRetryAttempts = null,
            bool createBlobContainerIfNotExists = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzBlobResponse AddToBlob(
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
            bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzBlobResponse> AddToBlobAsync(
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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="logLabelSet">A set of label information used to populate the corresponding label properties (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        AzBlobResponse AddToBlob(
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
            bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <see cref="LogEntity"/> to the Azure Blob storage, created using the information provided.
        /// </summary>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <param name="message">Message associated with the log (Optional).</param>
        /// <param name="category">Category associated with the log (Optional).</param>
        /// <param name="verbose">A <see cref="Dictionary{TKey, TValue}"/> instance containing additional information associated with the log (Optional).</param>
        /// <param name="exception">Exception associated with the log (Optional).</param>
        /// <param name="logLabelSet">A set of label information used to populate the corresponding label properties (Optional).</param>
        /// <param name="callStack">Call stack associated with the source of the log (Optional).</param>
        /// <param name="callStackMethod">Call stack method associated with the source of the log (Optional).</param>
        /// <param name="callStackLine">Call stack line number associated with the source of the log (Optional).</param>
        /// <param name="callStackColumn">Call stack column number associated with the source of the log (Optional).</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If specified, it takes precedence over the value of the "RetryOnFailures" property.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If specified, it takes precedence over the value of the "MaxRetryAttempts" property.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <see cref="LogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="source"/> is null.</exception>
        Task<AzBlobResponse> AddToBlobAsync(
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
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a logging service for Azure Table Service and Azure Blob Storage.
    /// </summary>
    /// <typeparam name="TRawLog">The type of raw log.</typeparam>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <typeparam name="TReadOnlyLog">The type of read-only log.</typeparam>
    /// <typeparam name="TLogEntity">The type of log entity.</typeparam>
    /// <typeparam name="TEx">The type of the exception associated with the raw log.</typeparam>
    /// <remarks>
    /// A table name in the correct format for Azure Table Service must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters or numbers.<br/>
    /// - The name must begin with a letter.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// - Some names are reserved, including 'tables'.<br/>
    /// <br/>
    /// A blob container name in the correct format for Azure Blob Storage must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters, numbers, or the '-' character.<br/>
    /// - All characters must be lowercase.<br/>
    /// - The first and last characters must be a letter or a number.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// <br/>
    /// When adding a log, the current values of the properties or default values are always used.<br/>
    /// If a value is specified through the method parameter, it takes precedence over the property or default value.<br/>
    /// The <see cref="IAzLogger{TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx}"/> can be managed and configured using the following methods:<br/>
    /// <see cref="UpdateDelegatesAsync(DelegateContainer{TReadOnlyRawLog}, CancellationToken)"/><br/>
    /// <see cref="SetGeneratePartitionKeyDelAsync(GeneratePartitionKeyDel{TReadOnlyRawLog}?, CancellationToken)"/><br/>
    /// <see cref="SetGeneratePartitionKeyAsync(Func{TReadOnlyRawLog, string}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateRowKeyDelAsync(GenerateRowKeyDel{TReadOnlyRawLog}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateRowKeyAsync(Func{TReadOnlyRawLog, string}?, CancellationToken)"/><br/>
    /// <see cref="SetFillVerboseLabelSetDelAsync(FillVerboseLabelSetDel?, CancellationToken)"/><br/>
    /// <see cref="SetFillVerboseLabelSetAsync(Action{VerboseLabelSet, ReadOnlyDictionary{string, string}}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateLogTableNameDelAsync(GenerateLogTableNameDel{TReadOnlyRawLog}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateLogTableNameAsync(Func{TReadOnlyRawLog, string, string, string}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateLogBlobContainerNameDelAsync(GenerateLogBlobContainerNameDel{TReadOnlyRawLog}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateLogBlobContainerNameAsync(Func{TReadOnlyRawLog, string, string, string}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateLogBlobNameDelAsync(GenerateLogBlobNameDel{TReadOnlyRawLog}?, CancellationToken)"/><br/>
    /// <see cref="SetGenerateLogBlobNameAsync(Func{TReadOnlyRawLog, string, string, string}?, CancellationToken)"/><br/>
    /// <see cref="SetAzTableRetryOptionsAsync(AzRetryOptions?, CancellationToken)"/><br/>
    /// <see cref="SetAzBlobRetryOptionsAsync(AzRetryOptions?, CancellationToken)"/><br/>
    /// <see cref="SetRetryOnFailuresAsync(bool, CancellationToken)"/><br/>
    /// <see cref="GetRetryOnFailuresAsync(CancellationToken)"/><br/>
    /// <see cref="SetMaxRetryAttemptsAsync(int?, CancellationToken)"/><br/>
    /// <see cref="GetMaxRetryAttemptsAsync(CancellationToken)"/><br/>
    /// </remarks>
    public interface IAzLogger<TRawLog, TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx>
        where TRawLog : IRawLog<TEx> 
        where TReadOnlyRawLog : IReadOnlyRawLog
        where TReadOnlyLog : IReadOnlyLog
        where TLogEntity : class, ILogEntity
        where TEx : Exception
    {
        /// <summary>
        /// Asynchronously gets the flag indicating whether to retry on failures. Default value is true. 
        /// If any method has specified a value for retry on failures, then that value takes precedence
        /// over default or previously set values.
        /// </summary>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the flag indicating whether to retry on failures.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task<bool> GetRetryOnFailuresAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the flag indicating whether to retry on failures. Default value is true. 
        /// If any method has specified a value for retry on failures, then that value takes precedence
        /// over default or previously set values.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetRetryOnFailuresAsync(bool value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously gets the maximum number of retry attempts (Optional). Default values is 3. 
        /// If any method has specified a value for max retry attempts, then that value takes precedence
        /// over default or previously set values.
        /// </summary>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the maximum retry attempts.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task<int?> GetMaxRetryAttemptsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the maximum number of retry attempts (Optional). Default values is 3.
        /// If any method has specified a value for max retry attempts, then that value takes precedence
        /// over default or previously set values.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetMaxRetryAttemptsAsync(int? value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates the delegates in the logger with the values from <paramref name="delegateContainer"/>, which are used for creating partition keys, row keys, 
        /// table names, blob container names, and blob names for log entities in Azure Storage.
        /// Sets a property in <paramref name="delegateContainer"/> to null to reset it to the default behavior.
        /// </summary>
        /// <param name="delegateContainer">A class that contains delegates for generating keys and names for Azure log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="delegateContainer"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task UpdateDelegatesAsync(DelegateContainer<TReadOnlyRawLog> delegateContainer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the delegate that generates a partition key for Azure log entities. 
        /// If <paramref name="generatePartitionKeyDel"/> is null, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generatePartitionKeyDel">Delegate that generates a partition key for Azure log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGeneratePartitionKeyDelAsync(GeneratePartitionKeyDel<TReadOnlyRawLog>? generatePartitionKeyDel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the <paramref name="generatePartitionKey"/> as the delegate that generates a partition key for Azure log entities.
        /// If <paramref name="generatePartitionKey"/> is not null, the delegate will be initialized with the provided function. 
        /// Otherwise, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generatePartitionKey">Delegate that generates a partition key for Azure log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGeneratePartitionKeyAsync(Func<TReadOnlyRawLog, string>? generatePartitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the delegate that generates a row key for Azure log entities. 
        /// If <paramref name="generateRowKeyDel"/> is null, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generateRowKeyDel">Delegate that generates a row key for Azure log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateRowKeyDelAsync(GenerateRowKeyDel<TReadOnlyRawLog>? generateRowKeyDel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the <paramref name="generateRowKey"/> as the delegate that generates a row key for Azure log entities.
        /// If <paramref name="generateRowKey"/> is not null, the delegate will be initialized with the provided function.
        /// Otherwise, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generateRowKey">The function to be used as the delegate for generating row keys. Can be null.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateRowKeyAsync(Func<TReadOnlyRawLog, string>? generateRowKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the delegate for filling an instance of <see cref="IVerboseLabelSet"/> with specific data for categorization or identification purposes, 
        /// using data from the specified "Verbose" collection.
        /// The instance of <see cref="IVerboseLabelSet"/> will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.  
        /// If <paramref name="fillVerboseLabelSetDel"/> is null, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="fillVerboseLabelSetDel">Delegate for filling an instance of <see cref="IVerboseLabelSet"/>.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetFillVerboseLabelSetDelAsync(FillVerboseLabelSetDel? fillVerboseLabelSetDel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the <paramref name="fillVerboseLabelSet"/> as delegate for filling an instance of <see cref="IVerboseLabelSet"/> with specific data for categorization or identification purposes, 
        /// using data from the specified "Verbose" collection.
        /// The instance of <see cref="IVerboseLabelSet"/> will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.  
        /// If <paramref name="fillVerboseLabelSet"/> is not null, the delegate will be initialized with the provided function.
        /// Otherwise, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="fillVerboseLabelSet">The function to be used as the delegate for filling an instance of <see cref="IVerboseLabelSet"/>. Can be null.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetFillVerboseLabelSetAsync(Action<VerboseLabelSet, ReadOnlyDictionary<string, string>>? fillVerboseLabelSet, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the delegate that generates a table name for storing log entities.  
        /// If <paramref name="generateLogTableNameDel"/> is null, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// If the delegate is null, the table names will be generated using the default behavior:
        /// "Logger" appended with the current year, month, and the following suffix:
        /// "Low" if the <see cref="ILog.SeverityLevel"/> is <see cref="SeverityLevel.Info"/> or <see cref="SeverityLevel.Warning"/>, otherwise "High".
        /// <example>
        /// "Logger200101Low"
        /// "Logger200101High"
        /// </example>
        /// </summary>
        /// <param name="generateLogTableNameDel">Delegate that generates a table name for storing log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateLogTableNameDelAsync(GenerateLogTableNameDel<TReadOnlyRawLog>? generateLogTableNameDel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the <paramref name="generateLogTableName"/> as delegate that generates an Azure Table names for storing log entities. 
        /// If <paramref name="generateLogTableName"/> is not null, the delegate will be initialized with the provided function.
        /// Otherwise, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// If the delegate is null, the table names will be generated using the default behavior:
        /// "Logger" appended with the current year, month, and the following suffix:
        /// "Low" if the <see cref="ILog.SeverityLevel"/> is <see cref="SeverityLevel.Info"/> or <see cref="SeverityLevel.Warning"/>, otherwise "High".
        /// <example>
        /// "Logger200101Low"
        /// "Logger200101High"
        /// </example>
        /// </summary>
        /// <param name="generateLogTableName">The function to be used as the delegate for generating Azure Table names. Can be null.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateLogTableNameAsync(Func<TReadOnlyRawLog, string, string, string>? generateLogTableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the delegate that generates a blob container name for storing log entities. 
        /// If <paramref name="generateLogBlobContainerNameDel"/> is null, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// If the delegate is null, the blob container name will be generated using the default behavior:
        /// "azlogs-" appeded with the current year and month.
        /// <example>
        /// "azlogs-200101"
        /// </example>
        /// </summary>
        /// <param name="generateLogBlobContainerNameDel">Delegate that generates a blob container name for storing log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateLogBlobContainerNameDelAsync(GenerateLogBlobContainerNameDel<TReadOnlyRawLog>? generateLogBlobContainerNameDel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the <paramref name="generateLogBlobContainerName"/> as delegate that generates an Azure Blob Container names for storing log entities. 
        /// If <paramref name="generateLogBlobContainerName"/> is not null, the delegate will be initialized with the provided function.
        /// Otherwise, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// If the delegate is null, the blob container name will be generated using the default behavior:
        /// "azlogs-" appeded with the current year and month.
        /// <example>
        /// "azlogs-200101"
        /// </example>
        /// </summary>
        /// <param name="generateLogBlobContainerName">The function to be used as the delegate for generating Azure Blob Container names. Can be null.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateLogBlobContainerNameAsync(Func<TReadOnlyRawLog, string, string, string>? generateLogBlobContainerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the delegate that generates a blob name for storing log entities. 
        /// If <paramref name="generateLogBlobNameDel"/> is null, the delegate will be set to null.
        /// If the delegate is null, the table name will be generated using the default behavior
        /// ILogEntity.SeverityLevel/ILogEntity.PartitionKey/ILogEntity.RowKey.json.
        /// <example>
        /// "Info/PartitionKey/RowKey.json"
        /// </example>
        /// </summary>
        /// <param name="generateLogBlobNameDel">Delegate that generates a blob name for storing log entities.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateLogBlobNameDelAsync(GenerateLogBlobNameDel<TReadOnlyRawLog>? generateLogBlobNameDel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the <paramref name="generateLogBlobName"/> as delegate that generates an Azure Blob names for storing log entities. 
        /// If <paramref name="generateLogBlobName"/> is not null, the delegate will be initialized with the provided function.
        /// Otherwise, the delegate will be set to null.
        /// Set to null to reset to the default behavior.
        /// If the delegate is null, the table name will be generated using the default behavior
        /// ILogEntity.SeverityLevel/ILogEntity.PartitionKey/ILogEntity.RowKey.json.
        /// <example>
        /// "Info/PartitionKey/RowKey.json"
        /// </example>
        /// </summary>
        /// <param name="generateLogBlobName">The function to be used as the delegate for generating Azure Blob names. Can be null.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetGenerateLogBlobNameAsync(Func<TReadOnlyRawLog, string, string, string>? generateLogBlobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the specified <paramref name="azRetryOptions"/> to the <see cref="IAzLoggingService{TLogEntity}"/>.
        /// This set of options will be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new table name for the TableClient.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="azRetryOptions">The retry options instance (Optional).</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetAzTableRetryOptionsAsync(AzRetryOptions? azRetryOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously sets the specified <paramref name="azRetryOptions"/> to the <see cref="IAzLoggingService{TLogEntity}"/>. 
        /// This set of options can be specified in Azure to influence how
        /// retry attempts are made, and a failure is eligible to be retried. 
        /// These options only apply within Azure platform operations.
        /// Changes will take effect either initially or after reloading the Azure service client with a new blob container name for the BlobContainerClient.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="azRetryOptions">The retry options instance (Optional).</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the wait operation is canceled.</exception>
        Task SetAzBlobRetryOptionsAsync(AzRetryOptions? azRetryOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the specified sequence of store operations to add and retrieve log entities for Azure Table Service and Azure Blob Storage.
        /// </summary>
        /// <remarks>
        /// The store operations can be for both adding and retrieving log entities.
        /// Once the execution of the <paramref name="storeOperationSequence"/> is completed, 
        /// each store operation will contain a corresponding response associated with the result of the operation.  
        /// Each response contains the Status set to <see cref="ResponseStatus.Success"/> if log entity has been successfully added or retrieved; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.
        /// This method is slightly less efficient than the specific methods for adding or retrieving entities.
        /// </remarks>
        /// <param name="storeOperationSequence">The sequence of store operations to execute.</param>
        /// <returns>The <see cref="ExGlobalResponse"/> associated with the result of the execution. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="storeOperationSequence"/> has been successfully processed 
        /// according to the the specifications provided in the <paramref name="storeOperationSequence"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storeOperationSequence"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="storeOperationSequence"/> is empty.</exception>
        ExGlobalResponse ExecuteSequence(StoreOperationSequence<TRawLog, TReadOnlyLog, TEx> storeOperationSequence);

        /// <summary>
        /// Asynchronously executes the specified sequence of store operations to add and retrieve log entities for Azure Table Service and Azure Blob Storage.
        /// </summary>
        /// <remarks>
        /// The store operations can be for both adding and retrieving log entities.
        /// Once the execution of the <paramref name="storeOperationSequence"/> is completed, 
        /// each store operation will contain a corresponding response associated with the result of the operation.  
        /// Each response contains the Status set to <see cref="ResponseStatus.Success"/> if log entity has been successfully added or retrieved; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.
        /// This method is slightly less efficient than the specific methods for adding or retrieving entities.
        /// </remarks>
        /// <param name="storeOperationSequence">The sequence of store operations to execute.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="ExGlobalResponse"/> associated with the result of the execution. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="storeOperationSequence"/> has been successfully processed 
        /// according to the the specifications provided in the <paramref name="storeOperationSequence"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="storeOperationSequence"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="storeOperationSequence"/> is empty.</exception>
        Task<ExGlobalResponse> ExecuteSequenceAsync(StoreOperationSequence<TRawLog, TReadOnlyLog, TEx> storeOperationSequence, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <typeparamref name="TLogEntity"/> to the Azure Table service, created using the information from <paramref name="rawLog"/>.
        /// </summary>
        /// <param name="rawLog">The entity used to generate the <typeparamref name="TLogEntity"/> to add.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <typeparamref name="TLogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rawLog"/> is null.</exception>
        AzTableResponse AddToTable(TRawLog rawLog, bool? retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <typeparamref name="TLogEntity"/> to the Azure Table service, created using the information from <paramref name="rawLog"/>.
        /// </summary>
        /// <param name="rawLog">The entity used to generate the <typeparamref name="TLogEntity"/> to add.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <typeparamref name="TLogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rawLog"/> is null.</exception>
        Task<AzTableResponse> AddToTableAsync(TRawLog rawLog, bool? retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an instance of <typeparamref name="TLogEntity"/>, created using the information from <paramref name="rawLog"/>, to the Azure Blob logger.
        /// </summary>
        /// <param name="rawLog">The entity used to generate the <typeparamref name="TLogEntity"/> to add.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <typeparamref name="TLogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rawLog"/> is null.</exception>
        AzBlobResponse AddToBlob(TRawLog rawLog, bool? retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = true);

        /// <summary>
        /// Asynchronously adds an instance of <typeparamref name="TLogEntity"/>, created using the information from <paramref name="rawLog"/>, to the Azure Blob logger.
        /// </summary>
        /// <param name="rawLog">The entity used to generate the <typeparamref name="TLogEntity"/> to add.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the  <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if instance of <typeparamref name="TLogEntity"/> 
        /// has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rawLog"/> is null.</exception>
        Task<AzBlobResponse> AddToBlobAsync(TRawLog rawLog, bool? retryOnFailures = true, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the log entity from the Azure Table Service.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <returns>The <see cref="AzTableValueResponse{TReadOnlyLog}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzTableValueResponse<TReadOnlyLog> GetFromTable(string tableName, string partitionKey, string rowKey, bool? retryOnFailures = true, int? maxRetryAttempts = null, bool createTableIfNotExists = false);

        /// <summary>
        /// Asynchronously retrieve the log entity from the Azure Table Service.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="createTableIfNotExists">If true, create the Azure table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{TReadOnlyLog}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        Task<AzTableValueResponse<TReadOnlyLog>> GetFromTableAsync(string tableName, string partitionKey, string rowKey, bool? retryOnFailures = null, int? maxRetryAttempts = null, bool createTableIfNotExists = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the log entity from the Azure Blob Storage.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>The <see cref="AzBlobValueResponse{TReadOnlyLog}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        AzBlobValueResponse<TReadOnlyLog> GetFromBlob(string blobContainerName, string blobName, bool? retryOnFailures = null, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = false);

        /// <summary>
        /// Asynchronously retrieves the log entity from the Azure Blob Storage.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (Optional). 
        /// If a value is specified through the method parameter, it takes precedence over the default value.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <param name="createBlobContainerIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{TReadOnlyLog}"/> associated with the result of the operation.
        /// If the log entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested log; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        Task<AzBlobValueResponse<TReadOnlyLog>> GetFromBlobAsync(string blobContainerName, string blobName, bool? retryOnFailures = null, int? maxRetryAttempts = null, bool createBlobContainerIfNotExists = false, CancellationToken cancellationToken = default);
    }
}
