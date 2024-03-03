using ProjName.Application.Shared.Interfaces;
using System.Security.Claims;

namespace ProjName.API.Shared.Services;

public class RequestContextService : IRequestContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    public RequestContextService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public bool IsUserLoggedIn => GetLoggedInUserId() != null;

    public string? GetLoggedInUserEmail()
    {
        Claim? claim = FindClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        return claim != null ? claim.Value : null;
    }

    public Guid? GetLoggedInUserId()
    {
        Claim? claim = FindClaim("http://schemas.microsoft.com/identity/claims/objectidentifier");
        return claim != null ? new Guid(claim.Value) : null;
    }

    private Claim? FindClaim(string claimName)
    {
        ClaimsIdentity? claimsIdentity = _contextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        if (claimsIdentity == null)
        {
            return null;
        }
        Claim? claim = claimsIdentity?.FindFirst(claimName);
        return claim;
    }
}
