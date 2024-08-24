using System.Net;

namespace Global.Logging.Services.Abstractions
{
    /// <inheritdoc/>
    public abstract class AzStorageService<T> : IAzStorageService<T>
    {
        /// <summary>
        /// Calculates the delay for a retry attempt with exponential backoff and a random jitter.
        /// </summary>
        /// <param name="retryAttempt">The current retry attempt number.</param>
        /// <returns>A <see cref="TimeSpan"/> representing the delay for the retry attempt.</returns>
        public static TimeSpan CalculateDelay(int retryAttempt)
        {
            var delay = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds * Math.Pow(2, retryAttempt));
            var jitter = TimeSpan.FromMilliseconds(new Random().Next(0, 100));
            return delay + jitter;
        }

        /// <summary>
        /// Determines whether the add operation should be stopped based on the provided response.
        /// </summary>
        /// <typeparam name="TAzGlobalResponse">The type of the global response for Azure.</typeparam>
        /// <typeparam name="TAzureResponse">The type of the Azure response.</typeparam>
        /// <param name="response">The response to evaluate.</param>
        /// <returns><c>true</c> if the operation should be stopped; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="response"/> is null.</exception>
        protected bool ShouldStopAddOperation<TAzGlobalResponse, TAzureResponse>(TAzGlobalResponse response)
            where TAzureResponse : IAzureResponse
            where TAzGlobalResponse : IAzGlobalResponse<TAzureResponse, Exception>
        {
            AssertHelper.AssertNotNullOrThrow(response, nameof(response));

            return response.Status != ResponseStatus.Failure
                || (response.HasAzureResponse && !IsRetryableOperation(response.AzureResponse!));
        }

        /// <summary>
        /// Determines whether the get operation should be stopped based on the provided response.
        /// </summary>
        /// <typeparam name="TAzGlobalValueResponse">The type of the global value response for Azure.</typeparam>
        /// <typeparam name="TAzureResponse">The type of the Azure response.</typeparam>
        /// <param name="response">The response to evaluate.</param>
        /// <returns><c>true</c> if the operation should be stopped; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="response"/> is null.</exception>
        protected bool ShouldStopGetOperation<TAzGlobalValueResponse, TAzureResponse>(TAzGlobalValueResponse response)
            where TAzureResponse : IAzureResponse
            where TAzGlobalValueResponse : IAzGlobalValueResponse<T, TAzureResponse, Exception>
        {
            AssertHelper.AssertNotNullOrThrow(response, nameof(response));

            return response.Status == ResponseStatus.Success
                || (response.Status == ResponseStatus.Warning && response.HasValue)
                || (response.HasAzureResponse && !IsRetryableOperation(response.AzureResponse!));
        }

        /// <summary>
        /// Determines whether the load resource operation should be stopped based on the provided response.
        /// </summary>
        /// <typeparam name="TAzGlobalResponse">The type of the global response for Azure.</typeparam>
        /// <typeparam name="TAzureResponse">The type of the Azure response.</typeparam>
        /// <param name="response">The response to evaluate.</param>
        /// <returns><c>true</c> if the operation should be stopped; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="response"/> is null.</exception>
        protected bool ShouldStopLoadOperation<TAzGlobalResponse, TAzureResponse>(TAzGlobalResponse response)
            where TAzureResponse : IAzureResponse
            where TAzGlobalResponse : IAzGlobalResponse<TAzureResponse, Exception>
        {
            AssertHelper.AssertNotNullOrThrow(response, nameof(response));

            return response.Status == ResponseStatus.Failure
                && (response.HasAzureResponse && !IsRetryableOperation(response.AzureResponse!));
        }

        #region Private methods

        /// <summary>
        /// Determines whether the specified Azure response indicates a retryable operation.
        /// </summary>
        /// <typeparam name="TAzureResponse">The type of the Azure response.</typeparam>
        /// <param name="azureResponse">The Azure response to evaluate.</param>
        /// <returns><c>true</c> if the operation is retryable; otherwise, <c>false</c>.</returns>
        private bool IsRetryableOperation<TAzureResponse>(TAzureResponse azureResponse)
            where TAzureResponse : IAzureResponse
        {
            switch (azureResponse.Status)
            {
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.TooManyRequests:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
