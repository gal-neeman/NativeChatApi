using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace NativeChat;

public class ExtractUserIdFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context) 
    {
        var user = context.HttpContext.User;
        string? userJson = user.FindFirst("user")?.Value;

        if (userJson == null) return;

        using var doc = JsonDocument.Parse(userJson);
        var root = doc.RootElement;
        string id = root.GetProperty("id").GetString()!;

        context.HttpContext.Items["UserId"] = id;
    }
}
