using System.Net;

namespace ProjName.Application.Shared.Models;

public class BaseVM
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    public List<ResponseMessage> Messages { get; set; } = new List<ResponseMessage>();

    public BaseVM() { }
    public BaseVM(string message)
    {
        Messages.Add(new ResponseMessage { Message = message, Description = message });
    }
    public BaseVM(HttpStatusCode code, string message)
    {
        Messages.Add(new ResponseMessage { Message = message, Description = message });
        Status = code;
    }
}
public class BaseVM<T> : BaseVM
{
    public T Data { get; set; }
    public BaseVM() { }
    public BaseVM(T data)
    {
        Data = data;
    }
    public BaseVM(T data, string message) : base(message)
    {
        Data = data;
    }
    public BaseVM(T data, HttpStatusCode code, string message) : base(code, message)
    {
        Data = data;
        Status = code;
        Messages.Add(new ResponseMessage { Message = message, Description = message });
    }
}

public class ResponseMessage
{
    public string Message { get; set; }
    public string Description { get; set; }
}
public abstract class BaseEntityVM: BaseEntityVM<Guid>
{

}
public class SingleVM<T> : BaseVM
    where T : BaseEntityVM
{
    public T Data { get; set; }
    public SingleVM() { }
    public SingleVM(T data)
    {
        Data = data;
    }
    public SingleVM(T data, string message) : base(message)
    {
        Data = data;
        Messages.Add(new ResponseMessage { Message = message, Description = message });
    }
    public SingleVM(T data, HttpStatusCode code, string message) : base(code, message)
    {
        Data = data;
        Status = code;
        Messages.Add(new ResponseMessage { Message = message, Description = message });
    }
}
public class ListVM<T> : BaseVM
    where T : BaseEntityVM
{
    public IEnumerable<T> Data { get; set; }
    public PageDetails PageInfo { get; set; }
    public ListVM() { }
    public ListVM(IEnumerable<T> data)
    {
        Data = data;
    }
    public ListVM(IEnumerable<T> data, string message) : base(message)
    {
        Data = data;
        Messages.Add(new ResponseMessage { Message = message, Description = message });
    }
    public ListVM(IEnumerable<T> data, HttpStatusCode code, string message) : base(code, message)
    {
        Data = data;
        Status = code;
        Messages.Add(new ResponseMessage { Message = message, Description = message });
    }
}
public class FileVM: BaseVM
{
    public Stream File { get; private set; }
    public string FileType { get; private set; }
    public string FileName { get; private set; }

    public FileVM(Stream file, string fileType, string fileName)
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
public class PageDetails
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => TotalPages > 1 && PageNumber > 1;
    public bool HasNextPage => TotalPages > 1 && PageNumber < TotalPages;

    public PageDetails(int pageNum, int pageSize, int totalCount)
    {
        PageNumber = pageNum;
        PageSize = pageSize;
        TotalCount = totalCount;

        TotalPages = pageNum <= 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
