using System.Net;

namespace ProjName.Application.Shared.Models;

// Types defined here are suggested to be used by the shared projects only. For native development please use types defined in `BaseVMs.cs`
public abstract class BaseEntityVM<TPK> where TPK : struct
{
    public TPK? Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public TPK? CreatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public TPK? UpdatedById { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
public class SingleVM<T, TPK> : BaseVM
    where T : BaseEntityVM<TPK>
    where TPK: struct
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
public class ListVM<T, TPK> : BaseVM
    where T : BaseEntityVM<TPK>
    where TPK : struct
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