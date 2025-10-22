using System.Diagnostics;
using System.Text;

namespace Client.Middleware
{
    /// <summary>
    /// Middleware for logging the incoming request and outgoing response with duration
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        #region Fields

        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The injected logger instance.</param>
        /// <param name="next">The next action to perform in the middleware pipeline</param>
        public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invokes the middleware logic.
        /// </summary>
        /// <param name="context">The current http request reponse context.</param>
        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var request = await FormatRequest(context.Request);
            var originalBodyStream = context.Response.Body;

            string response = string.Empty;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context).ConfigureAwait(false);
                    response = await FormatResponse(context.Response);
                } catch (Exception ex)
                {
                    response = $"500: {ex.Message}";
                } finally
                {
                    stopwatch.Stop();
                    _logger.LogInformation(request);
                    _logger.LogInformation(response);
                    _logger.LogInformation("Execution time: {time}", stopwatch.Elapsed);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }

        /// <summary>
        /// Formats the request into a readable format for logging.
        /// </summary>
        /// <param name="request">The http request.</param>
        /// <returns>A formatted string.</returns>
        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadExactlyAsync(buffer);

            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        /// <summary>
        /// Formats the response into a readable format for logging.
        /// </summary>
        /// <param name="response">The http response.</param>
        /// <returns>A formatted string.</returns>
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{response.StatusCode}: {text}";
        }

        #endregion

    }

    /// <summary>
    /// Static class for adding the logging middleware.
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExetensions
    {
        /// <summary>
        /// Extension method for adding the logging middleware.
        /// </summary>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
