
namespace Global.Logging.Tests
{
    public abstract class AzLoggerTableBase : AzLoggerBase
    {
        protected readonly GenerateLogTableNameDel<ReadOnlyRawLog<Exception>> _generateDefaultLogTableName;
        protected readonly GenerateLogTableNameDel<ReadOnlyRawLog<Exception>> _generateConstantLogTableName;
        protected readonly string _constantLogTableName;

        public AzLoggerTableBase()
        {
            _generateDefaultLogTableName = ServiceHelper.GenerateDefaultLogTableName;
            _generateConstantLogTableName = ServiceHelper.GenerateConstantLogTableName;

#pragma warning disable CS8625
            _constantLogTableName = ServiceHelper.GenerateConstantLogTableName(default, default, default);
#pragma warning restore
        }

        protected void CommonBuildAzTableRepositoryMock(
            ConcurrentMethodType concurrentMethodType,
            bool expectedCreateTableIfNotExists,
            string expectedTableName,
            AzTableResponse? expectedLoadTableClientResponse = null,
            CancellationToken? loadTableClientAsyncCancellationToken = null)
        {
            loadTableClientAsyncCancellationToken ??= _defaultCancellationToken;

            if (concurrentMethodType == ConcurrentMethodType.Sync)
                _azTableRepositoryMock.LoadTableClient(expectedTableName, expectedCreateTableIfNotExists, expectedLoadTableClientResponse);
            else
                _azTableRepositoryMock.LoadTableClientAsync(expectedTableName, expectedCreateTableIfNotExists, loadTableClientAsyncCancellationToken.Value, expectedLoadTableClientResponse);
        }
    }
}
