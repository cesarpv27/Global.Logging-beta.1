
namespace Global.Logging.Services.Helpers
{
    internal static class AzServiceHelper
    {
        public static int GetMaxRetryAttempts(int defaultAddAzTableMaxRetryAttempts, bool retryOnFailures = true, int? maxRetryAttempts = null)
        {
            if (!retryOnFailures)
                return 1;

            if (retryOnFailures && maxRetryAttempts == null)
                return defaultAddAzTableMaxRetryAttempts;

            return maxRetryAttempts!.Value;
        }
    }
}
