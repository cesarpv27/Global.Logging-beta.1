
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a delegate that generates a log table name from the data in the specified <paramref name="readOnlyRawLog"/>.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <param name="readOnlyRawLog">The read-only raw log.</param>
    /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
    /// <param name="rowKey">The generated row key for the log entity which will be stored.</param>
    /// <returns>The generated log table name.</returns>
    public delegate string GenerateLogTableNameDel<TReadOnlyRawLog>(TReadOnlyRawLog readOnlyRawLog, string partitionKey, string rowKey)
        where TReadOnlyRawLog : IReadOnlyRawLog;
}
