namespace ProjName.Application.Shared.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileToStorageAsync(string fileName, string contentType, Stream fileStream, string storageContainer = "ThinkStack", string contentDisposition = "attachment", bool isPublic = true, bool generateUri = true);
    Task<Stream> ReadFileFromStorageAsync(string docLink);
    Task DeleteFileAsync(string fileName, string containerName = "ThinkStack");
}
