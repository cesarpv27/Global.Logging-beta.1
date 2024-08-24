
namespace Global.Logging.Tests
{
    public abstract class AzLoggerBlobBase : AzLoggerBase
    {
        protected GenerateLogBlobContainerNameDel<ReadOnlyRawLog<Exception>> _generateDefaultLogBlobContainerName;
        protected GenerateLogBlobContainerNameDel<ReadOnlyRawLog<Exception>> _generateConstantLogBlobContainerName;

        protected GenerateLogBlobNameDel<ReadOnlyRawLog<Exception>> _generateDefaultLogBlobName;
        protected GenerateLogBlobNameDel<ReadOnlyRawLog<Exception>> _generateConstantLogBlobName;

        protected BlobContentInfo _defaultBlobContentInfo;

        protected readonly string _constantLogBlobContainerName;
        protected readonly string _constantLogBlobName;

        protected bool _defaultCreateBlobContainerIfNotExists;
        protected bool _defaultExpectedCreateBlobContainerIfNotExists;

        public AzLoggerBlobBase()
        {
            _generateDefaultLogBlobContainerName = ServiceHelper.GenerateDefaultLogBlobContainerName;
            _generateConstantLogBlobContainerName = ServiceHelper.GenerateConstantLogBlobContainerName;

            _generateDefaultLogBlobName = ServiceHelper.GenerateDefaultLogBlobName;
            _generateConstantLogBlobName = ServiceHelper.GenerateConstantLogBlobName;

            _defaultBlobContentInfo = RepositoryHelper.CreateDefaultBlobContentInfo();

#pragma warning disable CS8625
            _constantLogBlobContainerName = ServiceHelper.GenerateConstantLogBlobContainerName(default, default, default);
            _constantLogBlobName = ServiceHelper.GenerateConstantLogBlobName(default, default, default);
#pragma warning restore
            
            _defaultCreateBlobContainerIfNotExists = true;
            _defaultExpectedCreateBlobContainerIfNotExists = true;
        }

        protected void CommonBuildAzBlobRepositoryMock(
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateBlobContainerIfNotExists,
            string expectedLogBlobContainerName,
            string expectedLogBlobName,
            AzBlobResponse? expectedLoadBlobContainerClientResponse = null,
            AzBlobResponse? expectedLoadBlobClientResponse = null,
            CancellationToken? loadBlobContainerClientAsyncCancellationToken = null,
            CancellationToken? loadBlobClientAsyncCancellationToken = null)
        {
            loadBlobContainerClientAsyncCancellationToken ??= _defaultCancellationToken;
            loadBlobClientAsyncCancellationToken ??= _defaultCancellationToken;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
            {
                _azBlobRepositoryMock.LoadBlobContainerClient(expectedLogBlobContainerName, expectedCreateBlobContainerIfNotExists, expectedLoadBlobContainerClientResponse);
                _azBlobRepositoryMock.LoadBlobClient(expectedLogBlobName, expectedLoadBlobClientResponse);
            }
            else
            {
                _azBlobRepositoryMock.LoadBlobContainerClientAsync(expectedLogBlobContainerName, expectedCreateBlobContainerIfNotExists,
                    loadBlobContainerClientAsyncCancellationToken.Value, expectedLoadBlobContainerClientResponse);
                _azBlobRepositoryMock.LoadBlobClientAsync(expectedLogBlobName, loadBlobClientAsyncCancellationToken.Value, expectedLoadBlobClientResponse);
            }
        }
    }
}
