using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers
{
    /// <summary>
    /// Controller class that handles interactions with the ML model.
    /// </summary>
    [ApiController]
    [Route("model")]
    public class ModelController
    {
        #region Fields

        private readonly HttpClient _client;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelController"/> class.
        /// </summary>
        /// <param name="client"></param>
        public ModelController(HttpClient client)
        {
            _client = client;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a prediction of the ticket priority from the model.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        /// <returns>The prediction from the model as an int.</returns>
        public async Task<ActionResult<int>> GetPriorityPrediction([FromBody] Ticket ticket)
        {

        }

        #endregion
    }
}
