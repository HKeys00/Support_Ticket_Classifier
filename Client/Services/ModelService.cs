using Shared.Enums.Ticket;
using Shared.Models;

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
        /// <returns>The predicted priority.</returns>
        public async Task<TicketPriority> GetPrediction(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync<Ticket>("model", ticket);

            return TicketPriority.Low;
        }

        #endregion
    }
}
