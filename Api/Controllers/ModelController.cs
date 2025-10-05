using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers
{
    /// <summary>
    /// Controller class that handles interactions with the ML model.
    /// </summary>
    [ApiController]
    [Route("model")]
    public class ModelController : ControllerBase
    {
        #region Fields

        private readonly ILogger<ModelController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelController"/> class.
        /// </summary>
        /// <param name="logger">The injected logger instance.</param>
        /// <param name="clientFactory">The injected client factory instance.</param>
        public ModelController(ILogger<ModelController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a prediction of the ticket priority from the model.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        /// <returns>The prediction as a json string from the model.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetPriorityPrediction([FromBody] Ticket ticket)
        {
            using var client = _clientFactory.CreateClient();

            try
            {
                var response = await client.PostAsJsonAsync<Ticket>("http://localhost:3000/predict", ticket);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Model API returned error: {Status} - {Message}", response.StatusCode, json);
                    return StatusCode((int)response.StatusCode, json);
                }

                _logger.LogInformation("Successfully fetched prediction from model");
                return Ok(json);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching prediction from model.");
                return Problem(
                    detail: $"An unexpected error occured while fetching priority prediction from model: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
