
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a class that contains default filter for Azure logs, determining whether log writing or reading is allowed based on the <see cref="SeverityLevel"/>.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <typeparam name="TReadOnlyLog">The type of read-only log.</typeparam>
    public interface IAzSeverityLevelLogFilter<TReadOnlyRawLog, TReadOnlyLog> : IAzLogFilter<TReadOnlyRawLog, TReadOnlyLog>
        where TReadOnlyRawLog : IReadOnlyRawLog
        where TReadOnlyLog : IReadOnlyLog
    {
        #region Writing

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is <see cref="SeverityLevel.Info"/>, 
        /// and allows it for writing.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is <see cref="SeverityLevel.Info"/>; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        bool AllowInfoSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog);

        /// <summary>
        /// Sets the <see cref="AllowInfoSeverityLevelForWriting"/> to the 'IsWritingAllowedDel' property.
        /// </summary>
        public void SetAllowInfoSeverityLevelForWriting()
        {
            IsWritingAllowed = AllowInfoSeverityLevelForWriting;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the low severity levels, 
        /// and allows it for writing. 
        /// Low severity levels are: 
        /// <see cref="SeverityLevel.Info"/>,
        /// <see cref="SeverityLevel.Warning"/>
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the low severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        bool AllowLowSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog);

        /// <summary>
        /// Sets the <see cref="AllowLowSeverityLevelForWriting"/> to the 'IsWritingAllowedDel' property.
        /// </summary>
        public void SetAllowLowSeverityLevelForWriting()
        {
            IsWritingAllowed = AllowLowSeverityLevelForWriting;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the high severity levels, 
        /// and allows it for writing. 
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Error"/>, 
        /// <see cref="SeverityLevel.FatalError"/>, 
        /// <see cref="SeverityLevel.Exception"/>, 
        /// <see cref="SeverityLevel.FatalException"/>
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the high severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        bool AllowHighSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog);

        /// <summary>
        /// Sets the <see cref="AllowHighSeverityLevelForWriting"/> to the 'IsWritingAllowedDel' property.
        /// </summary>
        public void SetAllowHighSeverityLevelForWriting()
        {
            IsWritingAllowed = AllowHighSeverityLevelForWriting;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the error severity levels, 
        /// and allows it for writing. 
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Error"/>, 
        /// <see cref="SeverityLevel.FatalError"/>
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the error severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        bool AllowErrorSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog);

        /// <summary>
        /// Sets the <see cref="AllowErrorSeverityLevelForWriting"/> to the 'IsWritingAllowedDel' property.
        /// </summary>
        public void SetAllowErrorSeverityLevelForWriting()
        {
            IsWritingAllowed = AllowErrorSeverityLevelForWriting;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the exception severity levels, 
        /// and allows it for writing. 
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Exception"/>, 
        /// <see cref="SeverityLevel.FatalException"/>
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyRawLog"/> is any of the exception severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyRawLog"/> is null.</exception>
        bool AllowExceptionSeverityLevelForWriting(TReadOnlyRawLog readOnlyRawLog);

        /// <summary>
        /// Sets the <see cref="AllowExceptionSeverityLevelForWriting"/> to the 'IsWritingAllowedDel' property.
        /// </summary>
        public void SetAllowExceptionSeverityLevelForWriting()
        {
            IsWritingAllowed = AllowExceptionSeverityLevelForWriting;
        }

        #endregion

        #region Reading

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is <see cref="SeverityLevel.Info"/>, 
        /// and allows it for reading.
        /// </summary>
        /// <param name="readOnlyLog">The read-only raw log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is <see cref="SeverityLevel.Info"/>; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyLog"/> is null.</exception>
        bool AllowInfoSeverityLevelForReading(TReadOnlyLog readOnlyLog);

        /// <summary>
        /// Sets the <see cref="AllowInfoSeverityLevelForReading"/> to the 'IsReadingAllowedDel' property.
        /// </summary>
        public void SetAllowInfoSeverityLevelForReading()
        {
            IsReadingAllowed = AllowInfoSeverityLevelForReading;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the low severity levels, 
        /// and allows it for reading. 
        /// Low severity levels are: 
        /// <see cref="SeverityLevel.Info"/>,
        /// <see cref="SeverityLevel.Warning"/>
        /// </summary>
        /// <param name="readOnlyLog">The read-only log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the low severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyLog"/> is null.</exception>
        bool AllowLowSeverityLevelForReading(TReadOnlyLog readOnlyLog);

        /// <summary>
        /// Sets the <see cref="AllowLowSeverityLevelForReading"/> to the 'IsReadingAllowedDel' property.
        /// </summary>
        public void SetAllowLowSeverityLevelForReading()
        {
            IsReadingAllowed = AllowLowSeverityLevelForReading;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the high severity levels, 
        /// and allows it for reading. 
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Error"/>, 
        /// <see cref="SeverityLevel.FatalError"/>, 
        /// <see cref="SeverityLevel.Exception"/>, 
        /// <see cref="SeverityLevel.FatalException"/>
        /// </summary>
        /// <param name="readOnlyLog">The read-only log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the high severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyLog"/> is null.</exception>
        bool AllowHighSeverityLevelForReading(TReadOnlyLog readOnlyLog);

        /// <summary>
        /// Sets the <see cref="AllowHighSeverityLevelForReading"/> to the 'IsReadingAllowedDel' property.
        /// </summary>
        public void SetAllowHighSeverityLevelForReading()
        {
            IsReadingAllowed = AllowHighSeverityLevelForReading;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the error severity levels, 
        /// and allows it for reading. 
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Error"/>, 
        /// <see cref="SeverityLevel.FatalError"/>
        /// </summary>
        /// <param name="readOnlyLog">The read-only log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the error severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyLog"/> is null.</exception>
        bool AllowErrorSeverityLevelForReading(TReadOnlyLog readOnlyLog);

        /// <summary>
        /// Sets the <see cref="AllowErrorSeverityLevelForReading"/> to the 'IsReadingAllowedDel' property.
        /// </summary>
        public void SetAllowErrorSeverityLevelForReading()
        {
            IsReadingAllowed = AllowErrorSeverityLevelForReading;
        }

        /// <summary>
        /// Checks if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the exception severity levels, 
        /// and allows it for reading. 
        /// High severity levels are: 
        /// <see cref="SeverityLevel.Exception"/>, 
        /// <see cref="SeverityLevel.FatalException"/>
        /// </summary>
        /// <param name="readOnlyLog">The read-only log to check.</param>
        /// <returns><c>true</c> if the <see cref="SeverityLevel"/> in the provided <paramref name="readOnlyLog"/> is any of the exception severity levels; 
        /// otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="readOnlyLog"/> is null.</exception>
        bool AllowExceptionSeverityLevelForReading(TReadOnlyLog readOnlyLog);

        /// <summary>
        /// Sets the <see cref="AllowExceptionSeverityLevelForReading"/> to the 'IsReadingAllowedDel' property.
        /// </summary>
        public void SetAllowExceptionSeverityLevelForReading()
        {
            IsReadingAllowed = AllowExceptionSeverityLevelForReading;
        }

        #endregion
    }
}
