namespace DevEventsApi.Middlewares;

public class LoggerMiddleware
{
  private readonly ILogger<LoggerMiddleware> logger;
  private readonly RequestDelegate next;

  public LoggerMiddleware(ILogger<LoggerMiddleware> logger, RequestDelegate next)
  {
    this.logger = logger;
    this.next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    // Run before request processing.
    var initTime = DateTime.UtcNow;

    await next.Invoke(context);

    // Run after request processing. (Response)
    logger.LogInformation($"Timing: {context.Request.Path}: {(DateTime.UtcNow - initTime).TotalMilliseconds}ms");
  }
}