
namespace Global.Logging.Factories
{
    /// <inheritdoc/>
    /// <summary>
    /// Represents a factory for creating log entities.
    /// </summary>
    public class LogFactory<TEx> : ILogFactory<ReadOnlyRawLog<TEx>, ReadOnlyLog, LogEntity, TEx> where TEx : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFactory{TEx}"/> class.
        /// </summary>
        public LogFactory() { }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public virtual ExGlobalValueResponse<LogEntity> Create(
            ReadOnlyRawLog<TEx> readOnlyRawLog,
            GeneratePartitionKeyDel<ReadOnlyRawLog<TEx>> generatePartitionKeyDel,
            GenerateRowKeyDel<ReadOnlyRawLog<TEx>> generateRowKeyDel,
            FillVerboseLabelSetDel fillVerboseLabelSetDel)
        {
            AssertBeforeCreateLogEntity(readOnlyRawLog);
            AssertHelper.AssertNotNullOrThrow(generatePartitionKeyDel, nameof(generatePartitionKeyDel));
            AssertHelper.AssertNotNullOrThrow(generateRowKeyDel, nameof(generateRowKeyDel));
            AssertHelper.AssertNotNullOrThrow(fillVerboseLabelSetDel, nameof(fillVerboseLabelSetDel));

            try
            {
                var partitionKey = generatePartitionKeyDel(readOnlyRawLog);
                var rowKey = generateRowKeyDel(readOnlyRawLog);
                AzRepositoryAssertHelper.AssertPartitionKeyRowKeyOrThrow(partitionKey, rowKey);

                var verboseLabelSet = VerboseLabelSetFactory.Create();
                if (readOnlyRawLog.Verbose != null)
                    fillVerboseLabelSetDel(verboseLabelSet, readOnlyRawLog.Verbose);

                var logEntity = new LogEntity(partitionKey, rowKey, readOnlyRawLog.Source, readOnlyRawLog.SeverityLevel);
                InitializeLogEntity(logEntity, readOnlyRawLog, readOnlyRawLog, verboseLabelSet);

                return ExGlobalValueResponseFactory.CreateSuccessful(logEntity);
            }
            catch (Exception ex)
            {
                return ExGlobalValueResponseFactory.CreateFailure<LogEntity>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual ExGlobalValueResponse<ReadOnlyLog> Create(LogEntity logEntity)
        {
            AssertBeforeCreateLogEntity(logEntity);

            try
            {
                return ExGlobalValueResponseFactory.CreateSuccessful(new ReadOnlyLog(logEntity));
            }
            catch (Exception ex)
            {
                return ExGlobalValueResponseFactory.CreateFailure<ReadOnlyLog>(ex);
            }
        }

        /// <inheritdoc/>
        public virtual ExGlobalValueResponse<ReadOnlyRawLog<TEx>> Create<TRawLog>(TRawLog rawLog)
            where TRawLog : IRawLog<TEx>
        {
            AssertBeforeCreateLogEntity(rawLog);

            try
            {
                var logExceptionInfo = new LogExceptionInfo();
                InitializeLogExceptionInfo(logExceptionInfo, rawLog.Exception);
                
                return ExGlobalValueResponseFactory.CreateSuccessful(new ReadOnlyRawLog<TEx>(rawLog, logExceptionInfo));
            }
            catch (Exception ex)
            {
                return ExGlobalValueResponseFactory.CreateFailure<ReadOnlyRawLog<TEx>>(ex);
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Initializes the properties of a <paramref name="logEntity"/> with values from the specified parameters.
        /// </summary>
        /// <param name="logEntity">The log entity to initialize.</param>
        /// <param name="readOnlyRawLog">The read-only raw log used to initialize the <paramref name="logEntity"/>.</param>
        /// <param name="logExceptionInfo">The exception information used to initialize the <paramref name="logEntity"/>.</param>
        /// <param name="verboseLabelSet">The verbose label used to initialize the <paramref name="logEntity"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logEntity"/>, or the <paramref name="readOnlyRawLog"/>, 
        /// or the <paramref name="logExceptionInfo"/>, or the <paramref name="verboseLabelSet"/> are null.</exception>
        protected virtual void InitializeLogEntity(
            LogEntity logEntity,
            IReadOnlyRawLog readOnlyRawLog, 
            ILogExceptionInfo logExceptionInfo,
            IVerboseLabelSet verboseLabelSet)
        {
            AssertHelper.AssertNotNullOrThrow(logEntity, nameof(logEntity));
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));
            AssertHelper.AssertNotNullOrThrow(logExceptionInfo, nameof(logExceptionInfo));
            AssertHelper.AssertNotNullOrThrow(verboseLabelSet, nameof(verboseLabelSet));

            // ILog properties
            logEntity.Message = readOnlyRawLog.Message;
            logEntity.Category = readOnlyRawLog.Category;
            logEntity.CallStack = readOnlyRawLog.CallStack;
            logEntity.CallStackMethod = readOnlyRawLog.CallStackMethod;
            logEntity.CallStackLine = readOnlyRawLog.CallStackLine;
            logEntity.CallStackColumn = readOnlyRawLog.CallStackColumn;

            // ILogLabelSet
            InitializeLogLabelSet(logEntity, readOnlyRawLog);

            // ILogEntity properties
            if (readOnlyRawLog.Verbose != null)
                logEntity.Verbose = System.Text.Json.JsonSerializer.Serialize(readOnlyRawLog.Verbose);

            // ILogExceptionInfo properties
            InitializeLogExceptionInfo(logEntity, logExceptionInfo);

            // IVerboseLabelSet
            InitializeVerboseLabelSet(logEntity, verboseLabelSet);
        }

        #endregion

        #region Asserts

        private void AssertBeforeCreateLogEntity(ILog log)
        {
            AssertHelper.AssertNotNullOrThrow(log, nameof(log));

            AssertHelper.AssertNotNullNotEmptyOrThrow(log.Source, nameof(log.Source));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the properties of <paramref name="logExceptionInfo"/> instance with information from the provided <paramref name="exception"/>.
        /// </summary>
        /// <param name="logExceptionInfo">The instance to initialize.</param>
        /// <param name="exception">The exception from which to extract information.</param>
        /// <remarks>
        /// If the <paramref name="exception"/> parameter is not null, this method populates the properties of the <paramref name="logExceptionInfo"/> instance
        /// with relevant information extracted from the exception, including exception messages, stack traces, and type. If the exception contains a stack trace,
        /// it also captures the method name, line number, and column number of the top frame in the stack trace.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logExceptionInfo"/> is null.</exception>
        private void InitializeLogExceptionInfo(LogExceptionInfo logExceptionInfo, TEx? exception)
        {
            AssertHelper.AssertNotNullOrThrow(logExceptionInfo, nameof(logExceptionInfo));

            if (exception != default)
            {
                var exceptionType = exception.GetType().Name;
                var exceptionMessages = exception.GetAllMessagesFromExceptionHierarchy();
                var exceptionStackTraces = exception.GetAllStackTracesFromExceptionHierarchy();
                string? exceptionStackMethod = default;
                int? exceptionStackLine = default;
                int? exceptionStackColumn = default;

                var st = new StackTrace(exception, true);
                if (st.FrameCount > 0)
                {
                    StackFrame? frame = st.GetFrame(0);

                    if (frame != default)
                    {
                        var method = frame.GetMethod();

                        if (method != null)
                            exceptionStackMethod = $"{method.DeclaringType?.FullName}.{method.Name}";

                        exceptionStackLine = frame.GetFileLineNumber();
                        exceptionStackColumn = frame.GetFileColumnNumber();
                    }
                }

                logExceptionInfo.ExceptionType = exceptionType;
                logExceptionInfo.ExceptionMessages = exceptionMessages;
                logExceptionInfo.ExceptionStackTraces = exceptionStackTraces;
                logExceptionInfo.ExceptionStackMethod = exceptionStackMethod;
                logExceptionInfo.ExceptionStackLine = exceptionStackLine;
                logExceptionInfo.ExceptionStackColumn = exceptionStackColumn;
            }
        }

        /// <summary>
        /// Initializes the properties of <see cref="ILogExceptionInfo"/> in the <paramref name="logEntity"/> instance 
        /// from the data in the specified <paramref name="sourceLogExceptionInfo"/>.
        /// </summary>
        /// <param name="logEntity">The instance to initialize.</param>
        /// <param name="sourceLogExceptionInfo">The source with the exception information.</param>
        private void InitializeLogExceptionInfo(
            LogEntity logEntity,
            ILogExceptionInfo sourceLogExceptionInfo)
        {
            InitializeLogExceptionInfo(
                logEntity,
                sourceLogExceptionInfo.ExceptionType,
                sourceLogExceptionInfo.ExceptionMessages,
                sourceLogExceptionInfo.ExceptionStackTraces,
                sourceLogExceptionInfo.ExceptionStackMethod,
                sourceLogExceptionInfo.ExceptionStackLine,
                sourceLogExceptionInfo.ExceptionStackColumn);
        }

        /// <summary>
        /// Initializes the properties of <see cref="ILogExceptionInfo"/> in the <paramref name="logEntity"/> instance.
        /// </summary>
        /// <param name="logEntity">The instance to initialize.</param>
        /// <param name="exceptionType">Type of exception associated with the log.</param>
        /// <param name="exceptionMessages">Contains all messages from the exception hierarchy, including the message of 
        /// the top-level exception and all inner exceptions, associated with the log.</param>
        /// <param name="exceptionStackTraces">Contains all stack traces from the exception hierarchy, including the stack trace of 
        /// the top-level exception and all inner exceptions, associated with the log.</param>
        /// <param name="exceptionStackMethod">Call stack name of method of the exception associated with the log.</param>
        /// <param name="exceptionStackLine">Call stack line number of the exception associated with the log.</param>
        /// <param name="exceptionStackColumn">Call stack column number of the exception associated with the log.</param>
        private void InitializeLogExceptionInfo(
            LogEntity logEntity,
            string? exceptionType,
            string? exceptionMessages,
            string? exceptionStackTraces,
            string? exceptionStackMethod,
            int? exceptionStackLine,
            int? exceptionStackColumn)
        {
            logEntity.ExceptionType = exceptionType;
            logEntity.ExceptionMessages = exceptionMessages;
            logEntity.ExceptionStackTraces = exceptionStackTraces;
            logEntity.ExceptionStackMethod = exceptionStackMethod;
            logEntity.ExceptionStackLine = exceptionStackLine;
            logEntity.ExceptionStackColumn = exceptionStackColumn;
        }

        /// <summary>
        /// Inicializes the properties of <see cref="ILogLabelSet"/> in the <paramref name="logEntity"/> instance.
        /// </summary>
        /// <param name="logEntity">The instance to initialize.</param>
        /// <param name="logLabelSet">The source with the log label information.</param>
        private void InitializeLogLabelSet(
            LogEntity logEntity,
            ILogLabelSet logLabelSet)
        {
            logEntity.Label0 = logLabelSet.Label0;
            logEntity.Label1 = logLabelSet.Label1;
            logEntity.Label2 = logLabelSet.Label2;
            logEntity.Label3 = logLabelSet.Label3;
            logEntity.Label4 = logLabelSet.Label4;
            logEntity.Label5 = logLabelSet.Label5;
            logEntity.Label6 = logLabelSet.Label6;
            logEntity.Label7 = logLabelSet.Label7;
            logEntity.Label8 = logLabelSet.Label8;
            logEntity.Label9 = logLabelSet.Label9;
        }

        /// <summary>
        /// Inicializes the properties of <see cref="IVerboseLabelSet"/> in the <paramref name="logEntity"/> instance.
        /// </summary>
        /// <param name="logEntity">The instance to initialize.</param>
        /// <param name="verboseLabelSet">The source with the verbose label information.</param>
        private void InitializeVerboseLabelSet(
            LogEntity logEntity,
            IVerboseLabelSet verboseLabelSet)
        {
            logEntity.VerboseLabel0 = verboseLabelSet.VerboseLabel0;
            logEntity.VerboseLabel1 = verboseLabelSet.VerboseLabel1;
            logEntity.VerboseLabel2 = verboseLabelSet.VerboseLabel2;
            logEntity.VerboseLabel3 = verboseLabelSet.VerboseLabel3;
            logEntity.VerboseLabel4 = verboseLabelSet.VerboseLabel4;
            logEntity.VerboseLabel5 = verboseLabelSet.VerboseLabel5;
            logEntity.VerboseLabel6 = verboseLabelSet.VerboseLabel6;
            logEntity.VerboseLabel7 = verboseLabelSet.VerboseLabel7;
            logEntity.VerboseLabel8 = verboseLabelSet.VerboseLabel8;
            logEntity.VerboseLabel9 = verboseLabelSet.VerboseLabel9;
        }

        #endregion

        /// <summary>
        /// Represents an implementation of <see cref="ILogExceptionInfo"/>.
        /// </summary>
        class LogExceptionInfo : ILogExceptionInfo
        {
            #region ILogExceptionInfo properties

            /// <inheritdoc/>
            public virtual string? ExceptionType { get; set; }

            /// <inheritdoc/>
            public virtual string? ExceptionMessages { get; set; }

            /// <inheritdoc/>
            public virtual string? ExceptionStackTraces { get; set; }

            /// <inheritdoc/>
            public virtual string? ExceptionStackMethod { get; set; }

            /// <inheritdoc/>
            public virtual int? ExceptionStackLine { get; set; }

            /// <inheritdoc/>
            public virtual int? ExceptionStackColumn { get; set; }

            #endregion
        }
    }
}
