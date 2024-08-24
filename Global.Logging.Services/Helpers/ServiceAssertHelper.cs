
namespace Global.Logging.Services.Helpers
{
    internal static class ServiceAssertHelper
    {
        /// <summary>
        /// Asserts that the retry values are valid.
        /// </summary>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="retryOnFailures"/> and <paramref name="maxRetryAttempts"/> were specified
        /// and <paramref name="maxRetryAttempts"/> is out of range (less than zero).</exception>
        public static void AssertMaxRetryAttemptsOrThrow(bool retryOnFailures, int? maxRetryAttempts = default)
        {
            if (retryOnFailures && maxRetryAttempts != default && maxRetryAttempts!.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxRetryAttempts), AzServiceConstants.MaxRetryAttemptsIfOutOfRange(nameof(maxRetryAttempts)));
        }

        /// <summary>
        /// Assert the parameters.
        /// </summary>
        /// <param name="tableName">The name of the table to assert.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        public static void AssertBeforeGetOrThrow(
            string tableName,
            string partitionKey,
            string rowKey,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null)
        {
            AzAssertHelper.AssertTableNameOrThrow(tableName, nameof(tableName));
            AzRepositoryAssertHelper.AssertPartitionKeyRowKeyOrThrow(partitionKey, rowKey);
            AssertMaxRetryAttemptsOrThrow(retryOnFailures, maxRetryAttempts);
        }

        /// <summary>
        /// Asserts that the <paramref name="tableName"/>, <paramref name="entity"/> and the retry values are valid.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="tableName">The name of the table to assert.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        public static void AssertBeforeAddOrThrow<T>(
            T entity,
            string tableName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null) where T : ILogEntity
        {
            AzRepositoryAssertHelper.AssertEntityOrThrow(entity);
            AzAssertHelper.AssertTableNameOrThrow(tableName, nameof(tableName));
            AssertMaxRetryAttemptsOrThrow(retryOnFailures, maxRetryAttempts);
        }

        /// <summary>
        /// Asserts that the <paramref name="entity"/> and the parameters are valid.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="blobContainerName">The blob container name to assert.</param>
        /// <param name="blobName">The blob name to assert. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        public static void AssertBeforeAddOrThrow<T>(
            T entity,
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null) where T : ILogEntity
        {
            AzRepositoryAssertHelper.AssertEntityOrThrow(entity);
            AzRepositoryAssertHelper.AssertBlobContainerNameBlobNameOrThrow(blobContainerName, blobName);
            AssertMaxRetryAttemptsOrThrow(retryOnFailures, maxRetryAttempts);
        }

        /// <summary>
        /// Asserts that the parameters are valid.
        /// </summary>
        /// <param name="blobContainerName">The blob container name to assert.</param>
        /// <param name="blobName">The blob name to assert. Must include the directory path of the blob.</param>
        /// <param name="retryOnFailures">Flag indicating whether to retry on failures.</param>
        /// <param name="maxRetryAttempts">The maximum number of retry attempts (optional).</param>
        public static void AssertBeforeGetOrThrow(
            string blobContainerName,
            string blobName,
            bool retryOnFailures = true,
            int? maxRetryAttempts = null)
        {
            AzRepositoryAssertHelper.AssertBlobContainerNameBlobNameOrThrow(blobContainerName, blobName);
            AssertMaxRetryAttemptsOrThrow(retryOnFailures, maxRetryAttempts);
        }
    }
}
