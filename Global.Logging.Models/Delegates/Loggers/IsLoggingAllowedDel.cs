
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a delegate that determines if the logging is allowed for the specified read-only log.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only log.</typeparam>
    /// <param name="readOnlyLog">The read-only log to check.</param>
    /// <returns><c>true</c> if the logging is allowed for the specified log; otherwise, <c>false</c>.</returns>
    public delegate bool IsLoggingAllowedDel<TReadOnlyRawLog>(TReadOnlyRawLog readOnlyLog) where TReadOnlyRawLog : IReadOnlyRawLogBase;
}
