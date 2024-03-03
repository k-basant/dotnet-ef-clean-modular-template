using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using ProjName.Application.Shared;

namespace ProjName.API.Shared.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(ValidationException), HandleFluentValidationException },
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        if (context.Exception is ValidationException errorsObject && errorsObject.Errors != null)
        {
            HandleFluentValidationException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var result = new BaseVM(System.Net.HttpStatusCode.BadRequest, "ModelState exception");

        context.Result = new ObjectResult(result)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        };

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = context.Exception as NotFoundException;

        var result = new BaseVM(System.Net.HttpStatusCode.NotFound, exception!.Message);

        context.Result = new NotFoundObjectResult(result);

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var exception = context.Exception as UnauthorizedAccessException;

        var result = new BaseVM(System.Net.HttpStatusCode.Unauthorized, exception!.Message);

        context.Result = new ObjectResult(result)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var exception = context.Exception as ForbiddenAccessException;

        var result = new BaseVM(System.Net.HttpStatusCode.Forbidden, exception!.Message);

        context.Result = new ObjectResult(result)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var result = new BaseVM(System.Net.HttpStatusCode.InternalServerError, "Unhandled Exception");

        context.Result = new ObjectResult(result)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    /// <summary>
    /// All the errors and validation errors are thrown as ValidationException, processed in this method and returned as array of error messages
    /// </summary>
    /// <param name="context"></param>
    private void HandleFluentValidationException(ExceptionContext context)
    {
        var contextObject = context.Exception as ValidationException;

        var result = new BaseVM { Status = System.Net.HttpStatusCode.BadRequest };

        foreach (ValidationFailure error in contextObject!.Errors)
        {
            result.Messages.Add(new ResponseMessage()
            {
                Message = "Validation exception",
                Description = error.ErrorMessage,
            });
        }
        if (!result.Messages.Any())
        {
            result.Messages.Add(new ResponseMessage
            {
                Message = "Validation exception",
                Description = contextObject.Message
            });
        }
        context.Result = new ObjectResult(result)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest
        };

        context.ExceptionHandled = true;
    }
}
