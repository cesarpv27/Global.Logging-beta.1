
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a table repository in Azure, inheriting from <see cref="IAzStorageRepository"/>.
    /// </summary>
    /// <typeparam name="T">The type of the Azure Table entity.</typeparam>
    /// <remarks>
    /// A table name in the correct format for Azure Table Service must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters or numbers.<br/>
    /// - The name must begin with a letter.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// - Some names are reserved, including 'tables'.<br/>
    /// </remarks>
    public interface IAzTableRepository<T> : IAzStorageRepository
         where T : class, ITableEntity
    {
        /// <summary>
        /// Asserts whether the provided <paramref name="tableName"/> is in the correct format for Azure Table Service; otherwise, throws an exception.
        /// For more information, visit https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model.
        /// </summary>
        /// <param name="tableName">The table name to assert.</param>
        /// <param name="tableNameParamName">The parameter name for the table name (optional).</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tableName"/> has an incorrect format for Azure Table Service.</exception>
        void AssertTableNameOrThrow(string tableName, string? tableNameParamName = null);

        /// <summary>
        /// Loads the Azure <see cref="TableClient"/> for the specified by <paramref name="tableName"/> if it is not the current Azure Table. In other case, the current Azure Table is retained.
        /// </summary>
        /// <param name="tableName">Name of Azure Table.</param>
        /// <param name="createIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation.
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="tableName"/> is the current Azure Table, 
        /// or if the <see cref="TableClient"/> has been successfully loaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tableName"/> has an incorrect format for Azure Table Service.</exception>
        AzTableResponse LoadTableClient(string tableName, bool createIfNotExists = true);

        /// <summary>
        /// Asynchronously loads the Azure <see cref="TableClient"/> for the specified by <paramref name="tableName"/> if it is not the current Azure Table. In other case, the current Azure Table is retained.
        /// </summary>
        /// <param name="tableName">Name of Azure Table.</param>
        /// <param name="createIfNotExists">If true, create the Azure Table if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation.
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="tableName"/> is the current Azure Table, 
        /// or if the <see cref="TableClient"/> has been successfully loaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tableName"/> has an incorrect format for Azure Table Service.</exception>
        Task<AzTableResponse> LoadTableClientAsync(string tableName, bool createIfNotExists = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds entity <paramref name="entity"/> into the loaded Azure Table.
        /// The 'TableClient' must be properly initialized using either the <see cref="LoadTableClient"/> or
        /// <see cref="LoadTableClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="TableClient"/> has not been previously loaded
        /// using either the <see cref="LoadTableClient(string, bool)"/> or <see cref="LoadTableClientAsync(string, bool, CancellationToken)"/> methods.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ITableEntity.PartitionKey"/> or <see cref="ITableEntity.RowKey"/> are null or empty in <paramref name="entity"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when 'TableClient' has not been loaded.</exception>
        AzTableResponse Add(T entity);

        /// <summary>
        /// Asynchronously adds entity <paramref name="entity"/> into the loaded Azure Table.
        /// The 'TableClient' must be properly initialized using either the <see cref="LoadTableClient"/> or
        /// <see cref="LoadTableClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="entity"/> has been successfully added; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="TableClient"/> has not been previously loaded
        /// using either the <see cref="LoadTableClient(string, bool)"/> or <see cref="LoadTableClientAsync(string, bool, CancellationToken)"/> methods.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="ITableEntity.PartitionKey"/> or <see cref="ITableEntity.RowKey"/> are null or empty in <paramref name="entity"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when 'TableClient' has not been loaded.</exception>
        Task<AzTableResponse> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an entity from the loaded Azure Table.
        /// The 'TableClient' must be properly initialized using either the <see cref="LoadTableClient"/> or
        /// <see cref="LoadTableClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <returns>The <see cref="AzTableValueResponse{T}"/> associated with the result of the operation.
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="partitionKey"/> or <paramref name="rowKey"/> are null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when 'TableClient' has not been loaded.</exception>
        AzTableValueResponse<T> Get(string partitionKey, string rowKey);

        /// <summary>
        /// Asynchronously retrieves an entity from the loaded Azure Table.
        /// The 'TableClient' must be properly initialized using either the <see cref="LoadTableClient"/> or
        /// <see cref="LoadTableClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzTableValueResponse{T}"/> associated with the result of the operation. 
        /// If the entity exists, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the requested entity; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="partitionKey"/> or <paramref name="rowKey"/> are null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when 'TableClient' has not been loaded.</exception>
        Task<AzTableValueResponse<T>> GetAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default);
    }
}
