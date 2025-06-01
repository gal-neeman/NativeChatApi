using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace NativeChat;

public class GlobalErrorHandler : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var error = new InternalServerError<string>(context.Exception.Message);
        var result = new JsonResult(error);
        result.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = result;
        context.ExceptionHandled = true;

        Log.Error("ERROR - Internal Server Error: '" + context.Exception + "'");
    }
}
