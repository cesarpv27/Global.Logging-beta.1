
namespace Global.Logging.Tests
{
    internal static class LoggerAsserts
    {
        /// <summary>
        /// Assert <paramref name="actualResponse"/> not null and <see cref="IAzTableValueResponse{TValue}.Status"/> iguals to <see cref="ResponseStatus.Success"/>.
        /// Assert <see cref="IAzTableValueResponse{TValue}.Value"/> not null and <see cref="IAzTableValueResponse{TValue}.HasValue"/> has true.
        /// If <paramref name="compare"/> is specified, assert <see cref="IAzTableValueResponse{TValue}.Value"/> is equals to <paramref name="expectedValue"/> 
        /// using the specified <paramref name="compare"/>.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="expectedValue">Expected value.</param>
        /// <param name="actualResponse">The response.</param>
        /// <param name="compare">Func to compare <see cref="IAzTableValueResponse{TValue}.Value"/> and <paramref name="expectedValue"/></param>.
        public static void SuccessTableValueResponse<TResponse, TValue>(
            TValue expectedValue,
            TResponse actualResponse, 
            Func<TValue, ReadOnlyLog, bool>? compare) 
            where TResponse : class, IAzTableValueResponse<ReadOnlyLog>
        {
            Asserts.SuccessResponse(actualResponse);
            Assert.True(actualResponse.HasValue);
            Asserts.NotNull(actualResponse.Value);

            if (compare != null)
                Assert.True(compare(expectedValue, actualResponse.Value!));
        }

        /// <summary>
        /// Assert <paramref name="response"/> not null and <see cref="IAzBlobValueResponse{TValue}.Status"/> iguals to <see cref="ResponseStatus.Success"/>.
        /// Assert <see cref="IAzBlobValueResponse{TValue}.Value"/> not null and <see cref="IAzBlobValueResponse{TValue}.HasValue"/> has true.
        /// If <paramref name="compare"/> is specified, assert <see cref="IAzBlobValueResponse{TValue}.Value"/> is equals to <paramref name="expected"/> 
        /// using the specified <paramref name="compare"/>.
        /// </summary>
        /// <typeparam name="TResponse">Type of the response.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="expected">Expected value.</param>
        /// <param name="response">The response.</param>
        /// <param name="compare">Func to compare <see cref="IAzBlobValueResponse{TValue}.Value"/> and <paramref name="expected"/></param>.
        public static void SuccessBlobValueResponse<TResponse, TValue>(
            TValue expected,
            TResponse response,
            Func<TValue, ReadOnlyLog, bool>? compare)
            where TResponse : class, IAzBlobValueResponse<ReadOnlyLog>
        {
            Asserts.SuccessResponse(response);
            Assert.True(response.HasValue);
            Asserts.NotNull(response.Value);

            if (compare != null)
                Assert.True(compare(expected, response.Value!));
        }
    }
}
