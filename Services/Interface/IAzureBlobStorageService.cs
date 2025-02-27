namespace InventiCloud.Services.Interface
{
    public interface IAzureBlobStorageService
    {
        Task UploadFileAsync(IFormFile file, string fileName, string containerName);
        Task<bool> DeleteFileAsync(string fileName, string containerName);
        string GetFileUrl(string fileName, string containerName);
    }
}
