
namespace Global.Logging.Models
{
    /// <summary>
    /// Represents a blob storage repository in Azure, inheriting from <see cref="IAzStorageRepository"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <remarks>
    /// A blob container name in the correct format for Azure Blob Storage must follow these conditions:<br/>
    /// - All characters must be in the English alphabet.<br/>
    /// - All characters must be letters, numbers, or the '-' character.<br/>
    /// - All characters must be lowercase.<br/>
    /// - The first and last characters must be a letter or a number.<br/>
    /// - The length must be between 3 and 63 characters.<br/>
    /// </remarks>
    public interface IAzBlobRepository<T> : IAzStorageRepository
         where T : class
    {
        /// <summary>
        /// Serializes the specified <paramref name="value"/> to JSON using the default serializer <see cref="JsonSerializer.Serialize{T}(T, JsonSerializerOptions?)"/>.
        /// </summary>
        /// <param name="value">The value to serialize to JSON.</param>
        /// <param name="options">Optional. The serializer options to use during serialization.</param>
        /// <returns>A JSON string representation of the value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">
        /// There is no compatible <see cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.
        /// </exception>
        string DefaultJsonSerialize(T value, JsonSerializerOptions? options = null);

        /// <summary>
        /// Serializes the specified <paramref name="value"/> to JSON using the default serializer function <see cref="DefaultJsonSerialize"/>.
        /// </summary>
        /// <param name="value">The value to serialize to JSON.</param>
        /// <param name="options">Optional. The serializer options to use during serialization.</param>
        /// <returns>The <see cref="AzBlobValueResponse{JSON}"/> associated with the result of the operation. 
        /// The response contains the serialized JSON string representing the result of the serialization operation if <paramref name="value"/> 
        /// has been successfully serialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">
        /// There is no compatible <see cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.
        /// </exception>
        AzBlobValueResponse<string> Serialize(T value, JsonSerializerOptions? options = null);

        /// <summary>
        /// Serializes the specified <paramref name="value"/> to JSON using the custom serializer function <paramref name="serialize"/>.
        /// </summary>
        /// <param name="value">The value to serialize to JSON.</param>
        /// <param name="serialize">The function used for serialization to JSON.</param>
        /// <returns>The <see cref="AzBlobValueResponse{JSON}"/> associated with the result of the operation. 
        /// The response contains the serialized JSON string representing the result of the serialization operation if <paramref name="value"/> 
        /// has been successfully serialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        AzBlobValueResponse<string> Serialize(T value, Func<T, string> serialize);

        /// <summary>
        /// Asynchronously serializes the specified <paramref name="value"/> to JSON using the default serializer <see cref="JsonSerializer.SerializeAsync{T}(Stream, T, JsonSerializerOptions?, CancellationToken)"/>.
        /// </summary>
        /// <param name="value">The value to serialize to JSON.</param>
        /// <param name="options">Optional. The serializer options to use during serialization.</param>
        /// <param name="encoding">Optional. The character encoding to use during serialization.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains a JSON string representation of the value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">
        /// There is no compatible <see cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.
        /// </exception>
        Task<string?> DefaultJsonSerializeAsync(T value, JsonSerializerOptions? options = null, Encoding? encoding = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously serializes the specified <paramref name="value"/> to JSON using the default serializer function <see cref="DefaultJsonSerializeAsync"/>.
        /// </summary>
        /// <param name="value">The value to serialize to JSON.</param>
        /// <param name="options">Optional. The serializer options to use during serialization.</param>
        /// <param name="encoding">Optional. The character encoding to use during serialization.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{JSON}"/> associated with the result of the operation. 
        /// The response contains the serialized JSON string representing the result of the serialization operation if <paramref name="value"/> 
        /// has been successfully serialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="NotSupportedException">
        /// There is no compatible <see cref="JsonConverter"/> for <typeparamref name="T"/> or its serializable members.
        /// </exception>
        Task<AzBlobValueResponse<string>> SerializeAsync(T value, JsonSerializerOptions? options = null, Encoding? encoding = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously serializes the specified <paramref name="value"/> to JSON using the custom serializer function <paramref name="serializeAsync"/>.
        /// </summary>
        /// <param name="value">The value to serialize to JSON.</param>
        /// <param name="serializeAsync">The asynchronous function used for serialization to JSON.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{JSON}"/> associated with the result of the operation. 
        /// The response contains the serialized JSON string representing the result of the serialization operation if <paramref name="value"/> 
        /// has been successfully serialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="serializeAsync"/> are null.</exception>
        Task<AzBlobValueResponse<string>> SerializeAsync(T value, Func<T, CancellationToken, Task<string>> serializeAsync, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deserializes the content of <paramref name="stream"/> using the default deserializer <see cref="JsonSerializer.Deserialize{T}(Stream, JsonSerializerOptions?)"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        /// <param name="options">Optional. Options to control the behavior of the deserializer during reading.</param>
        /// <returns>The instance of <typeparamref name="T"/> representing the result of the deserialization operation if 
        /// <paramref name="stream"/> has been successfully deserialized; otherwise, returns null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
        /// <exception cref="JsonException">Thrown when <paramref name="stream"/> contains an invalid JSON,
        /// or <typeparamref name="T"/> is not compatible with the JSON or is remaining data in the stream.</exception>
        /// <exception cref="NotSupportedException">Thrown when there is no compatible <see cref="JsonConverter"/> 
        /// for <typeparamref name="T"/> or its serializable members.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
        T? DefaultJsonDeserialize(Stream stream, JsonSerializerOptions? options = null);

        /// <summary>
        /// Deserializes the content of <paramref name="stream"/> using the default deserializer function <see cref="DefaultJsonDeserialize"/>.
        /// </summary>
        /// <param name="stream">The stream containing the content to deserialize.</param>
        /// <param name="options">Optional. Options to control the behavior of the deserializer during reading.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/> 
        /// representing the result of the deserialization operation if <paramref name="stream"/> has been successfully deserialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
        /// <exception cref="JsonException">Thrown when <paramref name="stream"/> contains an invalid JSON,
        /// or <typeparamref name="T"/> is not compatible with the JSON or is remaining data in the stream.</exception>
        /// <exception cref="NotSupportedException">Thrown when there is no compatible <see cref="JsonConverter"/> 
        /// for <typeparamref name="T"/> or its serializable members.</exception>
        AzBlobValueResponse<T> Deserialize(Stream stream, JsonSerializerOptions? options = null);

        /// <summary>
        /// Deserializes the content of <paramref name="stream"/> using the custom deserializer function <paramref name="deserialize"/>.
        /// </summary>
        /// <param name="stream">The stream containing the content to deserialize.</param>
        /// <param name="deserialize">The function used for deserialization.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/> 
        /// representing the result of the deserialization operation if <paramref name="stream"/> has been successfully deserialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="deserialize"/> is null.</exception>
        AzBlobValueResponse<T> Deserialize(Stream stream, Func<Stream, T?> deserialize);

        /// <summary>
        /// Asynchronously deserializes the content of <paramref name="stream"/> using the default deserializer <see cref="JsonSerializer.Deserialize{T}(Stream, JsonSerializerOptions?)"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize.</param>
        /// <returns>The instance of <typeparamref name="T"/> representing the result of the deserialization operation if 
        /// <paramref name="stream"/> has been successfully deserialized; otherwise, returns null.</returns>
        /// <param name="options">Options to control the behavior of the deserializer during reading.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the instance of <typeparamref name="T"/> representing the result of the deserialization operation if 
        /// <paramref name="stream"/> has been successfully deserialized; otherwise, returns null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
        /// <exception cref="JsonException">Thrown when <paramref name="stream"/> contains an invalid JSON,
        /// or <typeparamref name="T"/> is not compatible with the JSON or is remaining data in the stream.</exception>
        /// <exception cref="NotSupportedException">Thrown when there is no compatible System.Text.Json.Serialization.JsonConverter 
        /// for <typeparamref name="T"/> or its serializable members.</exception>
        Task<T?> DefaultJsonDeserializeAsync(Stream stream, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deserializes the content of <paramref name="stream"/> using the default deserializer function <see cref="DefaultJsonDeserializeAsync"/>.
        /// </summary>
        /// <param name="stream">The stream containing the content to deserialize.</param>
        /// <param name="options">Options to control the behavior of the deserializer during reading.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/> 
        /// representing the result of the deserialization operation if <paramref name="stream"/> has been successfully deserialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is null.</exception>
        /// <exception cref="JsonException">Thrown when <paramref name="stream"/> contains an invalid JSON,
        /// or <typeparamref name="T"/> is not compatible with the JSON or is remaining data in the stream.</exception>
        /// <exception cref="NotSupportedException">Thrown when there is no compatible System.Text.Json.Serialization.JsonConverter 
        /// for <typeparamref name="T"/> or its serializable members.</exception>
        Task<AzBlobValueResponse<T>> DeserializeAsync(Stream stream, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deserializes the content of <paramref name="stream"/> using the custom deserializer function <paramref name="deserializeAsync"/>.
        /// </summary>
        /// <param name="stream">The stream containing the content to deserialize.</param>
        /// <param name="deserializeAsync">The asynchronous function used for deserialization.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/> 
        /// representing the result of the deserialization operation if <paramref name="stream"/> has been successfully deserialized; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> or <paramref name="deserializeAsync"/> are null.</exception>
        Task<AzBlobValueResponse<T>> DeserializeAsync(Stream stream, Func<Stream, CancellationToken, Task<T?>> deserializeAsync, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads the Azure Blob Container specified by <paramref name="blobContainerName"/> if it is not the current Azure Blob Container. 
        /// In other case, the current Azure Blob Container is retained.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="createIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <returns></returns>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation.
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="blobContainerName"/> is the current Azure Blob Container or has been successfully loaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="blobContainerName"/> has an incorrect format for Azure Blob Storage.</exception>
        AzBlobResponse LoadBlobContainerClient(string blobContainerName, bool createIfNotExists = true);

        /// <summary>
        /// Asynchronously loads the Azure Blob Container specified by <paramref name="blobContainerName"/> if it is not the current Azure Blob Container. 
        /// In other case, the current Azure Blob Container is retained.
        /// </summary>
        /// <param name="blobContainerName">Name of Azure Blob Container.</param>
        /// <param name="createIfNotExists">If true, create the Azure Blob Container if it does not exist.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation.
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="blobContainerName"/> is the current Azure Blob Container or has been successfully loaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="blobContainerName"/> has an incorrect format for Azure Blob Storage.</exception>
        Task<AzBlobResponse> LoadBlobContainerClientAsync(string blobContainerName, bool createIfNotExists = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Loads the Azure Blob specified by <paramref name="blobName"/> if it is not the current Azure Blob. In other case, the current Azure Blob is retained. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation.
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="blobName"/> has been successfully loaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' has not been loaded.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="blobName"/> is null, empty or white spaces.</exception>
        AzBlobResponse LoadBlobClient(string blobName);

        /// <summary>
        /// Asynchronously loads the Azure Blob specified by <paramref name="blobName"/> if it is not the current Azure Blob. 
        /// In other case, the current Azure Blob is retained. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="blobName">Name of Azure Blob. Must include the directory path of the blob.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="blobName"/> has been successfully loaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' has not been loaded.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="blobName"/> is null, empty or white spaces.</exception>
        Task<AzBlobResponse> LoadBlobClientAsync(string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads the content of <paramref name="entity"/> serialized by the default function <see cref="DefaultJsonSerialize"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="entity">Entity to upload.</param>
        /// <param name="jsonSerializerOptions">Optional. The serializer options to use during serialization.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the serialized <paramref name="entity"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="entity"/> is null.</exception>
        AzBlobValueResponse<BlobContentInfo> Upload(T entity, JsonSerializerOptions? jsonSerializerOptions = null, bool overwrite = false);

        /// <summary>
        /// Asynchronously uploads the content of <paramref name="entity"/> serialized by the default function <see cref="DefaultJsonSerializeAsync"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="entity">Entity to upload.</param>
        /// <param name="jsonSerializerOptions">Optional. The serializer options to use during serialization.</param>
        /// <param name="encoding">Optional. The character encoding to use during serialization.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the serialized <paramref name="entity"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="entity"/> is null.</exception>
        Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(T entity, JsonSerializerOptions? jsonSerializerOptions = null, Encoding? encoding = null, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads the content of <paramref name="entity"/> serialized by <paramref name="serialize"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="entity">Entity to upload.</param>
        /// <param name="serialize">Used to serialize the content of <paramref name="entity"/> to <see cref="string"/>.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the serialized <paramref name="entity"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="serialize"/> or <paramref name="entity"/> are null.</exception>
        AzBlobValueResponse<BlobContentInfo> Upload(T entity, Func<T, string> serialize, bool overwrite = false);

        /// <summary>
        /// Asynchronously uploads the content of <paramref name="entity"/> serialized by <paramref name="serializeAsync"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="entity">Entity to upload.</param>
        /// <param name="serializeAsync">The asynchronous function used to serialize the <paramref name="entity"/> to <see cref="string"/>.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if the serialized <paramref name="entity"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="serializeAsync"/> or <paramref name="entity"/> are null.</exception>
        Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(T entity, Func<T, CancellationToken, Task<string>> serializeAsync, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads <paramref name="content"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="content">Content to upload.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="content"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="content"/> is null.</exception>
        AzBlobValueResponse<BlobContentInfo> Upload(BinaryData content, bool overwrite = false);

        /// <summary>
        /// Asynchronously uploads <paramref name="content"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="content">Content to upload.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="content"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="content"/> is null.</exception>
        Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(BinaryData content, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads the <paramref name="content"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="content">Content to upload.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="content"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="content"/> is null.</exception>
        AzBlobValueResponse<BlobContentInfo> Upload(MemoryStream content, bool overwrite = false);

        /// <summary>
        /// Asynchronously uploads the <paramref name="content"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="content">Content to upload.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="content"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="content"/> is null.</exception>
        Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(MemoryStream content, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads the <paramref name="content"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="content">Content to upload.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation.
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="content"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="content"/> is null.</exception>
        AzBlobValueResponse<BlobContentInfo> Upload(byte[] content, bool overwrite = false);

        /// <summary>
        /// Asynchronously uploads the <paramref name="content"/> to the Azure Blob Storage. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="content">Content to upload.</param>
        /// <param name="overwrite">Whether the upload should overwrite any existing blobs. The default value is false.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobContentInfo}"/> associated with the result of the operation. 
        /// The response contains the Status set to <see cref="ResponseStatus.Success"/> if <paramref name="content"/> has been successfully uploaded; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if
        /// <paramref name="content"/> is null.</exception>
        Task<AzBlobValueResponse<BlobContentInfo>> UploadAsync(byte[] content, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the content from the Azure Blob Storage and deserializes it into an instance of type <typeparamref name="T"/>, 
        /// using the default deserializer function <see cref="DefaultJsonDeserialize"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="serializerOptions">Options to control the behavior of the deserializer during reading.</param>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        AzBlobValueResponse<T> Get(JsonSerializerOptions? serializerOptions = null, BlobDownloadOptions? options = null);

        /// <summary>
        /// Asynchronously retrieves the content from the Azure Blob Storage and deserializes it into an instance of type <typeparamref name="T"/>, 
        /// using the default deserializer function <see cref="DefaultJsonDeserializeAsync"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="serializerOptions">Options to control the behavior of the deserializer during reading.</param>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        Task<AzBlobValueResponse<T>> GetAsync(JsonSerializerOptions? serializerOptions = null, BlobDownloadOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the content from the Azure Blob Storage and deserializes it into an instance of type <typeparamref name="T"/>,
        /// using the custom deserializer function <paramref name="deserialize"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="deserialize">The function used for deserialization.</param>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <returns>The <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if 
        /// <paramref name="deserialize"/> is null.</exception>
        AzBlobValueResponse<T> Get(Func<Stream, T?> deserialize, BlobDownloadOptions? options = null);

        /// <summary>
        /// Asynchronously retrieves the content from the Azure Blob Storage and deserializes it into an instance of type <typeparamref name="T"/>,
        /// using the custom deserializer function <paramref name="deserializeAsync"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="deserializeAsync">The asynchronous function used for deserialization.</param>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{T}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the instance of <typeparamref name="T"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded, or if 
        /// <paramref name="deserializeAsync"/> is null.</exception>
        Task<AzBlobValueResponse<T>> GetAsync(Func<Stream, CancellationToken, Task<T?>> deserializeAsync, BlobDownloadOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the content from the Azure Blob Storage as a streaming result. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobDownloadStreamingResult}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the streaming result of the blob download operation; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        AzBlobValueResponse<BlobDownloadStreamingResult> DownloadStreaming(BlobDownloadOptions? options = null);

        /// <summary>
        /// Asynchronously downloads the content from the Azure Blob Storage as a streaming result. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobDownloadStreamingResult}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the streaming result of the blob download operation; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        Task<AzBlobValueResponse<BlobDownloadStreamingResult>> DownloadStreamingAsync(BlobDownloadOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the content from the Azure Blob Storage as <see cref="Stream"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <returns>The <see cref="AzBlobValueResponse{Stream}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the stream with the content of the downloaded blob; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        AzBlobValueResponse<Stream> DownloadStream(BlobDownloadOptions? options = null);

        /// <summary>
        /// Asynchronously downloads the content from the Azure Blob Storage as <see cref="Stream"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{Stream}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and the stream with the content of the downloaded blob; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        Task<AzBlobValueResponse<Stream>> DownloadStreamAsync(BlobDownloadOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the content from the Azure Blob Storage as <see cref="BlobDownloadResult"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <returns>The <see cref="AzBlobValueResponse{BlobDownloadResult}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and 
        /// the <see cref="BlobDownloadResult"/> with information about the downloaded blob; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        AzBlobValueResponse<BlobDownloadResult> DownloadContent(BlobDownloadOptions? options = null);

        /// <summary>
        /// Asynchronously downloads the content from the Azure Blob Storage as <see cref="BlobDownloadResult"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobDownloadResult}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and 
        /// the <see cref="BlobDownloadResult"/> with information about the downloaded blob; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        Task<AzBlobValueResponse<BlobDownloadResult>> DownloadContentAsync(BlobDownloadOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the content from the Azure Blob Storage to the specified <paramref name="destinationStream"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="destinationStream">The stream to which the blob content will be downloaded.</param>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <returns>The <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> indicating that
        /// the blob content has been successfully downloaded to the <paramref name="destinationStream"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        AzBlobResponse DownloadTo(Stream destinationStream, BlobDownloadToOptions? options = null);

        /// <summary>
        /// Asynchronously downloads the content from the Azure Blob Storage to the specified <paramref name="destinationStream"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <param name="destinationStream">The stream to which the blob content will be downloaded.</param>
        /// <param name="options">Optional blob download options to configure the download operation.</param>
        /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobResponse"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> indicating that
        /// the blob content has been successfully downloaded to the <paramref name="destinationStream"/>; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        Task<AzBlobResponse> DownloadToAsync(Stream destinationStream, BlobDownloadToOptions? options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the content from the Azure Blob Storage as <see cref="BlobDownloadInfo"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <returns>The <see cref="AzBlobValueResponse{BlobDownloadInfo}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and 
        /// the <see cref="BlobDownloadInfo"/> with information about the downloaded blob; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        [Obsolete("This method includes 'BlobClient.Download()', which has been deprecated by Microsoft. Please consider using the options described in the summary.", false)]
        AzBlobValueResponse<BlobDownloadInfo> Download();

        /// <summary>
        /// Asynchronously downloads the content from the Azure Blob Storage as <see cref="BlobDownloadInfo"/>. 
        /// The 'BlobContainerClient' must be properly initialized using either the <see cref="LoadBlobContainerClient"/> or
        /// <see cref="LoadBlobContainerClientAsync"/> methods before attempting to perform this operation.
        /// The 'BlobClient' must be properly initialized using either the <see cref="LoadBlobClient"/> or
        /// <see cref="LoadBlobClientAsync"/> methods before attempting to perform this operation.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The resulting task contains the <see cref="AzBlobValueResponse{BlobDownloadInfo}"/> associated with the result of the operation. 
        /// If successful, the response contains the Status set to <see cref="ResponseStatus.Success"/> and 
        /// the <see cref="BlobDownloadInfo"/> with information about the downloaded blob; 
        /// otherwise, the response contains the Status set to <see cref="ResponseStatus.Failure"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when 'BlobContainerClient' or 'BlobClient' have not been loaded.</exception>
        Task<AzBlobValueResponse<BlobDownloadInfo>> DownloadAsync(CancellationToken cancellationToken = default);
    }
}
