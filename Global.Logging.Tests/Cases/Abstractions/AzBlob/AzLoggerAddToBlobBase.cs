
namespace Global.Logging.Tests
{
    public class AzLoggerAddToBlobBase : AzLoggerBlobBase
    {
        protected bool _defaultOverwrite;

        public AzLoggerAddToBlobBase()
        {
            _defaultOverwrite = false;
        }

        protected async Task<AzBlobResponse> CommonAddToBlob(
            ConcurrentMethodType concurrentMethodType,
            bool expectedOverwrite,
            bool createBlobContainerIfNotExists,
            bool expectedCreateBlobContainerIfNotExists,
            string expectedLogBlobContainerName,
            string expectedLogBlobName,
            (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity)? entities = null,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = null,
            IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azSeverityLevelLogFilter = null,
            AzBlobValueResponse<BlobContentInfo>? expectedUploadResponse = null,
            AzBlobResponse? expectedLoadBlobContainerClientResponse = null,
            AzBlobResponse? expectedLoadBlobClientResponse = null)
        {
            // Arrange
            if (entities == default)
                entities = EntityFactory.Create(fillVerboseLabelSet: delegateContainer?.FillVerboseLabelSetDel);

            BuildAzBlobRepositoryMock(concurrentMethodType, expectedOverwrite, expectedCreateBlobContainerIfNotExists,
                expectedLogBlobContainerName, expectedLogBlobName, entities!.Value.LogEntity, 
                expectedUploadResponse, expectedLoadBlobContainerClientResponse, expectedLoadBlobClientResponse);

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
                ? azLogger.AddToBlob(entities!.Value.RawLog, createBlobContainerIfNotExists: createBlobContainerIfNotExists)
                : await azLogger.AddToBlobAsync(entities!.Value.RawLog, createBlobContainerIfNotExists: createBlobContainerIfNotExists, cancellationToken: _defaultCancellationToken);
        }

        protected void BuildAzBlobRepositoryMock(
            ConcurrentMethodType concurrentMethodType,
            bool expectedOverwrite,
            bool expectedCreateBlobContainerIfNotExists,
            string expectedLogBlobContainerName,
            string expectedLogBlobName,
            LogEntity logEntity,
            AzBlobValueResponse<BlobContentInfo>? expectedUploadResponse = null,
            AzBlobResponse? expectedLoadBlobContainerClientResponse = null,
            AzBlobResponse? expectedLoadBlobClientResponse = null,
            CancellationToken? uploadAsyncCancellationToken = null,
            CancellationToken? loadBlobContainerClientAsyncCancellationToken = null,
            CancellationToken? loadBlobClientAsyncCancellationToken = null)
        {
            uploadAsyncCancellationToken ??= _defaultCancellationToken;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
                _azBlobRepositoryMock.Upload(logEntity, default, expectedOverwrite, expectedUploadResponse);
            else
                _azBlobRepositoryMock.UploadAsync(logEntity, default, default, expectedOverwrite, uploadAsyncCancellationToken.Value, expectedUploadResponse);

            CommonBuildAzBlobRepositoryMock(concurrentMethodType, expectedCreateBlobContainerIfNotExists,
                expectedLogBlobContainerName, expectedLogBlobName, expectedLoadBlobContainerClientResponse, expectedLoadBlobClientResponse,
                loadBlobContainerClientAsyncCancellationToken, loadBlobClientAsyncCancellationToken);
        }
    }
}
