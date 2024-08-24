namespace Global.Logging.Models
{
    /// <summary>
    /// Provides constant values for store operations.
    /// </summary>
    public static class StoreOperationConstants
    {
        /// <summary>
        /// Gets the error message when the store operation type was not found in the store operation container.
        /// </summary>
        /// <param name="storeOperationTypeName">The name of the store operation type.</param>
        /// <returns>An error message indicating that the store operation type was not found in the store operation container.</returns>
        public static string GetStoreOperationTypeNotFound(string storeOperationTypeName) => $"Store operation type not found in the store operation container. Store operation type: {storeOperationTypeName}";

        /// <summary>
        /// Gets an error message indicating that the specified store operation category is not supported.
        /// </summary>
        /// <param name="storeOperationCategory">The store operation category.</param>
        /// <param name="paramName">The name of the parameter that contains the unsupported store operation category.</param>
        /// <returns>An error message indicating that the store operation type is not supported.</returns>
        public static string GetUnsupportedStoreOperationCategoryMessage(StoreOperationCategory storeOperationCategory, string paramName)
        {
            return $"The store operation category {storeOperationCategory.ToString().WrapInSingleQuotationMarks()} in {paramName.WrapInSingleQuotationMarks()} is not supported.";
        }
    }
}
