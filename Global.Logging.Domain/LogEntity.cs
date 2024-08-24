
namespace Global.Logging.Domain
{
    /// <inheritdoc cref="ILogEntity"/>
    public class LogEntity : PartialLog, ILogEntity
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntity"/> class. 
        /// Reserved only for Azure Table Service and Azure Blob Storage compatibility.
        /// </summary>
#pragma warning disable CS8618
        public LogEntity() { }
#pragma warning restore

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntity"/> class.
        /// </summary>
        /// <param name="partitionKey">The partition key of the log entity.</param>
        /// <param name="rowKey">The row key of the log entity.</param>
        /// <param name="source">Source of the log.</param>
        /// <param name="severityLevel">Severity level of the log.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="partitionKey"/>, or the <paramref name="rowKey"/>, 
        /// or the <paramref name="source"/> are null.</exception>
        public LogEntity(
            string partitionKey, 
            string rowKey, 
            string source,
            SeverityLevel severityLevel) : base(source, severityLevel)
        {
            if (partitionKey == null)
                throw new ArgumentNullException(partitionKey);
            if (rowKey == null)
                throw new ArgumentNullException(rowKey);

            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        #endregion

        #region ITableEntity properties

        /// <inheritdoc/>
        public string PartitionKey { get; set; }

        /// <inheritdoc/>
        public string RowKey { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset? Timestamp { get; set; }

        /// <inheritdoc/>
        public ETag ETag { get; set; }

        #endregion

        #region ILogEntity

        /// <inheritdoc/>
        public virtual string? Verbose { get; set; }

        #endregion

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

        #region IVerboseLabelSet properties

        /// <inheritdoc/>
        public virtual string? VerboseLabel0 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel1 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel2 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel3 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel4 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel5 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel6 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel7 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel8 { get; set; }

        /// <inheritdoc/>
        public virtual string? VerboseLabel9 { get; set; }

        #endregion
    }
}
