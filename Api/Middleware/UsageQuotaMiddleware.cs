using Api.Services;

namespace Api.Middleware
{
    /// <summary>
    /// Middleware for capturing and throtteling user requests.
    /// </summary>
    public class UsageQuotaMiddleware : IMiddleware
    {
        #region Constants



        #endregion

        #region Fields

        private UsageQuotaService _usageQuotaService;
        private ILogger<UsageQuotaMiddleware> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageQuotaMiddleware"/> class.
        /// </summary>
        /// <param name="usage">The injected quota service instace.</param>
        /// <param name="logger">The injected logger instance.</param>
        public UsageQuotaMiddleware(UsageQuotaService usage, ILogger<UsageQuotaMiddleware> logger)
        {
            _usageQuotaService = usage;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var ip = context.Connection.RemoteIpAddress;
            if (ip == null)
            {
                _logger.LogError("Error getting client IP: Client IP address does not exist");
                return;
            }

            int timeout = await _usageQuotaService.GetTimeout(ip, context.Request.Path);
            await Task.Delay(timeout);

            await _usageQuotaService.CreateRequest(ip, context.Request.Path);
            await next.Invoke(context);
        }

        #endregion
    }
}
