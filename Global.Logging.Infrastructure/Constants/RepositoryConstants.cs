

namespace Global.Logging.Infrastructure.Constants
{
    internal static class RepositoryConstants
    {
        public static string SerializerErrorMessageKey => "SerializerError";
        public static string DeserializerErrorMessageKey => "DeserializerError";
        public static string TableNameMessageKey => "TableName";
        public static string PartitionKeyMessageKey => "PartitionKey";
        public static string RowKeyMessageKey => "RowKey";
        public static string TableNameNullOrEmptyMessage => $"'{TableNameMessageKey}' is null or empty";
        public static string BlobContainerNameMessageKey => "BlobContainerName";
        public static string BlobContainerNameNullOrEmptyMessage => $"'{BlobContainerNameMessageKey}' is null or empty";
        public static string BlobNameMessageKey => "BlobName";
        public static string BlobNameNullOrEmptyMessage => $"'{BlobNameMessageKey}' is null or empty";

        public static string NullSerializerResult(string serializerParamName)
        {
            return $"The serializer {serializerParamName.WrapInSingleQuotationMarks()} has returned a null value.";
        }

        public static string NullDeserializerResult(string deserializeParamName)
        {
            return $"The deserializer {deserializeParamName.WrapInSingleQuotationMarks()} has returned a null value.";
        }
    }
}
