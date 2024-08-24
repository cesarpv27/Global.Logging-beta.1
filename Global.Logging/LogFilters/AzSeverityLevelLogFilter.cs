
namespace Global.Logging
{
    /// <inheritdoc cref="IAzSeverityLevelLogFilter{TReadOnlyRawLog, TReadOnlyLog}"/>
    public class AzSeverityLevelLogFilter<TReadOnlyRawLog, TReadOnlyLog> : IAzSeverityLevelLogFilter<TReadOnlyRawLog, TReadOnlyLog>
        where TReadOnlyRawLog : IReadOnlyRawLog
        where TReadOnlyLog : IReadOnlyLog
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzSeverityLevelLogFilter{TReadOnlyRawLog, TReadOnlyLog}"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public AzSeverityLevelLogFilter() { }

        #endregion

        #region Delegates

        /// <inheritdoc/>
        public virtual IsLoggingAllowedDel<TReadOnlyRawLog>? IsWritingAllowed { get; set; }

        /// <inheritdoc/>
        public virtual IsLoggingAllowedDel<TReadOnlyLog>? IsReadingAllowed { get; set; }

        #endregion

        #region Writing

        /// <inheritdoc/>
        public virtual bool AllowInfoSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));
            
            return SeverityLevelFilterFactory.AllowInfoSeverityLevel(readOnlyRawLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowLowSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));

            return SeverityLevelFilterFactory.AllowLowSeverityLevel(readOnlyRawLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowHighSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));

            return SeverityLevelFilterFactory.AllowHighSeverityLevel(readOnlyRawLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowErrorSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));

            return SeverityLevelFilterFactory.AllowErrorSeverityLevel(readOnlyRawLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowExceptionSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyRawLog, nameof(readOnlyRawLog));

            return SeverityLevelFilterFactory.AllowExceptionSeverityLevel(readOnlyRawLog.SeverityLevel);
        }

        #endregion

        #region IsReadingAllowedFor methods

        /// <inheritdoc/>
        public virtual bool AllowInfoSeverityLevelForReading(TReadOnlyLog readOnlyLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyLog, nameof(readOnlyLog));

            return SeverityLevelFilterFactory.AllowInfoSeverityLevel(readOnlyLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowLowSeverityLevelForReading(TReadOnlyLog readOnlyLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyLog, nameof(readOnlyLog));

            return SeverityLevelFilterFactory.AllowLowSeverityLevel(readOnlyLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowHighSeverityLevelForReading(TReadOnlyLog readOnlyLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyLog, nameof(readOnlyLog));

            return SeverityLevelFilterFactory.AllowHighSeverityLevel(readOnlyLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowErrorSeverityLevelForReading(TReadOnlyLog readOnlyLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyLog, nameof(readOnlyLog));

            return SeverityLevelFilterFactory.AllowErrorSeverityLevel(readOnlyLog.SeverityLevel);
        }

        /// <inheritdoc/>
        public virtual bool AllowExceptionSeverityLevelForReading(TReadOnlyLog readOnlyLog)
        {
            AssertHelper.AssertNotNullOrThrow(readOnlyLog, nameof(readOnlyLog));

            return SeverityLevelFilterFactory.AllowExceptionSeverityLevel(readOnlyLog.SeverityLevel);
        }

        #endregion
    }
}
