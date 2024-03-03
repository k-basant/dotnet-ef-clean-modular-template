using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ProjName.API.Shared.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{

    protected ISender Mediator => HttpContext.RequestServices.GetService<ISender>()!;

    protected ActionResult HandleResult(BaseVM result, ResponseType responseType = ResponseType.Json)
    {

        return result.Status switch
        {
            HttpStatusCode.OK => (responseType, result) switch
            {
                (_, FileVM file) => File(file.File, file.FileType, file.FileName),
                _ => Ok(result),
            },
            HttpStatusCode.NotFound => NotFound(result),
            HttpStatusCode.BadRequest => BadRequest(result),
            _ => StatusCode((int)result.Status, result)
        };
    }
}

public enum ResponseType
{
    Json        = 0,
    File        = 1,
}
