
namespace Global.Logging.Online.Tests
{
    internal static class AzLoggerHelper
    {
        #region AzLogger

        public static AzLogger CreateAzLogger(string connectionString)
        {
            var azTableRepository = CreateAzTableReponsitory(connectionString);
            var azBlobRepository = CreateAzBlobReponsitory(connectionString);

            var azTableService = CreateAzTableService(azTableRepository);
            var azBlobService = CreateAzBlobService(azBlobRepository);

            var azLoggingService = CreateAzLoggingService(azTableService, azBlobService);
            var logFactory = CreateLogFactory();
            var azLogFilter = CreateAzLogFilter();

            return CreateAzLogger(azLoggingService, logFactory, azLogFilter);
        }

        public static AzLogger CreateAzLogger(
            IAzLoggingService<LogEntity> azLoggingService,
            ILogFactory<ReadOnlyRawLog<Exception>, ReadOnlyLog, LogEntity, Exception> logFactory,
            IAzLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>? azLogFilter = default,
            DelegateContainer<ReadOnlyRawLog<Exception>>? delegateContainer = default)
        {
            return new AzLogger(azLoggingService, logFactory, azLogFilter, delegateContainer);
        }

        #endregion

        #region Repositories

        public static IAzTableRepository<LogEntity> CreateAzTableReponsitory(string connectionString)
        {
            return new AzTableRepository<LogEntity>(connectionString);
        }

        public static IAzBlobRepository<LogEntity> CreateAzBlobReponsitory(string connectionString)
        {
            return new AzBlobRepository<LogEntity>(connectionString);
        }

        #endregion

        #region Services

        public static IAzTableService<LogEntity> CreateAzTableService(IAzTableRepository<LogEntity> azTableRepository)
        {
            return new AzTableService<LogEntity>(azTableRepository);
        }

        public static IAzBlobService<LogEntity> CreateAzBlobService(IAzBlobRepository<LogEntity> azBlobRepository)
        {
            return new AzBlobService<LogEntity>(azBlobRepository);
        }

        public static IAzLoggingService<LogEntity> CreateAzLoggingService(
            IAzTableService<LogEntity> azTableService,
            IAzBlobService<LogEntity> azBlobService)
        {
            return new AzLoggingService<LogEntity>(azTableService, azBlobService);
        }

        #endregion

        #region LogFactory

        public static ILogFactory<ReadOnlyRawLog<Exception>, ReadOnlyLog, LogEntity, Exception> CreateLogFactory()
        {
            return new LogFactory<Exception>();
        }

        #endregion

        #region AzLogFilter

        public static IAzLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog> CreateAzLogFilter()
        {
            return new AzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>();
        }

        #endregion

        #region LogLabelSet

        public static ILogLabelSet GenerateDefaultLogLabelSet()
        {
            var partialRawLog = new RawLog<Exception>(Constants.DefaultSourceMessage, SeverityLevel.Info, Constants.DefaultMessage);

            partialRawLog.Label0 = Constants.DefaultLabel0Message;
            partialRawLog.Label1 = Constants.DefaultLabel1Message;
            partialRawLog.Label2 = Constants.DefaultLabel2Message;
            partialRawLog.Label3 = Constants.DefaultLabel3Message;
            partialRawLog.Label4 = Constants.DefaultLabel4Message;
            partialRawLog.Label5 = Constants.DefaultLabel5Message;
            partialRawLog.Label6 = Constants.DefaultLabel6Message;
            partialRawLog.Label7 = Constants.DefaultLabel7Message;
            partialRawLog.Label8 = Constants.DefaultLabel8Message;
            partialRawLog.Label9 = Constants.DefaultLabel9Message;

            return partialRawLog;
        }

        #endregion

        #region Default RawLog

        public static RawLog<Exception> GenerateDefaultRawLog()
        {
            var rawLog = (RawLog<Exception>)(GenerateDefaultLogLabelSet());

            rawLog.SeverityLevel = SeverityLevel.FatalException;
            rawLog.Category = Constants.DefaultCategory;
            rawLog.Verbose = RawLogHelper.verbose;

            try
            {
                throw new ArgumentException(Constants.TestExceptionMessage);
            }
            catch (Exception e)
            {
                rawLog.Exception = e;
            }

            rawLog.CallStack = Constants.DefaultCallStackMessage;
            rawLog.CallStackMethod = Constants.DefaultCallStackMethodMessage;
            rawLog.CallStackLine = Constants.DefaultCallStackLineMessage;
            rawLog.CallStackColumn = Constants.DefaultCallStackColumnMessage;

            return rawLog;
        }

        #endregion

        #region FillVerboseLabelSet

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

        #endregion
    }
}
