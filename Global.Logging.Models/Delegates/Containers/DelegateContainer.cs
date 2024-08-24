
namespace Global.Logging.Models
{
    /// <summary>
    /// A class that contains delegates for generating keys and names for Azure log entities.
    /// This class is used to provide delegates to the Azure logger for creating partition keys, row keys, 
    /// table names, blob container names, and blob names for log entities in Azure Storage.
    /// </summary>
    public class DelegateContainer<TReadOnlyRawLog>
        where TReadOnlyRawLog : IReadOnlyRawLog
    {
        /// <summary>
        /// Delegate that generates a partition key for Azure log entities.
        /// Set to null to reset to the default behavior.
        /// </summary>
        public GeneratePartitionKeyDel<TReadOnlyRawLog>? GeneratePartitionKeyDel { get; set; }

        /// <summary>
        /// Sets the <paramref name="generatePartitionKey"/> as the delegate that generates a partition key for Azure log entities.
        /// If <paramref name="generatePartitionKey"/> is not null, <see cref="GeneratePartitionKeyDel"/> will be initialized with the provided function.
        /// Otherwise, <see cref="GeneratePartitionKeyDel"/> will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generatePartitionKey">The function to be used as the delegate for generating partition keys. Can be null.</param>
        public void SetGeneratePartitionKey(Func<TReadOnlyRawLog, string>? generatePartitionKey)
        {
            if (generatePartitionKey != default)
                GeneratePartitionKeyDel = new GeneratePartitionKeyDel<TReadOnlyRawLog>(generatePartitionKey);
            else
                GeneratePartitionKeyDel = default;
        }

        /// <summary>
        /// Delegate that generates a row keys for Azure Storage entities.
        /// Set to null to reset to the default behavior.
        /// </summary>
        public GenerateRowKeyDel<TReadOnlyRawLog>? GenerateRowKeyDel { get; set; }

        /// <summary>
        /// Sets the <paramref name="generateRowKey"/> as the delegate that generates a row key for Azure log entities.
        /// If <paramref name="generateRowKey"/> is not null, <see cref="GenerateRowKeyDel"/> will be initialized with the provided function.
        /// Otherwise, <see cref="GenerateRowKeyDel"/> will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generateRowKey">The function to be used as the delegate for generating row keys. Can be null.</param>
        public void SetGenerateRowKey(Func<TReadOnlyRawLog, string>? generateRowKey)
        {
            if (generateRowKey != default)
                GenerateRowKeyDel = new GenerateRowKeyDel<TReadOnlyRawLog>(generateRowKey);
            else
                GenerateRowKeyDel = default;
        }

        /// <summary>
        /// Delegate for filling an instance of <see cref="IVerboseLabelSet"/> with specific data for categorization or identification purposes,  
        /// using data from the specified "Verbose" collection.
        /// The verbose label set will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.
        /// Set to null to reset to the default behavior.
        /// </summary>
        public FillVerboseLabelSetDel? FillVerboseLabelSetDel { get; set; }

        /// <summary>
        /// Sets the <paramref name="fillVerboseLabelSet"/> as delegate for filling an instance of <see cref="IVerboseLabelSet"/> with specific data for categorization or identification purposes,  
        /// using data from the specified "Verbose" collection.
        /// The verbose label set will be used to populate the same properties in the instance of <see cref="ILogEntity"/> which will be stored.
        /// If <paramref name="fillVerboseLabelSet"/> is not null, <see cref="FillVerboseLabelSetDel"/> will be initialized with the provided function.
        /// Otherwise, <see cref="FillVerboseLabelSetDel"/> will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="fillVerboseLabelSet">The function to be used as the delegate for filling an instance of <see cref="IVerboseLabelSet"/>. Can be null.</param>
        public void SetFillVerboseLabelSet(Action<VerboseLabelSet, ReadOnlyDictionary<string, string>>? fillVerboseLabelSet)
        {
            if (fillVerboseLabelSet != default)
                FillVerboseLabelSetDel = new FillVerboseLabelSetDel(fillVerboseLabelSet);
            else
                FillVerboseLabelSetDel = default;
        }

        /// <summary>
        /// Delegate that generates an Azure Table names for storing log entities. 
        /// Set to null to reset to the default behavior. 
        /// If <see cref="GenerateLogTableNameDel"/> is null, the table names will be generated using the default behavior:
        /// "Logger" appended with the current year, month, and the following suffix:
        /// "Low" if the "SeverityLevel" is <see cref="SeverityLevel.Info"/> or <see cref="SeverityLevel.Warning"/>, otherwise "High".
        /// <example>
        /// "Logger200101Low"
        /// "Logger200101High"
        /// </example>
        /// </summary>
        public GenerateLogTableNameDel<TReadOnlyRawLog>? GenerateLogTableNameDel { get; set; }

        /// <summary>
        /// Sets the <paramref name="generateLogTableName"/> as delegate that generates an Azure Table names for storing log entities. 
        /// If <paramref name="generateLogTableName"/> is not null, <see cref="GenerateLogTableNameDel"/> will be initialized with the provided function.
        /// Otherwise, <see cref="GenerateLogTableNameDel"/> will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generateLogTableName">The function to be used as the delegate for generating Azure Table names. Can be null.</param>
        public void SetGenerateLogTableName(Func<TReadOnlyRawLog, string, string, string>? generateLogTableName)
        {
            if (generateLogTableName != default)
                GenerateLogTableNameDel = new GenerateLogTableNameDel<TReadOnlyRawLog>(generateLogTableName);
            else
                GenerateLogTableNameDel = default;
        }

        /// <summary>
        /// Delegate that generates an Azure Blob Container names for storing log entities.
        /// Set to null to reset to the default behavior. 
        /// If <see cref="GenerateLogBlobContainerNameDel"/> is null, the blob container name will be generated using the default behavior:
        /// "azlogs-" appeded with the current year and month.
        /// <example>
        /// "azlogs-200101"
        /// </example>
        /// </summary>
        public GenerateLogBlobContainerNameDel<TReadOnlyRawLog>? GenerateLogBlobContainerNameDel { get; set; }

        /// <summary>
        /// Sets the <paramref name="generateLogBlobContainerName"/> as delegate that generates an Azure Blob Container names for storing log entities. 
        /// If <paramref name="generateLogBlobContainerName"/> is not null, <see cref="GenerateLogBlobContainerNameDel"/> will be initialized with the provided function.
        /// Otherwise, <see cref="GenerateLogBlobContainerNameDel"/> will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generateLogBlobContainerName">The function to be used as the delegate for generating Azure Blob Container names. Can be null.</param>
        public void SetGenerateLogBlobContainerName(Func<TReadOnlyRawLog, string, string, string>? generateLogBlobContainerName)
        {
            if (generateLogBlobContainerName != default)
                GenerateLogBlobContainerNameDel = new GenerateLogBlobContainerNameDel<TReadOnlyRawLog>(generateLogBlobContainerName);
            else
                GenerateLogBlobContainerNameDel = default;
        }

        /// <summary>
        /// Delegate that generates an Azure Blob names for storing log entities.
        /// Set to null to reset to the default behavior. 
        /// If <see cref="GenerateLogBlobNameDel"/> is null, the blob name will be generated using the default behavior:
        /// ILogEntity.SeverityLevel/ILogEntity.PartitionKey/ILogEntity.RowKey.json.
        /// <example>
        /// "Info/PartitionKey/RowKey.json"
        /// </example>
        /// </summary>
        public GenerateLogBlobNameDel<TReadOnlyRawLog>? GenerateLogBlobNameDel { get; set; }

        /// <summary>
        /// Sets the <paramref name="generateLogBlobName"/> as delegate that generates an Azure Blob names for storing log entities. 
        /// If <paramref name="generateLogBlobName"/> is not null, <see cref="GenerateLogBlobNameDel"/> will be initialized with the provided function.
        /// Otherwise, <see cref="GenerateLogBlobNameDel"/> will be set to null.
        /// Set to null to reset to the default behavior.
        /// </summary>
        /// <param name="generateLogBlobName">The function to be used as the delegate for generating Azure Blob names. Can be null.</param>
        public void SetGenerateLogBlobName(Func<TReadOnlyRawLog, string, string, string>? generateLogBlobName)
        {
            if (generateLogBlobName != default)
                GenerateLogBlobNameDel = new GenerateLogBlobNameDel<TReadOnlyRawLog>(generateLogBlobName);
            else
                GenerateLogBlobNameDel = default;
        }
    }
}
