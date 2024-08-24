
namespace Global.Logging.Infrastructure.Helpers
{
    internal static class AzBlobRepositoryHelper
    {
        public static ExGlobalValueResponse<BinaryData> ConverTo(byte[] content)
        {
            try
            {
                return ExGlobalValueResponseFactory.CreateSuccessful(new BinaryData(content));
            }
            catch (Exception ex)
            {
                return ExGlobalValueResponseFactory.CreateFailure<BinaryData>(ex);
            }
        }
    }
}
