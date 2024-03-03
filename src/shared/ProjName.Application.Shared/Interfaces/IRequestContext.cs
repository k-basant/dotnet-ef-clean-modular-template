namespace ProjName.Application.Shared.Interfaces;

#nullable enable
public interface IRequestContext
{
    bool IsUserLoggedIn { get; }
    Guid? GetLoggedInUserId();
    string? GetLoggedInUserEmail();
}
