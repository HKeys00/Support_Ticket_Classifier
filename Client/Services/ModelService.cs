using Shared.Models;
using Shared.Models.Result;
using System.Text.Json;

namespace Client.Services
{
    /// <summary>
    /// Service used to query the model api.
    /// </summary>
    public class ModelService
    {
        #region Fields

        private readonly IHttpClientFactory _clientFactory;

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
        public async Task<PredictionResult> GetPriorityPrediction(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("model/prediction", ticket);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var prediction = JsonSerializer.Deserialize<Prediction>(result);

                return PredictionResult.FromSuccess(prediction!);
            }

            var error = await response.Content.ReadAsStringAsync();
            return PredictionResult.FromError(
                $"Model API returned {(int)response.StatusCode} - {error}",
                response.StatusCode
            );
        }

        /// <summary>
        /// Gets a 
        /// </summary>
        /// <returns></returns>
        public async Task<RetrainResult> RetrainModel()
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PostAsync("model/retrain", null);

            if (response.IsSuccessStatusCode)
            {
                return RetrainResult.FromSuccess();
            }

            return RetrainResult.FromError(response.ReasonPhrase ?? string.Empty, response.StatusCode);
        }

        #endregion
    }
}
