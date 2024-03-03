using ProjName.Application.Shared.Interfaces;
using System.Reflection;

namespace ProjName.Infrastructure.Shared.Services;

public class DiskFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public DiskFileStorageService()
    {
        _rootPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;
    }

    public Task<string> SaveFileToStorageAsync(string fileName, string contentType, Stream fileStream, string storageContainer = "ThinkStack", string contentDisposition = "attachment", bool isPublic = true, bool generateUri = true)
    {
        var containerPath = Path.Combine(_rootPath, storageContainer);

        if (!Directory.Exists(containerPath))
        {
            Directory.CreateDirectory(containerPath);
        }

        var filePath = Path.Combine(containerPath, fileName);

        using (var fileStreamOutput = File.Create(filePath))
        {
            fileStream.CopyTo(fileStreamOutput);
            fileStream.Position = 0;
        }

        if (generateUri)
        {
            return Task.FromResult(new Uri(filePath).AbsoluteUri);
        }

        return Task.FromResult(filePath);
    }

    public Task<Stream> ReadFileFromStorageAsync(string docLink)
    {
        var uri = new Uri(docLink);
        var localPath = uri.LocalPath;

        if (!File.Exists(localPath))
        {
            throw new FileNotFoundException("The requested file could not be found.", localPath);
        }

        FileStream fileStream = File.Open(localPath, FileMode.Open, FileAccess.Read);
        return Task.FromResult((Stream)fileStream);
    }

    public Task DeleteFileAsync(string fileName, string containerName = "ThinkStack")
    {
        var containerPath = Path.Combine(_rootPath, containerName);
        var filePath = Path.Combine(containerPath, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The file to delete could not be found.", filePath);
        }

        File.Delete(filePath);

        return Task.CompletedTask;
    }
}
