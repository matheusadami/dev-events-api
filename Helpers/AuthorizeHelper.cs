using System.Security.Claims;
using DevEventsApi.Config;

namespace DevEventsApi.Helpers;

public static class AuthorizeHelper
{
  public static string GetUserIdentifierOrError(HttpContext? httpContext)
  {
    if (httpContext is null)
      throw new ArgumentNullException("HttpContext not found.");

    var userIdentifier = httpContext.User.FindFirst(AuthSettings.UserIdentifierClaimTypeName)?.Value;
    return userIdentifier ?? throw new Exception("User Identifier not found.");
  }

  public static string GetUsernameOrRemoteAddress(HttpContext? httpContext)
  {
    if (httpContext is null)
      return string.Empty;

    var username = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (username is not null)
      return username;

    return $"{httpContext.Connection.RemoteIpAddress} - {httpContext.Request.Headers.UserAgent}";
  }
}