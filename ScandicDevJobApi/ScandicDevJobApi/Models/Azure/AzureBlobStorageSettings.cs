namespace ScandicDevJobApi.Models.Azure
{
    public class AzureBlobStorageSettings
    {
        public required string ConnectionString { get; set; }
        public required string ContainerName { get; set; }
        public required string BaseUrl { get; set; }
    }
}
