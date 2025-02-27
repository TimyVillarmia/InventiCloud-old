using Azure.Storage.Blobs;
using InventiCloud.Services.Interface;

namespace InventiCloud.Services
{
    public class AzureBlobStorageService(IConfiguration configuration) : IAzureBlobStorageService
    {
        private string? _connectionString = configuration["AzureBlobStorage:ConnectionString"];
        private string? _baseUrl = configuration["AzureBlobStorage:BaseUrl"];

        public async Task<bool> DeleteFileAsync(string fileName, string containerName)
        {
            // Create the container and return a container client object
            BlobContainerClient container = new BlobContainerClient(_connectionString, containerName);

            // Get a reference to a blob
            BlobClient blobClient = container.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public string GetFileUrl(string fileName, string containerName)
        {
            return $"{_baseUrl}/{containerName}/{fileName}";
        }

        public async Task UploadFileAsync(IFormFile file,string fileName, string containerName)
        {
            // Create the container and return a container client object
            BlobContainerClient container = new BlobContainerClient(_connectionString, containerName);

            // Get a reference to a blob
            BlobClient blobClient = container.GetBlobClient(fileName);

            // Upload data from the local file, overwrite the blob if it already exists
            await blobClient.UploadAsync(file.ContentType, true);

        }
    }
}
