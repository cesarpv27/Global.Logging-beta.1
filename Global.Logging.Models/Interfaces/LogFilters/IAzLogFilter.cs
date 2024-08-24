
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents the filter for Azure logs, determining whether log writing or reading is allowed.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <typeparam name="TReadOnlyLog">The type of read-only log.</typeparam>
    public interface IAzLogFilter<TReadOnlyRawLog, TReadOnlyLog>
        where TReadOnlyRawLog : IReadOnlyRawLog
        where TReadOnlyLog : IReadOnlyLog
    {
        /// <summary>
        /// Delegate for determining whether logs writing is allowed.
        /// </summary>
        IsLoggingAllowedDel<TReadOnlyRawLog>? IsWritingAllowed { get; set; }

        /// <summary>
        /// Delegate for determining whether logs reading is allowed.
        /// </summary>
        IsLoggingAllowedDel<TReadOnlyLog>? IsReadingAllowed { get; set; }
    }
}
