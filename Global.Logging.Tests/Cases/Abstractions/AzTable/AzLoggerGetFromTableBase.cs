
namespace Global.Logging.Tests
{
    public abstract class AzLoggerGetFromTableBase : AzLoggerTableBase
    {
        protected async Task<(LogEntity ExpectedLogEntity, AzTableValueResponse<ReadOnlyLog> ActualResponse)> CommonGetFromTable(
            ConcurrentMethodType concurrentMethodType,
            bool createTableIfNotExists,
            bool expectedCreateTableIfNotExists,
            string expectedTableName,
            string tableName,
            string? expectedPartitionKey = default,
            string? expectedRowKey = default,
            (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)? entities = null,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = null,
            IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azSeverityLevelLogFilter = null,
            AzTableValueResponse<LogEntity>? expectedGetResponse = default,
            AzTableResponse? expectedLoadTableResponse = null)
        {
            // Arrange
            if (entities == default)
                entities = EntityFactory.Create(fillVerboseLabelSet: delegateContainer?.FillVerboseLabelSetDel);

            if (expectedGetResponse == default)
                expectedGetResponse = AzTableValueResponseFactory.CreateSuccessful(entities!.Value.LogEntity);

            BuildAzTableRepositoryMock(concurrentMethodType, expectedCreateTableIfNotExists, expectedTableName,
                entities!.Value.LogEntity, expectedGetResponse, expectedPartitionKey, expectedRowKey, expectedLoadTableResponse);

            var azLoggingService = ServiceFactory.CreateAzLoggingService(_azTableRepositoryMock, _azBlobRepositoryMock);

            var azLogger = ModelFactory.CreateAzLogger(azLoggingService,
                _defaultLogFactory,
                azSeverityLevelLogFilter ?? _defaultAzSeverityLevelLogFilter, delegateContainer);

            if (_retryOnFailures != null)
                await azLogger.SetRetryOnFailuresAsync(_retryOnFailures.Value);
            if (_maxRetryAttempts != null)
                await azLogger.SetMaxRetryAttemptsAsync(_maxRetryAttempts.Value);

            // Act
            return (entities!.Value.LogEntity,
                concurrentMethodType == ConcurrentMethodType.Sync
                ? azLogger.GetFromTable(tableName, entities!.Value.LogEntity.PartitionKey, entities!.Value.LogEntity.RowKey, createTableIfNotExists: createTableIfNotExists)
                : await azLogger.GetFromTableAsync(tableName, entities!.Value.LogEntity.PartitionKey, entities!.Value.LogEntity.RowKey, 
                createTableIfNotExists: createTableIfNotExists, cancellationToken: _defaultCancellationToken));
        }

        protected void BuildAzTableRepositoryMock(
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateTableIfNotExists,
            string expectedTableName,
            LogEntity expectedLogEntity,
            AzTableValueResponse<LogEntity> expectedGetResponse,
            string? expectedPartitionKey = default,
            string? expectedRowKey = default,
            AzTableResponse? expectedLoadTableResponse = null)
        {
            expectedPartitionKey ??= expectedLogEntity.PartitionKey;
            expectedRowKey ??= expectedLogEntity.RowKey;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
                _azTableRepositoryMock.Get(expectedPartitionKey, expectedRowKey, expectedGetResponse);
            else
                _azTableRepositoryMock.GetAsync(expectedPartitionKey, expectedRowKey, _defaultCancellationToken, expectedGetResponse);

            CommonBuildAzTableRepositoryMock(concurrentMethodType, expectedCreateTableIfNotExists, expectedTableName, expectedLoadTableResponse);
        }
    }
}
