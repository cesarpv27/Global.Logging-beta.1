
namespace Global.Logging.Tests
{
    public abstract class AzLoggerGetFromBlobBase : AzLoggerBlobBase
    {
        protected async Task<(LogEntity ExpectedLogEntity, AzBlobValueResponse<ReadOnlyLog> ActualResponse)> CommonGetFromBlob(
            ConcurrentMethodType concurrentMethodType,
            bool createBlobContainerIfNotExists,
            bool expectedCreateBlobContainerIfNotExists,
            string expectedLogBlobContainerName,
            string logBlobContainerName,
            string expectedLogBlobName,
            string logBlobName,
            (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)? entities = null,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = null,
            IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azSeverityLevelLogFilter = null,
            AzBlobValueResponse<LogEntity>? expectedGetResponse = null,
            AzBlobResponse? expectedLoadBlobContainerClientResponse = null,
            AzBlobResponse? expectedLoadBlobClientResponse = null)
        {
            // Arrange
            if (entities == default)
                entities = EntityFactory.Create(fillVerboseLabelSet: delegateContainer?.FillVerboseLabelSetDel);

            if (expectedGetResponse == default)
                expectedGetResponse = AzBlobValueResponseFactory.CreateSuccessful(entities!.Value.LogEntity);

            BuildAzBlobRepositoryMock(concurrentMethodType, expectedCreateBlobContainerIfNotExists,
                expectedLogBlobContainerName, expectedLogBlobName, entities!.Value.LogEntity,
                expectedGetResponse, expectedLoadBlobContainerClientResponse, expectedLoadBlobClientResponse);

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
                ? azLogger.GetFromBlob(logBlobContainerName, logBlobName, createBlobContainerIfNotExists: createBlobContainerIfNotExists)
                : await azLogger.GetFromBlobAsync(logBlobContainerName, logBlobName, createBlobContainerIfNotExists: createBlobContainerIfNotExists, cancellationToken: _defaultCancellationToken));
        }

        protected void BuildAzBlobRepositoryMock(
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateBlobContainerIfNotExists,
            string expectedLogBlobContainerName,
            string expectedLogBlobName,
            LogEntity logEntity,
            AzBlobValueResponse<LogEntity> expectedGetResponse,
            AzBlobResponse? expectedLoadBlobContainerClientResponse = null,
            AzBlobResponse? expectedLoadBlobClientResponse = null,
            CancellationToken? getAsyncCancellationToken = null,
            CancellationToken? loadBlobContainerClientAsyncCancellationToken = null,
            CancellationToken? loadBlobClientAsyncCancellationToken = null)
        {
            getAsyncCancellationToken ??= _defaultCancellationToken;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
                _azBlobRepositoryMock.Get(default, default, expectedGetResponse);
            else
                _azBlobRepositoryMock.GetAsync(default, default, getAsyncCancellationToken.Value, expectedGetResponse);

            CommonBuildAzBlobRepositoryMock(concurrentMethodType, expectedCreateBlobContainerIfNotExists,
                expectedLogBlobContainerName, expectedLogBlobName, expectedLoadBlobContainerClientResponse, expectedLoadBlobClientResponse,
                loadBlobContainerClientAsyncCancellationToken, loadBlobClientAsyncCancellationToken);
        }
    }
}
