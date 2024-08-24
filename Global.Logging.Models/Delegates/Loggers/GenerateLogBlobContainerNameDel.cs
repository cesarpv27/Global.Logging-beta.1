
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a delegate that generates a log container name from the data in the specified <paramref name="readOnlyRawLog"/>.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <param name="readOnlyRawLog">The read-only raw log.</param>
    /// <param name="partitionKey">The generated partition key for the log entity which will be stored.</param>
    /// <param name="rowKy">The generated row key for the log entity which will be stored.</param>
    /// <returns>The generated log container name.</returns>
    public delegate string GenerateLogBlobContainerNameDel<TReadOnlyRawLog>(TReadOnlyRawLog readOnlyRawLog, string partitionKey, string rowKy)
        where TReadOnlyRawLog : IReadOnlyRawLog;
}
