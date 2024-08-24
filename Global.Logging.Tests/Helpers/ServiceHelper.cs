
namespace Global.Logging.Tests
{
    internal static class ServiceHelper
    {
        #region Table delegates

        public static string GenerateDefaultLogTableName(
            ReadOnlyRawLog<Exception> readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));

            return GenerateDefaultLogTableName(readOnlyRawLog.SeverityLevel);
        }

        public static string GenerateDefaultLogTableName(SeverityLevel severityLevel)
        {
            var tableName = "Logger" + DateTime.Today.ToString("yyyyMM");

            if (severityLevel < SeverityLevel.Error)
                tableName += "Low";
            else
                tableName += "High";

            return tableName;
        }
        public static string GenerateConstantLogTableName(ReadOnlyRawLog<Exception> readOnlyRawLog, string partitionKey, string rowKey) => "ConstantLogTableName";

        #endregion

        #region Blob delegates

        public static string GenerateDefaultLogBlobContainerName(ReadOnlyRawLog<Exception> readOnlyRawLog, string partitionKey, string rowKey) => $"azlogs-{DateTime.Today.ToString("yyyyMM")}";

        public static string GenerateConstantLogBlobContainerName(ReadOnlyRawLog<Exception> readOnlyRawLog, string partitionKey, string rowKey) => "constantblobcontainername";

        public static string GenerateDefaultLogBlobName(ReadOnlyRawLog<Exception> readOnlyRawLog, string partitionKey, string rowKey) => $"{readOnlyRawLog.SeverityLevel}/{partitionKey}/{rowKey}.json";

        public static string GenerateConstantLogBlobName(ReadOnlyRawLog<Exception> readOnlyRawLog, string partitionKey, string rowKey) => "/ConstantBlobName";

        #endregion

        #region VerboseLabelSet

        public static void GenerateDefaultFillVerboseLabelSet(
            VerboseLabelSet verboseLabelSet, 
            ReadOnlyDictionary<string, string> verbose)
        {
            AssertHelper.AssertNotNullOrThrow(verboseLabelSet, nameof(verboseLabelSet));
            AssertHelper.AssertNotNullOrThrow(verbose, nameof(verbose));

            verboseLabelSet.VerboseLabel0 = verbose[RawLogHelper.VerboseKeyPrefix + 0];
            verboseLabelSet.VerboseLabel1 = verbose[RawLogHelper.VerboseKeyPrefix + 1];
            verboseLabelSet.VerboseLabel2 = verbose[RawLogHelper.VerboseKeyPrefix + 2];
            verboseLabelSet.VerboseLabel3 = verbose[RawLogHelper.VerboseKeyPrefix + 3];
            verboseLabelSet.VerboseLabel4 = verbose[RawLogHelper.VerboseKeyPrefix + 4];
            verboseLabelSet.VerboseLabel5 = verbose[RawLogHelper.VerboseKeyPrefix + 5];
            verboseLabelSet.VerboseLabel6 = verbose[RawLogHelper.VerboseKeyPrefix + 6];
            verboseLabelSet.VerboseLabel7 = verbose[RawLogHelper.VerboseKeyPrefix + 7];
            verboseLabelSet.VerboseLabel8 = verbose[RawLogHelper.VerboseKeyPrefix + 8];
            verboseLabelSet.VerboseLabel9 = verbose[RawLogHelper.VerboseKeyPrefix + 9];
        }

        public static void GenerateConstantFillVerboseLabelSet(
            VerboseLabelSet verboseLabelSet,
            ReadOnlyDictionary<string, string> verbose)
        {
            AssertHelper.AssertNotNullOrThrow(verboseLabelSet, nameof(verboseLabelSet));
            AssertHelper.AssertNotNullOrThrow(verbose, nameof(verbose));

            verboseLabelSet.VerboseLabel0 = Constants.ConstantVerboseLabel + 0;
            verboseLabelSet.VerboseLabel1 = Constants.ConstantVerboseLabel + 1;
            verboseLabelSet.VerboseLabel2 = Constants.ConstantVerboseLabel + 2;
            verboseLabelSet.VerboseLabel3 = Constants.ConstantVerboseLabel + 3;
            verboseLabelSet.VerboseLabel4 = Constants.ConstantVerboseLabel + 4;
            verboseLabelSet.VerboseLabel5 = Constants.ConstantVerboseLabel + 5;
            verboseLabelSet.VerboseLabel6 = Constants.ConstantVerboseLabel + 6;
            verboseLabelSet.VerboseLabel7 = Constants.ConstantVerboseLabel + 7;
            verboseLabelSet.VerboseLabel8 = Constants.ConstantVerboseLabel + 8;
            verboseLabelSet.VerboseLabel9 = Constants.ConstantVerboseLabel + 9;
        }

        public static void GenerateNotFillVerboseLabelSet(
            VerboseLabelSet verboseLabelSet,
            ReadOnlyDictionary<string, string> verbose)
        {
        }

        #endregion

        #region AzSeverityLevelLogFilter

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAzSeverityLevelLogFilter()
        {
            return new AzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>();
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAzSeverityLevelLogFilter(
            IsLoggingAllowedDel<ReadOnlyRawLog<Exception>>? isWritingAllowedDel,
            IsLoggingAllowedDel<ReadOnlyLog>? isReadingAllowedDel)
        {
            var result = GenerateAzSeverityLevelLogFilter();
            result.IsWritingAllowed = isWritingAllowedDel;
            result.IsReadingAllowed = isReadingAllowedDel;

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateDefaultAzSeverityLevelLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(default, default);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowAllLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => true, readOnlyRawLog => true);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateDenyAllLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => false, readOnlyRawLog => false);
        }

        #region Writing

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowAllForWritingLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => true, readOnlyRawLog => false);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateDenyAllForWritingLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => false, readOnlyRawLog => true);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowSeverityLevelForWritingLogFilter(SeverityLevel severityLevel)
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => readOnlyRawLog.SeverityLevel == severityLevel, readOnlyRawLog => false);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowInfoSeverityLevelForWritingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowInfoSeverityLevelForWriting();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowLowSeverityLevelForWritingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowLowSeverityLevelForWriting();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowHighSeverityLevelForWritingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowHighSeverityLevelForWriting();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowErrorSeverityLevelForWritingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowErrorSeverityLevelForWriting();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowExceptionSeverityLevelForWritingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowExceptionSeverityLevelForWriting();

            return result;
        }

        #endregion

        #region Reading

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowAllForReadingLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => false, readOnlyRawLog => true);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateDenyAllForReadingLogFilter()
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => true, readOnlyRawLog => false);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowSeverityLevelForReadingLogFilter(SeverityLevel severityLevel)
        {
            return GenerateAzSeverityLevelLogFilter(readOnlyRawLog => false, readOnlyRawLog => readOnlyRawLog.SeverityLevel == severityLevel);
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowInfoSeverityLevelForReadingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowInfoSeverityLevelForReading();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowLowSeverityLevelForReadingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowLowSeverityLevelForReading();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowHighSeverityLevelForReadingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowHighSeverityLevelForReading();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowErrorSeverityLevelForReadingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowErrorSeverityLevelForReading();

            return result;
        }

        public static IAzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> GenerateAllowExceptionSeverityLevelForReadingLogFilter()
        {
            var result = GenerateDenyAllLogFilter();
            result.SetAllowExceptionSeverityLevelForReading();

            return result;
        }

        #endregion

        #endregion

        #region LogFactory

        public static LogFactory<Exception> GenerateLogFactory()
        {
            return new LogFactory<Exception>();
        }

        #endregion
    }
}
