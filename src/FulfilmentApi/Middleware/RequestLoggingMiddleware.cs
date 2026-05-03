class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Call the next middleware in the pipeline
        await _next(context);

        // Log the outgoing response
        _logger.LogInformation("{Method} {Path} ({ResponseStatusCode})", context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
}
