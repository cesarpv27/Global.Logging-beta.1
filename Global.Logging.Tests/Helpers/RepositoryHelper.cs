
namespace Global.Logging.Tests
{
    internal static class RepositoryHelper
    {
        public static BlobContentInfo CreateDefaultBlobContentInfo()
        {
            return BlobsModelFactory.BlobContentInfo(
                eTag: new ETag("\"0x8D4BCC2E4835CD0\""),
                lastModified: new DateTimeOffset(2000, 1, 1, 12, 10, 7, TimeSpan.Zero),
                contentHash: new byte[] { 0xBA, 0xD0, 0xBE, 0xEF },
                versionId: "2024-05-03T12:10:07.1234567Z",
                encryptionKeySha256: Convert.ToBase64String(new byte[] { 0xBA, 0xAD, 0xF0, 0x0D }),
                encryptionScope: "my-encryption-scope",
                blobSequenceNumber: 42
            );
        }
    }
}
