
namespace Global.Logging.Online.Tests
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
        public static void SuccessTableValueResponse<TResponse>(TResponse? actualResponse)
            where TResponse : class, IAzTableValueResponse<ReadOnlyLog>
        {
            Asserts.SuccessResponse(actualResponse);
            Assert.True(actualResponse!.HasValue);
            Asserts.NotNull(actualResponse.Value);
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
        public static void SuccessBlobValueResponse<TResponse>(TResponse? response)
            where TResponse : class, IAzBlobValueResponse<ReadOnlyLog>
        {
            Asserts.SuccessResponse(response);
            Assert.True(response!.HasValue);
            Asserts.NotNull(response.Value);
        }

        public static void LabelSetFields(ReadOnlyLog readOnlyLog)
        {
            Asserts.NotNull(readOnlyLog);

            Assert.True(
                string.Equals(readOnlyLog.Label0, Constants.DefaultLabel0Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label1, Constants.DefaultLabel1Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label2, Constants.DefaultLabel2Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label3, Constants.DefaultLabel3Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label4, Constants.DefaultLabel4Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label5, Constants.DefaultLabel5Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label6, Constants.DefaultLabel6Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label7, Constants.DefaultLabel7Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label8, Constants.DefaultLabel8Message, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.Label9, Constants.DefaultLabel9Message, StringComparison.InvariantCulture));
        }

        public static void VerboseLabelSetFields(
            ReadOnlyLog readOnlyLog,
            Dictionary<string, string> verbose)
        {
            Asserts.NotNull(readOnlyLog);
            Asserts.NotNull(verbose);

            Assert.True(
                string.Equals(readOnlyLog.VerboseLabel0, verbose[RawLogHelper.VerboseKeyPrefix + 0], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel1, verbose[RawLogHelper.VerboseKeyPrefix + 1], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel2, verbose[RawLogHelper.VerboseKeyPrefix + 2], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel3, verbose[RawLogHelper.VerboseKeyPrefix + 3], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel4, verbose[RawLogHelper.VerboseKeyPrefix + 4], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel5, verbose[RawLogHelper.VerboseKeyPrefix + 5], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel6, verbose[RawLogHelper.VerboseKeyPrefix + 6], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel7, verbose[RawLogHelper.VerboseKeyPrefix + 7], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel8, verbose[RawLogHelper.VerboseKeyPrefix + 8], StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.VerboseLabel9, verbose[RawLogHelper.VerboseKeyPrefix + 9], StringComparison.InvariantCulture));
        }

        public static void ExceptionFields(
            ReadOnlyLog readOnlyLog,
            Exception exception)
        {
            Asserts.NotNull(readOnlyLog);
            Asserts.NotNull(exception);
            Asserts.NotNull(readOnlyLog.ExceptionType);
            Asserts.NotNull(readOnlyLog.ExceptionMessages);
            Asserts.NotNull(readOnlyLog.ExceptionStackTraces);
            Asserts.NotNull(readOnlyLog.ExceptionStackMethod);

            var exceptionType = exception.GetType().Name;
            var exceptionMessages = exception.GetAllMessagesFromExceptionHierarchy();
            var exceptionStackTraces = exception.GetAllStackTracesFromExceptionHierarchy();
            string? exceptionStackMethod = default;
            int? exceptionStackLine = default;
            int? exceptionStackColumn = default;

            var st = new StackTrace(exception, true);
            if (st.FrameCount > 0)
            {
                StackFrame? frame = st.GetFrame(0);

                if (frame != default)
                {
                    var method = frame.GetMethod();

                    if (method != null)
                        exceptionStackMethod = $"{method.DeclaringType?.FullName}.{method.Name}";

                    exceptionStackLine = frame.GetFileLineNumber();
                    exceptionStackColumn = frame.GetFileColumnNumber();
                }
            }

            Assert.True(
                string.Equals(readOnlyLog.ExceptionType, exceptionType, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.ExceptionMessages, exceptionMessages, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.ExceptionStackTraces, exceptionStackTraces, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.ExceptionStackMethod, exceptionStackMethod, StringComparison.InvariantCulture)
                && readOnlyLog.ExceptionStackLine == exceptionStackLine
                && readOnlyLog.ExceptionStackColumn == exceptionStackColumn);
        }

        public static void CallStackFields(ReadOnlyLog readOnlyLog)
        {
            Asserts.NotNull(readOnlyLog);

            Assert.True(string.Equals(readOnlyLog.CallStack, Constants.DefaultCallStackMessage, StringComparison.InvariantCulture)
                && string.Equals(readOnlyLog.CallStackMethod, Constants.DefaultCallStackMethodMessage, StringComparison.InvariantCulture)
                && readOnlyLog.CallStackLine == Constants.DefaultCallStackLineMessage
                && readOnlyLog.CallStackColumn == Constants.DefaultCallStackColumnMessage);
        }
    }
}
