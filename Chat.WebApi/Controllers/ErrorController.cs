using Chat.Domain.Exceptions;
using Chat.Domain.Exceptions.Common;
using Chat.WebApi.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
public class ErrorController : ControllerBase
{
    private IActionResult HandleError(string envName)
    {
        IExceptionHandlerFeature? exceptionHandlerFeature = HttpContext
            .Features
            .Get<IExceptionHandlerFeature>()!;

        Exception exception = exceptionHandlerFeature.Error;

        var businessException = exception as BusinessException;
        string? clientMessage = businessException?.ClientMessage;
        BusinessStatusCode? businessStatusCode = businessException?.BusinessStatusCode;

        object errorInfo = envName switch
        {
            AppEnvironment.Development
                => new
                {
                    clientMessage,
                    businessStatusCode,
                    exception.Message,
                    exception.Source,
                    exception.StackTrace,
                    exception.InnerException,
                    exception.Data,
                    exception.HelpLink,
                    exception.HResult,
                },
            _ => new { clientMessage, businessStatusCode }
        };

        ApiStatusCode statusCode =
            (exception as BusinessException)?.ApiStatusCode ?? ApiStatusCode.InternalServer;

        return StatusCode((int)statusCode, errorInfo);
    }

    [Route("Production")]
    public IActionResult HandleErrorProduction() => HandleError(AppEnvironment.Production);

    [Route("Development")]
    public IActionResult HandleErrorDevelopment() => HandleError(AppEnvironment.Development);
}
