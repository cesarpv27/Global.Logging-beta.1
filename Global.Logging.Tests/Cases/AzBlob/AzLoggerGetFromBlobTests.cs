
namespace Global.Logging.Tests
{
    public class AzLoggerGetFromBlobTests : AzLoggerGetFromBlobBase
    {
        [Fact]
        public async Task GetFromBlob()
        {
            // Act
            var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync, 
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobContainerName,
                _constantLogBlobName, _constantLogBlobName,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobContainerName,
                _constantLogBlobName, _constantLogBlobName,
                delegateContainer: _delegateContainer);

            // Act & Assert
            LoggerAsserts.SuccessBlobValueResponse(syncGetFromResponse.ExpectedLogEntity, syncGetFromResponse.ActualResponse, Extensions.CompareTo);
            LoggerAsserts.SuccessBlobValueResponse(asyncGetFromResponse.ExpectedLogEntity, asyncGetFromResponse.ActualResponse, Extensions.CompareTo);
        }

        [Fact]
        public async Task NullLogBlobContainerName()
        {
            // Arrange with null logBlobContainerName
            var entities = EntityFactory.Create();

            string logBlobContainerName = _generateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
            string logBlobName = _generateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

#pragma warning disable CS8625

            // Act & Assert
            var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync, 
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                logBlobContainerName, default,
                logBlobName, logBlobName, 
                entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async, 
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                logBlobContainerName, default,
                logBlobName, logBlobName,
                entities,
                delegateContainer: _delegateContainer);

#pragma warning restore

            // Assert
            Asserts.FailedResponse<AzBlobValueResponse<ReadOnlyLog>, ArgumentException>(syncGetFromResponse.ActualResponse, MockConstants.BlobContainerNameMessageKey);
            Asserts.FailedResponse<AzBlobValueResponse<ReadOnlyLog>, ArgumentException>(asyncGetFromResponse.ActualResponse, MockConstants.BlobContainerNameMessageKey);
        }

        [Fact]
        public async Task NullLogBlobName()
        {
            // Arrange with null logBlobContainerName
            var entities = EntityFactory.Create();

            string logBlobContainerName = _generateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);
            string logBlobName = _generateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

#pragma warning disable CS8625

            // Act & Assert
            var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                logBlobContainerName, logBlobContainerName,
                logBlobName, default,
                entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                logBlobContainerName, logBlobContainerName,
                logBlobName, default,
                entities,
                delegateContainer: _delegateContainer);

#pragma warning restore

            // Assert
            Asserts.FailedResponse<AzBlobValueResponse<ReadOnlyLog>, ArgumentException>(syncGetFromResponse.ActualResponse, MockConstants.BlobNameMessageKey);
            Asserts.FailedResponse<AzBlobValueResponse<ReadOnlyLog>, ArgumentException>(asyncGetFromResponse.ActualResponse, MockConstants.BlobNameMessageKey);
        }

        [Fact]
        public async Task DiferentLogBlobContainerName()
        {
            // Arrange with null tableName
            var entities = EntityFactory.Create();

            string expectedLogBlobContainerName = _generateDefaultLogBlobContainerName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

#pragma warning disable CS8625

            // Act
            var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync, 
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                expectedLogBlobContainerName, _constantLogBlobContainerName,
                _constantLogBlobName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async, 
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                expectedLogBlobContainerName, _constantLogBlobContainerName,
                _constantLogBlobName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer);

#pragma warning restore

            // Assert
            Asserts.FailedResponseWithMessage(syncGetFromResponse.ActualResponse, MockConstants.BlobContainerNameMessageKey, expectedLogBlobContainerName);
            Asserts.FailedResponseWithMessage(asyncGetFromResponse.ActualResponse, MockConstants.BlobContainerNameMessageKey, expectedLogBlobContainerName);
        }

        [Fact]
        public async Task DiferentLogBlobName()
        {
            // Arrange with null tableName
            var entities = EntityFactory.Create();

            string expectedLogBlobName = _generateDefaultLogBlobName(entities.ReadOnlyRawLog, entities.LogEntity.PartitionKey, entities.LogEntity.RowKey);

#pragma warning disable CS8625

            // Act
            var syncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Sync,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobContainerName,
                expectedLogBlobName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer);

            var asyncGetFromResponse = await CommonGetFromBlob(ConcurrentMethodType.Async,
                _defaultCreateBlobContainerIfNotExists, _defaultExpectedCreateBlobContainerIfNotExists,
                _constantLogBlobContainerName, _constantLogBlobContainerName,
                expectedLogBlobName, _constantLogBlobName,
                entities,
                delegateContainer: _delegateContainer);

#pragma warning restore

            // Assert
            Asserts.FailedResponseWithMessage(syncGetFromResponse.ActualResponse, MockConstants.BlobNameMessageKey, expectedLogBlobName);
            Asserts.FailedResponseWithMessage(asyncGetFromResponse.ActualResponse, MockConstants.BlobNameMessageKey, expectedLogBlobName);
        }
    }
}
