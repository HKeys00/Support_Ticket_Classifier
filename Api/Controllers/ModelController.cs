using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Text.Json;

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

        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelController"/> class.
        /// </summary>
        /// <param name="client"></param>
        public ModelController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a prediction of the ticket priority from the model.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        /// <returns>The prediction from the model as an int.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetPriorityPrediction([FromBody] Ticket ticket)
        {
            using var client = _clientFactory.CreateClient();

            try
            {
                var response = await client.PostAsJsonAsync<Ticket>("http://localhost:3000/predict", ticket);
                var json = await response.Content.ReadAsStringAsync();
                var prediction = JsonSerializer.Deserialize<PredictionDto>(json);

                if (prediction == null) {
                    throw new Exception("");
                }

                return Ok(prediction.Prediction);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
