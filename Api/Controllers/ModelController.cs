using Api.Data;
using Api.Helpers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Enums.Ticket;
using Shared.Models;
using Shared;
using System.Text.Json;

namespace Api.Controllers
{
    /// <summary>
    /// Controller class that handles interactions with the ML model.
    /// </summary>
    [ApiController]
    [Route(ApiEndpoints.Model.Endpoint)]
    public class ModelController : ControllerBase
    {
        #region Constants

        private const string LLMUrl = "https://router.huggingface.co/v1/chat/completions";
        private const string ModelUrl = "http://localhost:3000/predict";

        #endregion


        #region Fields

        private readonly ILogger<ModelController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelController"/> class.
        /// </summary>
        /// <param name="logger">The injected logger instance.</param>
        /// <param name="clientFactory">The injected client factory instance.</param>
        /// <param name="context">The injected db context.</param>
        public ModelController(ILogger<ModelController> logger, IHttpClientFactory clientFactory, ApplicationDbContext context)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a prediction of the ticket priority from the model.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        /// <returns>The prediction as a json string from the model.</returns>
        [HttpPost(ApiEndpoints.Model.ModelPrediction)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetModelPriorityPrediction([FromBody] Ticket ticket)
        {
            using var client = _clientFactory.CreateClient();

            try
            {
                var response = await client.PostAsJsonAsync(ModelUrl, ticket.TicketToPredictionDto());
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

        /// <summary>
        /// Gets a prediction of the ticket priority from an LLM.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        /// <returns>The prediction as a json string from the LLM.</returns>
        [HttpPost(ApiEndpoints.Model.LLMPrediction)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetLLMPriorityPrediction([FromBody] Ticket ticket)
        {
            string? key = Environment.GetEnvironmentVariable("HUGGING_FACE_KEY");
            using var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            try
            {
                var content = new
                {
                    messages = new[]
                    {
                            new
                            {
                                role = "user",
                                content = $@"Given the support ticket description below return a ticket priority (Low, Medium, High or Critical) {ticket.Description}"
                }
                        },
                    model = "MiniMaxAI/MiniMax-M2"
                };

                var response = await client.PostAsJsonAsync(LLMUrl, content);
                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var prediction = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("LLM API returned error: {Status} - {Message}", response.StatusCode, json);
                    return StatusCode((int)response.StatusCode, json);
                }

                if (string.IsNullOrEmpty(prediction))
                {
                    throw new Exception("Prediction not found in response");
                }


                _logger.LogInformation("Successfully fetched prediction from LLM");
                var jsonPrediction = new { Prediction = prediction, Confidence = 1};
                return Ok(JsonSerializer.Serialize(jsonPrediction));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching prediction from LLM.");
                return Problem(
                    detail: $"An unexpected error occured while fetching priority prediction from LLM: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrains the model based on the corrections made to the tickets.
        /// </summary>
        /// <remarks>
        /// This method is deliberatly inefficient because I was messing around with 
        /// cancellation tokens
        /// </remarks>
        /// <param name="cancellation">The cancellation token.</param>
        [HttpPost(ApiEndpoints.Model.Retrain)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> RetrainModel(CancellationToken cancellation)
        {
            var corrections = await _context.Corrections
                .AsNoTracking()
                .Include(c => c.Ticket)
                .ToListAsync();

            if (corrections.Count == 0)
            {
                return Ok("Success");
            }

            corrections.ForEach(c => c.Ticket.Priority = (TicketPriority)c.CorrectedPriority);
            
            var tickets = corrections.Select(c => TicketHelper.TicketToCorrectionDto(c.Ticket)).ToList();
            using var client = _clientFactory.CreateClient();

            try
            {
                foreach (var ticket in tickets)
                {
                    cancellation.ThrowIfCancellationRequested();
                    var response = await client.PostAsJsonAsync("http://localhost:3000/retrain", ticket);
                    var json = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Model API returned error: {Status} - {Message}", response.StatusCode, json);
                        return StatusCode((int)response.StatusCode, json);
                    }

                    _logger.LogInformation("Successfull retrained the model");
                }

                await _context.Corrections.ExecuteDeleteAsync(cancellation);
                await _context.SaveChangesAsync();

                return Ok();
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error retraining model.");
                return Problem(
                    detail: $"An unexpected error occured while retraining the model: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
