using DevEventsApi.Middlewares;

namespace DevEventsApi.Extensions;

public static class LoggerExtensions
{
  public static IApplicationBuilder UseLogger(this IApplicationBuilder app)
  {
    return app.UseMiddleware<LoggerMiddleware>();
  }
}