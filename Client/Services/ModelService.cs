using Shared.Enums.Ticket;
using Shared.Models;
using System.Text.Json;

namespace Client.Services
{
    /// <summary>
    /// Service used to query the model api.
    /// </summary>
    public class ModelService
    {
        #region Fields

        IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelService"/> class.
        /// </summary>
        /// <param name="clientFactory"></param>
        public ModelService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a prediction of the ticket priority from the model.
        /// </summary>
        /// <param name="ticket">The ticket data to use.</param>
        /// <returns>The prediction from the model.</returns>
        public async Task<Prediction?> GetPriorityPrediction(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync<Ticket>("model", ticket);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var prediction = JsonSerializer.Deserialize<Prediction>(result);
                return prediction;
            }

            //TODO Proper error handling
            return null;
        }

        #endregion
    }
}
