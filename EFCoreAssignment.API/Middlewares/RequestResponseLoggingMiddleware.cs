using System.Text;
namespace EFCoreAssignment.API.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await LogRequest(context.Request);
        
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);
            
            await LogResponse(context.Response);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task LogRequest(HttpRequest request)
    {
        request.EnableBuffering();

        var requestLog = new StringBuilder();
        requestLog.AppendLine($"Request {request.Method} {request.Path}");
        requestLog.AppendLine($"Content-Type: {request.ContentType}");
        requestLog.AppendLine($"Host: {request.Host}");

        if (request.QueryString.HasValue)
        {
            requestLog.AppendLine($"QueryString: {request.QueryString}");
        }

        if (request.ContentLength > 0)
        {
            using (var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                requestLog.AppendLine($"Body: {body}");
                request.Body.Position = 0;
            }
        }

        _logger.LogInformation(requestLog.ToString());
    }

    private async Task LogResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        var responseLog = new StringBuilder();
        responseLog.AppendLine($"Response {response.StatusCode}");
        responseLog.AppendLine($"Content-Type: {response.ContentType}");
        responseLog.AppendLine($"Body: {text}");

        _logger.LogInformation(responseLog.ToString());
    }
}