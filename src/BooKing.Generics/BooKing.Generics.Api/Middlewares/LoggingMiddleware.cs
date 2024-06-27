using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace BooKing.Generics.Api.Middlewares;
public class LoggingMiddleware
{
    private const string MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms with request body: {RequestBody} and response body: {ResponseBody}";

    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null)
            throw new ArgumentNullException(nameof(httpContext));

        var sw = Stopwatch.StartNew();
        string requestBody = null;
        
        httpContext.Request.EnableBuffering();

        if (httpContext.Request.ContentLength > 0 && httpContext.Request.Body.CanSeek)
        {
            httpContext.Request.Body.Position = 0;
            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;
            }
        }

        var originalBodyStream = httpContext.Response.Body;
        using var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;

        try
        {
            await _next(httpContext);
            sw.Stop();

            var statusCode = httpContext.Response?.StatusCode;
            var level = statusCode > 499 ? LogLevel.Error : LogLevel.Information;

            var responseBodyText = await ReadResponseBody(responseBody);            

            Log(level, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds, requestBody, responseBodyText);

            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex) when (LogException(httpContext, sw, ex, requestBody)) { }
    }

    private bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex, string requestBody)
    {
        sw.Stop();

        var responseBodyText = ReadResponseBody(httpContext.Response.Body);

        _logger.LogError(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds, requestBody, responseBodyText);
        return false;
    }

    private void Log(LogLevel level, string messageTemplate, string method, string path, int? statusCode, double elapsedMs, string requestBody, string responseBody)
    {
        if (level == LogLevel.Error)
        {
            _logger.LogError(messageTemplate, method, path, statusCode, elapsedMs, requestBody, responseBody);
        }
        else
        {
            _logger.LogInformation(messageTemplate, method, path, statusCode, elapsedMs, requestBody, responseBody);
        }
    }

    private async Task<string> ReadResponseBody(Stream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);

        return responseBodyText;
    }
}
