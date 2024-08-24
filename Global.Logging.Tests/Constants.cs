
namespace Global.Logging.Tests
{
    internal static class MockConstants
    {
        public static string Succeeded => "Succeeded";
        public static string MockedEntityPartitionKeyMessageKey => "mochedEntity.PartitionKey";
        public static string MockedEntityRowKeyMessageKey => "mochedEntity.RowKey";
        public static string EntityPartitionKeyMessageKey => "entity.PartitionKey";
        public static string EntityRowKeyMessageKey => "entity.RowKey";
        public static string PartitionKeyMessageKey => "partitionKey";
        public static string CancellationTokenMessageKey => "cancellationToken"; 
        public static string CreateIfNotExistsMessageKey => "createIfNotExists";
        public static string JsonSerializerOptionsMessageKey => "jsonSerializerOptions";
        public static string OverwriteMessageKey => "overwrite";
        public static string RowKeyMessageKey => "rowKey";
        public static string TableNameMessageKey => "tableName";
        public static string RawLogMessageKey => "rawLog";
        public static string BlobNameMessageKey => "blobName";
        public static string BlobContainerNameMessageKey => "blobContainerName";
        public static string EncodingMessageKey => "encoding";
    }

    internal static class Constants
    {
        public static string RowKeyFormat => "{0}_{1}_{2}";
        public static string ConstantLogLabel => "ConstantLogLabel";
        public static string ConstantVerboseLabel => "ConstantVerboseLabel";
    }
}
