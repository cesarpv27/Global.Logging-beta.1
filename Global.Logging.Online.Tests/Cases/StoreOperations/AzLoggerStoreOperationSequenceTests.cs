
namespace Global.Logging.Online.Tests
{
    public class AzLoggerStoreOperationSequenceTests : AzLoggerBase
    {
        protected bool? _retryOnFailures;
        protected int? _maxRetryAttempts;
        protected bool _createTableIfNotExists;
        protected bool _createBlobContainerIfNotExists;

        public AzLoggerStoreOperationSequenceTests()
        {
            _retryOnFailures = false;
            _maxRetryAttempts = 1;

            _createTableIfNotExists = true;
            _createBlobContainerIfNotExists = true;
        }

        [OnlineFact]
        public async Task AddTo_GetFrom_NextAlways()
        {
            // Arrange
            await _defaultAzLogger.SetFillVerboseLabelSetDelAsync(AzLoggerHelper.GenerateDefaultFillVerboseLabelSet);
            var defaultRawLog = AzLoggerHelper.GenerateDefaultRawLog();
            var sequenceExecutionType = SequenceExecutionType.NextAlways;

            // Arrange AddTo
            var storeOperationSequenceAddTo = new StoreOperationSequence<RawLog<Exception>, ReadOnlyLog, Exception>();
            
            storeOperationSequenceAddTo.AddNext(
                new AddToTableOperation<RawLog<Exception>, Exception>(
                    defaultRawLog,
                    sequenceExecutionType,
                    _retryOnFailures!.Value,
                    _maxRetryAttempts,
                    _createTableIfNotExists));

            storeOperationSequenceAddTo.AddNext(
                new AddToBlobOperation<RawLog<Exception>, Exception>(
                    defaultRawLog,
                    sequenceExecutionType,
                    _retryOnFailures!.Value,
                    _maxRetryAttempts,
                    _createBlobContainerIfNotExists));

            // Act sync AddTo
            _defaultAzLogger.ExecuteSequence(storeOperationSequenceAddTo);

            // Assert sync AddTo
            foreach (var operation in storeOperationSequenceAddTo.EnumerateAddToTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequenceAddTo.EnumerateAddToBlobOperations())
                Asserts.SuccessResponse(operation.Response);

            // Act async AddTo
            await _defaultAzLogger.ExecuteSequenceAsync(storeOperationSequenceAddTo, _defaultCancellationToken);

            // Assert async AddTo
            foreach (var operation in storeOperationSequenceAddTo.EnumerateAddToTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequenceAddTo.EnumerateAddToBlobOperations())
                Asserts.SuccessResponse(operation.Response);

            // Arrange GetFrom

            Assert.True(storeOperationSequenceAddTo.EnumerateAddToTableOperations().First().Response!.TryGetMessage(Constants.TableNameMessageKey, out string? tableName));
            Assert.True(storeOperationSequenceAddTo.EnumerateAddToTableOperations().First().Response!.TryGetMessage(Constants.PartitionKeyMessageKey, out string? partitionKey));
            Assert.True(storeOperationSequenceAddTo.EnumerateAddToTableOperations().First().Response!.TryGetMessage(Constants.RowKeyMessageKey, out string? rowKey));

            Assert.True(storeOperationSequenceAddTo.EnumerateAddToBlobOperations().First().Response!.TryGetMessage(Constants.BlobContainerNameMessageKey, out string? blobContainerName));
            Assert.True(storeOperationSequenceAddTo.EnumerateAddToBlobOperations().First().Response!.TryGetMessage(Constants.BlobNameMessageKey, out string? blobName));

            var storeOperationSequenceGetFrom = new StoreOperationSequence<RawLog<Exception>, ReadOnlyLog, Exception>();

            storeOperationSequenceGetFrom.AddNext(
                new GetFromTableOperation<ReadOnlyLog>(
                    tableName!,
                    partitionKey!,
                    rowKey!,
                    sequenceExecutionType,
                    _retryOnFailures!.Value,
                    _maxRetryAttempts,
                    _createTableIfNotExists));

            storeOperationSequenceGetFrom.AddNext(
                new GetFromBlobOperation<ReadOnlyLog>(
                    blobContainerName!,
                    blobName!,
                    sequenceExecutionType,
                    _retryOnFailures!.Value,
                    _maxRetryAttempts,
                    _createBlobContainerIfNotExists));

            // Act sync GetFrom
            _defaultAzLogger.ExecuteSequence(storeOperationSequenceGetFrom);

            // Assert sync
            foreach (var operation in storeOperationSequenceGetFrom.EnumerateGetFromTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequenceGetFrom.EnumerateGetFromBlobOperations())
                Asserts.SuccessResponse(operation.Response);

            // Act async GetFrom
            await _defaultAzLogger.ExecuteSequenceAsync(storeOperationSequenceGetFrom, _defaultCancellationToken);

            // Assert async GetFrom
            foreach (var operation in storeOperationSequenceGetFrom.EnumerateGetFromTableOperations())
                Asserts.SuccessResponse(operation.Response);

            foreach (var operation in storeOperationSequenceGetFrom.EnumerateGetFromBlobOperations())
                Asserts.SuccessResponse(operation.Response);
        }
    }
}
