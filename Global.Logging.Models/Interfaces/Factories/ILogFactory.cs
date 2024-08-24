
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a factory for creating log entities.
    /// </summary>
    /// <typeparam name="TReadOnlyRawLog">The type of read-only raw log.</typeparam>
    /// <typeparam name="TReadOnlyLog">The type of read-only log.</typeparam>
    /// <typeparam name="TLogEntity">The type of log entity.</typeparam>
    /// <typeparam name="TEx">The type of the exception associated with the raw log.</typeparam>
    public interface ILogFactory<TReadOnlyRawLog, TReadOnlyLog, TLogEntity, TEx>
        where TReadOnlyRawLog : IReadOnlyRawLog
        where TReadOnlyLog : IReadOnlyLog
        where TLogEntity : class, ILogEntity 
        where TEx : Exception
    {
        /// <summary>
        /// Creates a log entity based on the provided raw log.
        /// </summary>
        /// <param name="readOnlyRawLog">The read-only raw log providing information for creating the log entity.</param>
        /// <param name="generatePartitionKeyDel">Delegate that generates a partition keys for Azure Storage entities.</param>
        /// <param name="generateRowKeyDel">Delegate that generates a row keys for Azure Storage entities.</param>
        /// <param name="fillVerboseLabelSetDel">Delegate for filling a <see cref="VerboseLabelSet"/> with specific data for categorization or identification purposes, 
        /// using data from the specified "Verbose" collection. 
        /// The verbose label set will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.</param>
        /// <returns>The <see cref="ExGlobalValueResponse{T}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <see cref="ILogEntity"/> if has been successfully created; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/> and may contain an exception 
        /// related with the creation of the <see cref="ILogEntity"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="readOnlyRawLog"/>, <paramref name="generatePartitionKeyDel"/>, 
        /// <paramref name="generateRowKeyDel"/>, or the <paramref name="fillVerboseLabelSetDel"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when value of the property 'Source' in the <paramref name="readOnlyRawLog"/> is null or empty.</exception>
        ExGlobalValueResponse<TLogEntity> Create(
            TReadOnlyRawLog readOnlyRawLog,
            GeneratePartitionKeyDel<TReadOnlyRawLog> generatePartitionKeyDel,
            GenerateRowKeyDel<TReadOnlyRawLog> generateRowKeyDel,
            FillVerboseLabelSetDel fillVerboseLabelSetDel);

        /// <summary>
        /// Creates a new instance of read-only log based on the data in the specified <paramref name="logEntity"/>.
        /// </summary>
        /// <param name="logEntity">The log entity providing information for creating the read-only log.</param>
        /// <returns>The <see cref="ExGlobalValueResponse{T}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <see cref="ILogEntity"/> if has been successfully created; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/> and may contain an exception 
        /// related with the creation of the <see cref="ILogEntity"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="logEntity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when value of the property 'Source' in the <paramref name="logEntity"/> is null or empty.</exception>
        ExGlobalValueResponse<TReadOnlyLog> Create(TLogEntity logEntity);

        /// <summary>
        /// Creates a new instance of read-only raw log based on the data in the specified <paramref name="rawLog"/>.
        /// </summary>
        /// <typeparam name="TRawLog">The type of raw log.</typeparam>
        /// <param name="rawLog">The raw log from which to create the read-only log.</param>
        /// <returns>The <see cref="ExGlobalValueResponse{T}"/> associated with the result of the operation. 
        /// A new instance of <see cref="ReadOnlyRawLog{TEx}"/> initialized with the data of the provided <paramref name="rawLog"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rawLog"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when value of the property 'Source' in the <paramref name="rawLog"/> is null or empty.</exception>
        ExGlobalValueResponse<TReadOnlyRawLog> Create<TRawLog>(TRawLog rawLog) where TRawLog : IRawLog<TEx>;
    }
}
