
namespace Global.Logging.Tests
{
    internal static class EntityFactory
    {
        public static (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity) Create(
            string? logEntityPartitionKey = default,
            string? logEntityRowKey = default,
            string? source = default,
            SeverityLevel? severityLevel = default,
            string? message = default,
            string? category = default,
            Dictionary<string, string>? verbose = default,
            Exception? exception = default,
            string? callStack = default,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            FillVerboseLabelSetDel? fillVerboseLabelSet = default)
        {
            return Create(
                logEntityPartitionKey ?? EntityHelper.GenerateConstantPartitionKey(),
                logEntityRowKey ?? EntityHelper.GenerateConstantRowKey(),
                source ?? RawLogHelper.source,
                severityLevel  ?? RawLogHelper.severityLevel,
                message,
                category,
                verbose,
                exception,
                callStack,
                callStackMethod,
                callStackLine,
                callStackColumn,
                fillVerboseLabelSet);
        }

        //public static (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity) Create(
        //    string source,
        //    SeverityLevel severityLevel,
        //    string? message = default,
        //    string? category = default,
        //    Dictionary<string, string>? verbose = default,
        //    Exception? exception = default,
        //    string? callStack = default,
        //    string? callStackMethod = default,
        //    int? callStackLine = default,
        //    int? callStackColumn = default,
        //    FillVerboseLabelSetDel? fillVerboseLabelSet = default)
        //{
        //    AssertHelper.AssertNotNullNotEmptyOrThrow(source, nameof(source));

        //    var rawLog = CreateRawLog(
        //        source,
        //        severityLevel,
        //        message ?? DefaultRawLog.message,
        //        category ?? DefaultRawLog.category,
        //        verbose ?? DefaultRawLog.verbose,
        //        exception ?? DefaultRawLog.exception,
        //        callStack ?? DefaultRawLog.callStack,
        //        callStackMethod ?? DefaultRawLog.callStackMethod,
        //        callStackLine ?? DefaultRawLog.callStackLine,
        //        callStackColumn ?? DefaultRawLog.callStackColumn);

        //    var readOnlyRawLog = CreateRadOnlyRawLog(rawLog);

        //    var logEntity = CreateLogEntity(
        //        EntityHelper.GenerateConstantPartitionKey(),
        //        EntityHelper.GenerateConstantRowKey(),
        //        source,
        //        severityLevel,
        //        rawLog,
        //        ServiceHelper.BuildVerboseLabelSet(fillVerboseLabelSet, readOnlyRawLog.Verbose));

        //    var readOnlyLog = CreateRadOnlyLog(logEntity);

        //    return (rawLog, readOnlyRawLog, readOnlyLog, logEntity);
        //}

        public static (RawLog<Exception> RawLog, ReadOnlyRawLog<Exception> ReadOnlyRawLog, ReadOnlyLog ReadOnlyLog, LogEntity LogEntity) Create(
            string logEntityPartitionKey,
            string logEntityRowKey,
            string source,
            SeverityLevel severityLevel,
            string? message = default,
            string? category = default,
            Dictionary<string, string>? verbose = default,
            Exception? exception = default,
            string? callStack = default,
            string? callStackMethod = default,
            int? callStackLine = default,
            int? callStackColumn = default,
            FillVerboseLabelSetDel? fillVerboseLabelSet = default)
        {

            var rawLog = CreateRawLog(
                source,
                severityLevel,
                message ?? RawLogHelper.message,
                category ?? RawLogHelper.category,
                verbose ?? RawLogHelper.verbose,
                exception ?? RawLogHelper.exception,
                callStack ?? RawLogHelper.callStack,
                callStackMethod ?? RawLogHelper.callStackMethod,
                callStackLine ?? RawLogHelper.callStackLine,
                callStackColumn ?? RawLogHelper.callStackColumn);

            var readOnlyRawLog = CreateRadOnlyRawLog(rawLog);

            var logEntity = CreateLogEntity(
                logEntityPartitionKey,
                logEntityRowKey,
                readOnlyRawLog,
                fillVerboseLabelSet);

            var readOnlyLog = CreateRadOnlyLog(logEntity);

            return (rawLog, readOnlyRawLog, readOnlyLog, logEntity);
        }

        public static RawLog<Exception> CreateRawLog(
            string source,
            SeverityLevel severityLevel,
            string message,
            string category,
            Dictionary<string, string> verbose,
            Exception exception,
            string callStack,
            string callStackMethod,
            int callStackLine,
            int callStackColumn,
            bool includeConstantLogLabelSet = true)
        {
            var rawLog = new RawLog<Exception>(
                source,
                severityLevel,
                message,
                category,
                verbose,
                exception,
                callStack,
                callStackMethod,
                callStackLine,
                callStackColumn);

            if (includeConstantLogLabelSet)
            {
                rawLog.Label0 = Constants.ConstantLogLabel + 0;
                rawLog.Label1 = Constants.ConstantLogLabel + 1;
                rawLog.Label2 = Constants.ConstantLogLabel + 2;
                rawLog.Label3 = Constants.ConstantLogLabel + 3;
                rawLog.Label4 = Constants.ConstantLogLabel + 4;
                rawLog.Label5 = Constants.ConstantLogLabel + 5;
                rawLog.Label6 = Constants.ConstantLogLabel + 6;
                rawLog.Label7 = Constants.ConstantLogLabel + 7;
                rawLog.Label8 = Constants.ConstantLogLabel + 8;
                rawLog.Label9 = Constants.ConstantLogLabel + 9;
            }

            return rawLog;
        }

        public static ReadOnlyRawLog<Exception> CreateRadOnlyRawLog(RawLog<Exception> rawLog)
        {
            var createReadOnlyRawLogResponse = LogFactory.Create(rawLog);
            if (createReadOnlyRawLogResponse.Status != ResponseStatus.Success)
                throw new Exception($"ResponseStatus was {createReadOnlyRawLogResponse.Status} creating {nameof(ReadOnlyRawLog<Exception>)}.");

            return createReadOnlyRawLogResponse.Value!;
        }

        public static LogEntity CreateLogEntity(
            string logEntityPartitionKey,
            string logEntityRowKey,
            ReadOnlyRawLog<Exception> readOnlyRawLog,
            FillVerboseLabelSetDel? fillVerboseLabelSet = default)
        {
            if (fillVerboseLabelSet == default)
                fillVerboseLabelSet = ServiceHelper.GenerateNotFillVerboseLabelSet;

            var createReadOnlyRawLogResponse = LogFactory.Create(readOnlyRawLog, e => logEntityPartitionKey, e => logEntityRowKey, fillVerboseLabelSet);
            if (createReadOnlyRawLogResponse.Status != ResponseStatus.Success)
                throw new Exception($"ResponseStatus was {createReadOnlyRawLogResponse.Status} creating {nameof(ReadOnlyRawLog<Exception>)}.");

            return createReadOnlyRawLogResponse.Value!;
        }

        public static ReadOnlyLog CreateRadOnlyLog(LogEntity logEntity)
        {
            var createReadOnlyLogResponse = LogFactory.Create(logEntity);
            if (createReadOnlyLogResponse.Status == ResponseStatus.Failure)
                throw new Exception($"ResponseStatus was {createReadOnlyLogResponse.Status} creating {nameof(ReadOnlyLog)}.");

            return createReadOnlyLogResponse.Value!;
        }

        private static LogFactory<Exception> LogFactory { get; } = ServiceHelper.GenerateLogFactory();
    }
}
