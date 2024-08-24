
namespace Global.Logging.Services.Constants
{
    internal static class AzServiceConstants
    {
        public static string MaxRetryAttemptsIfOutOfRange(string paramName)
        {
            return $"{paramName.WrapInSingleQuotationMarks()} is out of range. Must be greater than zero.";
        }

        public static string AzRetryOptionsNotFoundMessage => $"{nameof(AzRetryOptions)} not found.";
    }
}
