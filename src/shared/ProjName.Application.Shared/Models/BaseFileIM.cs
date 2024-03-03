namespace ProjName.Application.Shared.Models;

public abstract class BaseFileIM
{
    private readonly string DefaultFileName = "FILE";
    private Dictionary<string, FileIM> fileIMs = new Dictionary<string, FileIM>();
    public void SetFile(FileIM file, string name = "", bool withoutExtension = true)
    {
        if (name.IsNullOrEmpty())
        {
            fileIMs[DefaultFileName] = file;
        }
        else
        {
            name = Path.GetFileNameWithoutExtension(name);
            fileIMs[name] = file;
        }
    }
    public FileIM GetFile(string name = "", bool withoutExtension = true)
    {
        if(name.IsNullOrEmpty())
        {
            name = DefaultFileName;
        }

        if (withoutExtension)
        {
            name = Path.GetFileNameWithoutExtension(name);
        }

        if (!fileIMs.ContainsKey(name))
        {
            return null;
        }
        return fileIMs[name];
    }
}

public class FileIM
{
    public Stream File { get; private set; }
    public string FileType { get; private set; }
    public string FileName { get; private set; }

    public FileIM(Stream file, string fileType, string fileName)
    {
        if (file.Length == 0 || fileType.IsNullOrEmpty() || fileName.IsNullOrEmpty())
        {
            throw new ValidationException("Please provide valid file");
        }
        this.File = file;
        this.FileType = fileType;
        this.FileName = fileName;
    }
}
