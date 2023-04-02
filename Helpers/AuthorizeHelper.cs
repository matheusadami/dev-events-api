using System.Security.Claims;
namespace DevEventsApi.Helpers;

public static class AuthorizeHelper
{
  public static string GetUsernameOrRemoteAddress(HttpContext? httpContext)
  {
    if (httpContext is not null)
    {
      var username = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (username is not null)
        return username;

      return $"{httpContext.Connection.RemoteIpAddress} - {httpContext.Request.Headers.UserAgent}";
    }

    return string.Empty;
  }
}