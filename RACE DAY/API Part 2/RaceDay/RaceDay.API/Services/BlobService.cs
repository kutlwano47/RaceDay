using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDay.API.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IConfiguration config, ILogger<BlobService> logger)
        {
            var connectionString = config["AzureBlob:ConnectionString"];
            _containerName = config["AzureBlob:ContainerName"] ?? "race-images";
            _logger = logger;

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("AzureBlob:ConnectionString is not configured.");

            var clientOptions = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2023_11_03);
            _blobServiceClient = new BlobServiceClient(connectionString, clientOptions);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            try
            {
                var container = _blobServiceClient.GetBlobContainerClient(_containerName);
                await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var blobClient = container.GetBlobClient(fileName);
                var uploadOptions = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = string.IsNullOrWhiteSpace(file.ContentType)
                            ? "application/octet-stream"
                            : file.ContentType
                    }
                };

                await using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, uploadOptions);

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName} to Azure Blob container {ContainerName}.", file.FileName, _containerName);
                throw;
            }
        }
    }
}