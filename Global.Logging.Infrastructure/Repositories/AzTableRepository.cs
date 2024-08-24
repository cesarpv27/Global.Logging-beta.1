
namespace Global.Logging.Infrastructure.Repositories
{
    /// <inheritdoc cref="IAzTableRepository{T}"/>
    public class AzTableRepository<T> : AzStorageRepository, IAzTableRepository<T>
         where T : class, ITableEntity
    {
        /// <summary>
        /// Connection string of Azure storage 
        /// </summary>
        protected readonly string _connectionString;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzTableRepository{T}"/> class with the specified <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="connectionString">Connection string of Azure storage </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString"/> is null, empty, or white spaces.</exception>
        public AzTableRepository(string connectionString)
        {
            AssertHelper.AssertNotNullNotWhiteSpaceOrThrow(connectionString, nameof(connectionString));

            _connectionString = connectionString;
        }

        #endregion

        #region Public & protected properties

        /// <summary>
        /// The Azure Table name.
        /// </summary>
        protected string? TableName { get; private set; }

        /// <summary>
        /// The Azure Table Client.
        /// </summary>
        protected TableClient? TableClient { get; private set; }

        #endregion

        #region Asserts

        /// <inheritdoc/>
        public virtual void AssertTableNameOrThrow(string tableName, string? tableNameParamName = null)
        {
            AzAssertHelper.AssertTableNameOrThrow(tableName, nameof(tableName));
        }

        /// <summary>
        /// Asserts that the <see cref="TableClient"/> has been loaded.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="TableClient"/> has not been loaded.</exception>
        protected virtual void AssertTableClientOrThrow()
        {
            AssertHelper.AssertNotNullOrThrow(TableClient, nameof(TableClient));
        }

        /// <summary>
        /// Asserts necessary conditions before adding an entity to the Azure Table service.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="TableClient"/> has not been loaded.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="entity"/> is null.</exception>
        protected virtual void AssertBeforeAddEntityOrThrow(T entity)
        {
            AssertTableClientOrThrow();

            AzRepositoryAssertHelper.AssertEntityOrThrow(entity);
        }

        /// <summary>
        /// Asserts necessary conditions before retrieving an entity from the table.
        /// </summary>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <see cref="TableClient"/> has not been loaded.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="partitionKey"/> or <paramref name="rowKey"/> are null or empty.</exception>
        protected virtual void AssertBeforeGetEntityOrThrow(string partitionKey, string rowKey)
        {
            AssertTableClientOrThrow();
            AzRepositoryAssertHelper.AssertPartitionKeyRowKeyOrThrow(partitionKey, rowKey);
        }

        #endregion

        #region Build responses

        private AzTableResponse BuildFailedAzTableResponse(RequestFailedException ex, T entity)
        {
            var response = AzTableResponseFactory.CreateFrom(ex);
            FillResponseMetadata(response, entity, TableName);

            return response;
        }

        private AzTableResponse BuildFailedAzTableResponse(RequestFailedException ex, string tableName)
        {
            var response = AzTableResponseFactory.CreateFrom(ex);
            FillResponseMetadata(response, tableName);

            return response;
        }

        private AzTableResponse BuildFailedAzTableResponse(Exception ex, T entity)
        {
            var response = AzTableResponseFactory.CreateFailure(ex);
            FillResponseMetadata(response, entity, TableName);

            return response;
        }

        private AzTableResponse BuildFailedAzTableResponse(Exception ex, string tableName)
        {
            var response = AzTableResponseFactory.CreateFailure(ex);
            FillResponseMetadata(response, tableName);

            return response;
        }

        private AzTableValueResponse<T> BuildFailedAzTableValueResponse(RequestFailedException ex, string partitionKey, string rowKey)
        {
            var response = AzTableValueResponseFactory.CreateFrom<T>(ex);
            FillResponseMetadata(response, partitionKey, rowKey, TableName);

            return response;
        }

        private AzTableValueResponse<T> BuildFailedAzTableValueResponse(Exception ex, string partitionKey, string rowKey)
        {
            var response = AzTableValueResponseFactory.CreateFailure<T>(ex);
            FillResponseMetadata(response, partitionKey, rowKey, TableName);

            return response;
        }

        #endregion

        #region Fill responses

        private void FillResponseMetadata<TAzureResponse, TEx>(
            IAzGlobalResponse<TAzureResponse, TEx> response, 
            T entity,
            string? tableName)
            where TAzureResponse : IAzureResponse where TEx : Exception
        {
            FillResponseMetadata(response, entity.PartitionKey, entity.RowKey, tableName);
        }

        private void FillResponseMetadata<TAzureResponse, TEx>(
            IAzGlobalResponse<TAzureResponse, TEx> response, 
            string partitionKey, 
            string rowKey,
            string? tableName)
            where TAzureResponse : IAzureResponse where TEx : Exception
        {
            FillResponseMetadata(response, tableName);

            response.AddMessageTransactionally(RepositoryConstants.PartitionKeyMessageKey, partitionKey);
            response.AddMessageTransactionally(RepositoryConstants.RowKeyMessageKey, rowKey);
        }

        private void FillResponseMetadata<TAzureResponse, TEx>(
            IAzGlobalResponse<TAzureResponse, TEx> response, 
            string? tableName)
            where TAzureResponse : IAzureResponse where TEx : Exception
        {
            response.AddMessageTransactionally(RepositoryConstants.TableNameMessageKey, tableName ?? RepositoryConstants.TableNameNullOrEmptyMessage);
        }

        #endregion

        #region Load TableClient methods

        /// <summary>
        /// Loads the table client with the specified table name.
        /// </summary>
        /// <param name="tableName">The name of the table to load.</param>
        /// <returns>The <see cref="AzTableResponse"/> associated with the result of the operation.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tableName"/> has an incorrect format for Azure Table Service.</exception>
        protected virtual AzTableResponse LoadTableClientIfNew(string tableName)
        {
            AssertTableNameOrThrow(tableName);

            try
            {
                if (string.IsNullOrEmpty(TableName) || !TableName.Equals(tableName))
                    TableClient = new TableClient(_connectionString, tableName, CreateClientOptionsOfGetDefault<AzTableClientOptions>());

                return AzTableResponseFactory.CreateSuccessful();
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableResponse(ex, tableName);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableResponse(ex, tableName);
            }
        }

        /// <inheritdoc/>
        public virtual AzTableResponse LoadTableClient(
            string tableName,
            bool createIfNotExists = true)
        {
            var loadTableClientResponse = LoadTableClientIfNew(tableName);
            if (loadTableClientResponse.Status == ResponseStatus.Failure)
                return loadTableClientResponse;

            try
            {
                AzTableResponse response;
                if (createIfNotExists)
                    response = AzTableResponseFactory.CreateFrom(TableClient!.CreateIfNotExists());
                else
                    response = AzTableResponseFactory.CreateSuccessful();

                if (response.Status == ResponseStatus.Success)
                    TableName = tableName;

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableResponse(ex, tableName);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableResponse(ex, tableName);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> LoadTableClientAsync(
            string tableName,
            bool createIfNotExists = true,
            CancellationToken cancellationToken = default)
        {
            var loadTableClientResponse = LoadTableClientIfNew(tableName);
            if (loadTableClientResponse.Status == ResponseStatus.Failure)
                return loadTableClientResponse;

            try
            {
                AzTableResponse response;
                if (createIfNotExists)
                    response = AzTableResponseFactory.CreateFrom(await TableClient!.CreateIfNotExistsAsync(cancellationToken));
                else
                    response = AzTableResponseFactory.CreateSuccessful();

                if (response.Status == ResponseStatus.Success)
                    TableName = tableName;

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableResponse(ex, tableName);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableResponse(ex, tableName);
            }
        }

        #endregion

        #region Add methods

        /// <inheritdoc/>
        public virtual AzTableResponse Add(T entity)
        {
            AssertBeforeAddEntityOrThrow(entity);

            try
            {
                var response = AzTableResponseFactory.CreateFrom(TableClient!.AddEntity(entity));
                FillResponseMetadata(response, entity, TableName);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableResponse(ex, entity);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableResponse(ex, entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableResponse> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            AssertBeforeAddEntityOrThrow(entity);

            try
            {
                var response = AzTableResponseFactory.CreateFrom(await TableClient!.AddEntityAsync(entity, cancellationToken));
                FillResponseMetadata(response, entity, TableName);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableResponse(ex, entity);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableResponse(ex, entity);
            }
        }

        #endregion

        #region Get methods

        /// <inheritdoc/>
        public virtual AzTableValueResponse<T> Get(string partitionKey, string rowKey)
        {
            AssertBeforeGetEntityOrThrow(partitionKey, rowKey);

            try
            {
                var response = AzTableValueResponseFactory.CreateFrom(TableClient!.GetEntity<T>(partitionKey, rowKey));
                FillResponseMetadata(response, partitionKey, rowKey, TableName);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableValueResponse(ex, partitionKey, rowKey);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableValueResponse(ex, partitionKey, rowKey);
            }
        }

        /// <inheritdoc/>
        public virtual async Task<AzTableValueResponse<T>> GetAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default)
        {
            AssertBeforeGetEntityOrThrow(partitionKey, rowKey);

            try
            {
                var response = AzTableValueResponseFactory.CreateFrom(await TableClient!.GetEntityAsync<T>(partitionKey, rowKey, cancellationToken: cancellationToken));
                FillResponseMetadata(response, partitionKey, rowKey, TableName);

                return response;
            }
            catch (RequestFailedException ex)
            {
                return BuildFailedAzTableValueResponse(ex, partitionKey, rowKey);
            }
            catch (Exception ex)
            {
                return BuildFailedAzTableValueResponse(ex, partitionKey, rowKey);
            }
        }

        #endregion
    }
}
