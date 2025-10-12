using Shared.Models;
using Shared.Models.Result;

namespace Client.Services
{
    /// <summary>
    /// Service that communicates with the ticket api.
    /// </summary>
    public class TicketService
    {
        #region Fields

        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// </summary>
        /// <param name="clientFactory">The injected <see cref="IHttpClientFactory"/> instance.></param>
        public TicketService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all the tickets from the database.
        /// </summary>
        /// <returns>A list of tickets.</returns>
        public async Task<TicketResult<List<Ticket>>> GetTicketsAsync()
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.GetAsync("ticket");

            if (response.IsSuccessStatusCode)
            {
                var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
                return TicketResult.FromSuccess(tickets!);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return TicketResult.FromError<List<Ticket>>(
                $"API returned {(int)response.StatusCode}: {errorMessage}",
                response.StatusCode
            );
        }

        /// <summary>
        /// Posts a new ticket to the database.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        public async Task<TicketResult<int>> CreateTicketAsync(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("ticket", ticket);

            if (response.IsSuccessStatusCode)
            {
                var ticketId = await response.Content.ReadFromJsonAsync<int>();
                return TicketResult.FromSuccess(ticketId);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return TicketResult.FromError<int>(
                $"API returned {(int)response.StatusCode}: {errorMessage}",
                response.StatusCode
            );
        }

        /// <summary>
        /// Updates a ticket in the database.
        /// </summary>
        /// <param name="ticket">The ticket data to update.</param>
        /// <returns>The result of the update.</returns>
        public async Task<TicketResult<int>> UpdateTicketAsync(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PutAsJsonAsync("ticket", ticket);

            if (response.IsSuccessStatusCode)
            {
                return TicketResult.FromSuccess(0);
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return TicketResult.FromError<int>(
                $"API returned {(int)response.StatusCode}: {errorMessage}",
                response.StatusCode
            );
        }

        /// <summary>
        /// Checks the database to see if corrections exist.
        /// </summary>
        /// <returns>If corrections exist.</returns>
        public async Task<bool> GetCorrectionsExist()
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.GetAsync("corrections");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return bool.Parse(result);
            }

            return false;
        }

        #endregion
    }
}
