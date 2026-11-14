using Azure.Storage.Blobs;

namespace RaceDay.API.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _container;
        public BlobService(IConfiguration config)
        {
            var conn = config["AzureBlob:ConnectionString"];
            var name = config["AzureBlob:ContainerName"];
            _container = new BlobContainerClient(conn, name);
            _container.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blob = _container.GetBlobClient(fileName);
            using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, true);
            return blob.Uri.ToString();
        }
    }
}