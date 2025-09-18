using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Services
{
    /// <summary>
    /// Service that communicates with the ticket api.
    /// </summary>
    public class TicketService
    {
        #region Fields

        IHttpClientFactory _clientFactory;

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
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            var tickets = new List<Ticket>();

            using var client = _clientFactory.CreateClient("Api");

            var response = await client.GetFromJsonAsync<List<Ticket>>("ticket");
            if (response != null)
            {
                tickets = response;
            }

            return tickets;
        }

        /// <summary>
        /// Posts a new ticket to the database.
        /// </summary>
        /// <param name="ticket">The ticket data.</param>
        public async Task<int> PostTicketAsync(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync<Ticket>("ticket", ticket);

            if (!response.IsSuccessStatusCode)
            {
                return 1; 
            }

            var ticketId = await response.Content.ReadFromJsonAsync<int>();
            return ticketId;
        }

        public async Task PutTicketAsync(Ticket ticket)
        {
            using var client = _clientFactory.CreateClient("Api");
            var response = await client.P


        }

        #endregion
    }
}
