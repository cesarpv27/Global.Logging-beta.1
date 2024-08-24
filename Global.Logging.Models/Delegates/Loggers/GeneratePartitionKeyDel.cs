
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a delegate that generates a partition key from the data in the specified <paramref name="readOnlyRawLog"/>. 
    /// The generated partition key will be used to populate the <see cref="ITableEntity.PartitionKey"/> property in the instance to be stored.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <param name="readOnlyRawLog">The read-only raw log.</param>
    /// <returns>The generated partition key.</returns>
    public delegate string GeneratePartitionKeyDel<TReadOnlyRawLog>(TReadOnlyRawLog readOnlyRawLog)
        where TReadOnlyRawLog : IReadOnlyRawLog;
}
