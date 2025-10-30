using Shared;
using Shared.Models;
using Shared.Models.Result;
using System.Text.Json;
using OpenAI;
using OpenAI.Responses;

namespace Client.Services
{
    /// <summary>
    /// Service used to query the model api.
    /// </summary>
    public class ModelService
    {
        #region Constants
        https://huggingface.co/MiniMaxAI/MiniMax-M2
        private readonly string _key =
            "";

        #endregion
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
            using var client = _clientFactory.CreateClient(ApiEndpoints.Client);
            var response = await client.PostAsJsonAsync(
                Path.Combine(ApiEndpoints.Model.Endpoint, ApiEndpoints.Model.Prediction),
                ticket);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var prediction = JsonSerializer.Deserialize<Prediction>(result);

                if (prediction.Confidence[prediction.Value] < 0.25)
                {
                    try
                    {
                        var openClient = new OpenAIResponseClient("text-moderation", _key);

                        var aiResponse = await openClient.CreateResponseAsync(
                            userInputText:
                            $@"Given the support ticket description below return a ticket priority (Low, Medium, High or Critical)
                               {ticket.Description}",
                            new ResponseCreationOptions()
                            {
                                ReasoningOptions = new ResponseReasoningOptions()
                                {
                                    ReasoningEffortLevel = ResponseReasoningEffortLevel.Low,
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        var t = 2;
                    }
                    

                    var m = 0l;
                }

                return PredictionResult.FromSuccess(prediction!);
            }

            var error = await response.Content.ReadAsStringAsync();
            return PredictionResult.FromError(
                $"Model API returned {(int)response.StatusCode} - {error}",
                response.StatusCode
            );
        }

        /// <summary>
        /// Makes a request to retrain the model.
        /// </summary>
        /// <returns>The result of the retrain request.</returns>
        public async Task<RetrainResult> RetrainModel(CancellationToken cancellation)
        {
            using var client = _clientFactory.CreateClient(ApiEndpoints.Client);

            try
            {
                var response = await client.PostAsync(
                    Path.Combine(ApiEndpoints.Model.Endpoint, ApiEndpoints.Model.Retrain),
                    null, cancellation);
                if (response.IsSuccessStatusCode)
                {
                    return RetrainResult.FromSuccess();
                }

                return RetrainResult.FromError(response.ReasonPhrase ?? string.Empty, response.StatusCode);
            } catch (Exception ex)
            {
                return RetrainResult.FromError(ex.Message ?? string.Empty);
            }
        }

        #endregion
    }
}
