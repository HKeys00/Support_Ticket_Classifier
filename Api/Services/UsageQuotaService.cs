using Api.Data;
using Shared.Models;
using Shared;
using System.Net;
using Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    /// <summary>
    /// Service that manages user limits.
    /// </summary>
    public class UsageQuotaService : IUsageQuoataService
    {
        #region Constants

        private Dictionary<string, int> EndPointHourlyLimits = new()
        {
            {EndpointHelper.Combine(ApiEndpoints.Model.Endpoint, ApiEndpoints.Model.Retrain), 5}
        };

        private int TimeoutMS = 60000;

        #endregion

        #region Fields

        private ILogger<UsageQuotaService> _logger;
        private ApplicationDbContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageQuotaService"/> class.
        /// </summary>
        /// <param name="logger">The injected logger instance.</param>
        /// <param name="context">The injected db context.</param>
        public UsageQuotaService(ILogger<UsageQuotaService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #endregion

        #region Public

        /// <summary>
        /// Creates a request in the database.
        /// </summary>
        /// <param name="address">The ip address the request is coming from.</param>
        /// <param name="endpoint">The endpoint the request is trying to hit.</param>
        public async Task CreateRequest(IPAddress address, string endpoint)
        {
            try
            {
                string ip = address.ToString();
                await _context.Requests.AddAsync(new Request
                {
                    IpAddress = ip,
                    Url = endpoint,
                    DateTime = DateTime.UtcNow,
                });

                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added request to database: Ip {0}, endpoint {1}", ip, endpoint);
            } catch (Exception ex)
            {
                _logger.LogError("Error adding request to database {0}", ex.Message);
            }
        }


        /// <summary>
        /// Gets the required timeout based on the ip address.
        /// </summary>
        /// <param name="address">The ip address the request is coming from.</param>
        /// <param name="endpoint">The endpoint the request is trying to hit.</param>
        /// <returns>The required timeout.</returns>
        public async Task<int> GetTimeout(IPAddress address, string endpoint)
        {
            int limit = -1;
            if (EndPointHourlyLimits.TryGetValue(endpoint, out var value))
            {
                limit = value;
            }

            if (limit < 0)
            {
                return 0;
            }

            string ip = address.ToString();
            DateTime date = DateTime.UtcNow.AddHours(-1);

            var requests = await _context.Requests
                .AsNoTracking()
                .Where(r => r.IpAddress == ip)
                .Where(r => r.DateTime > date)
                .ToListAsync();
            int count = requests.Count;
             
            if (count >= limit)
            {
                return TimeoutMS;
            }

            return 0;
        }

        #endregion
    }
}
