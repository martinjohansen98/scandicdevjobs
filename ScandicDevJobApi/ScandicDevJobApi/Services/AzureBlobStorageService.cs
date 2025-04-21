using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ScandicDevJobApi.Models.Azure;
using System.Threading;

public class AzureBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string? _containerName;

    public AzureBlobStorageService(IOptions<AzureBlobStorageSettings> options)
    {
        // Use secrets or env-based config here
        var settings = options.Value;
        var connectionString = settings.ConnectionString;
        _containerName =settings.ContainerName;

        _blobServiceClient = new BlobServiceClient(connectionString);

    }

    public async Task<Guid> UploadFileAsync(Stream fileStream, string contentType, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var fileId = Guid.NewGuid();
        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(
            fileStream, 
            new BlobHttpHeaders { 
                ContentType = contentType 
            },
            cancellationToken: cancellationToken);

        return fileId;
    }

    public async Task<(Stream Content, string ContentType)> DownloadFileAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        var response = await blobClient.DownloadAsync(cancellationToken: cancellationToken);
        var properties = await blobClient.GetPropertiesAsync();

        return (response.Value.Content, properties.Value.ContentType);
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
