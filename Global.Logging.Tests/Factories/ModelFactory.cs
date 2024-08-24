
namespace Global.Logging.Tests
{
    internal static class ModelFactory
    {
        public static AzLogger CreateAzLogger(
            IAzLoggingService<LogEntity> azLoggingService,
            ILogFactory<ReadOnlyRawLog<Exception>, ReadOnlyLog, LogEntity, Exception> logFactory,
            IAzLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azLogFilter = default,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = default)
        {
            return new AzLogger(azLoggingService, logFactory, azLogFilter, delegateContainer);
        }

        public static async Task<AzLogger> CreateAzLoggerAsync(
            IAzLoggingService<LogEntity> azLoggingService,
            ILogFactory<ReadOnlyRawLog<Exception>, ReadOnlyLog, LogEntity, Exception> logFactory,
            IAzLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azLogFilter,
            GeneratePartitionKeyDel<ReadOnlyRawLog<Exception>> generatePartitionKeyDel,
            GenerateRowKeyDel<ReadOnlyRawLog<Exception>> generateRowKeyDel)
        {
            var result = CreateAzLogger(azLoggingService, logFactory, azLogFilter);

            await result.SetGeneratePartitionKeyDelAsync(generatePartitionKeyDel);
            await result.SetGenerateRowKeyDelAsync(generateRowKeyDel);

            return result;
        }
    }
}
