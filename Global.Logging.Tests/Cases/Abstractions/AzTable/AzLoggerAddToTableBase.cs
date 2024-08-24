
namespace Global.Logging.Tests
{
    public abstract class AzLoggerAddToTableBase : AzLoggerTableBase
    {
        protected async Task<AzTableResponse> CommonAddToTable(
            ConcurrentMethodType concurrentMethodType,
            bool createTableIfNotExists,
            bool expectedCreateTableIfNotExists,
            string expectedTableName,
            (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)? entities = null,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = null,
            IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azSeverityLevelLogFilter = null,
            AzTableResponse? expectedAddResponse = null,
            AzTableResponse? expectedLoadTableResponse = null)
        {
            // Arrange
            if (entities == default)
                entities = EntityFactory.Create(fillVerboseLabelSet: delegateContainer?.FillVerboseLabelSetDel);

            BuildAzTableRepositoryMock(concurrentMethodType, expectedCreateTableIfNotExists, expectedTableName,
                entities!.Value.LogEntity, expectedAddResponse, expectedLoadTableResponse);

            var azLoggingService = ServiceFactory.CreateAzLoggingService(_azTableRepositoryMock, _azBlobRepositoryMock);

            var azLogger = ModelFactory.CreateAzLogger(azLoggingService,
                _defaultLogFactory,
                azSeverityLevelLogFilter ?? _defaultAzSeverityLevelLogFilter, delegateContainer);

            if (_retryOnFailures != null)
                await azLogger.SetRetryOnFailuresAsync(_retryOnFailures.Value);
            if (_maxRetryAttempts != null)
                await azLogger.SetMaxRetryAttemptsAsync(_maxRetryAttempts.Value);

            // Act
            return concurrentMethodType == ConcurrentMethodType.Sync
                ? azLogger.AddToTable(entities.Value.RawLog, createTableIfNotExists: createTableIfNotExists)
                : await azLogger.AddToTableAsync(entities.Value.RawLog, createTableIfNotExists: createTableIfNotExists, cancellationToken: _defaultCancellationToken);
        }

        protected void BuildAzTableRepositoryMock(
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateTableIfNotExists,
            string expectedTableName,
            LogEntity logEntity,
            AzTableResponse? expectedAddResponse = null,
            AzTableResponse? expectedLoadTableResponse = null)
        {
            if (concurrentMethodType == ConcurrentMethodType.Sync)
                _azTableRepositoryMock.Add(logEntity, expectedAddResponse);
            else
                _azTableRepositoryMock.AddAsync(logEntity, _defaultCancellationToken, expectedAddResponse);

            CommonBuildAzTableRepositoryMock(concurrentMethodType, expectedCreateTableIfNotExists, expectedTableName, expectedLoadTableResponse);
        }
    }
}
