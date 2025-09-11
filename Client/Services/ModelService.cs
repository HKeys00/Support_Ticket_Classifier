using Shared.Enums.Ticket;
using Shared.Models;

namespace Client.Services
{
    /// <summary>
    /// Service used to query the model api.
    /// </summary>
    public class ModelService
    {
        #region Methods

        /// <summary>
        /// Gets a prediction of the ticket priority from the model.
        /// </summary>
        /// <param name="ticket">The ticket data to use.</param>
        /// <returns>The predicted priority.</returns>
        public async Task<TicketPriority> GetPrediction(Ticket ticket)
        {

        }



        #endregion
    }
}
