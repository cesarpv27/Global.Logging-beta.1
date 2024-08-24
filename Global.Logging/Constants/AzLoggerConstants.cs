
namespace Global.Logging.Constants
{
    /// <summary>
    /// Provides constant values for AzLogger operations.
    /// </summary>
    public static class AzLoggerConstants
    {
        /// <summary>
        /// Gets the message key when writing is not allowed.
        /// </summary>
        public static string WritingNotAllowedMessageKey => "WritingNotAllowed";

        /// <summary>
        /// Gets the error message when writing is not allowed for the 'RawLog' specified.
        /// </summary>
        public static string WritingNotAllowedMessage => "The writing operation is not allowed for the 'RawLog' specified.";

        /// <summary>
        /// Gets the message key when reading is not allowed.
        /// </summary>
        public static string ReadingNotAllowedMessageKey => "ReadingNotAllowed";

        /// <summary>
        /// Gets the error message when reading is not allowed for the retrieved entity.
        /// </summary>
        public static string ReadingNotAllowedMessage => "The reading operation is not allowed for the retrieved entity.";

        /// <summary>
        /// Gets the default prefix for Azure log blob container names.
        /// </summary>
        public static string DefaultAzLogBlobContainerNamePrefix => "azlogs";

        /// <summary>
        /// Gets the default format string for row keys.
        /// </summary>
        public static string DefaultRowKeyFormat => "{0}_{1}_{2}";

        /// <summary>
        /// Gets a message indicating that the specified method in the specified class responded with the specified status.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="className">The name of the class.</param>
        /// <param name="responseStatus">The response status.</param>
        /// <returns>A message indicating the method response status.</returns>
        public static string GetMethodResponseStatusMessage(string methodName, string className, ResponseStatus responseStatus)
        {
            return $"The method {methodName.WrapInSingleQuotationMarks()} in class {className.WrapInSingleQuotationMarks()} responded with status {responseStatus}.";
        }

        /// <summary>
        /// Generates a message indicating that the specified store operation type is not supported.
        /// </summary>
        /// <param name="notSupportedTypeFullName">The full name of the unsupported type. If null, a default message "of element" is used.</param>
        /// <param name="paramName">The name of the parameter that contains the unsupported type.</param>
        /// <returns>A formatted string message indicating that the store operation type is not supported.</returns>
        public static string NotSupportedStoreOperationType(string? notSupportedTypeFullName, string paramName)
        {
            return $"The type {(notSupportedTypeFullName ?? "of element").WrapInSingleQuotationMarks()} in {paramName.WrapInSingleQuotationMarks()} is not supported.";
        }
    }
}
