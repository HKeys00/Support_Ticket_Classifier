using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Models;
namespace Client.Components.Pages
{
    /// <summary>
    /// The ticket kanban board page.
    /// </summary>
    public partial class Tickets
    {
        #region Fields

        private List<Ticket> _tickets;
        private Ticket? _selectedTicket;

        private bool _newTicketDialogShowing;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="CurrentTicketService"/> instance.
        /// </summary>
        [Inject]
        public required CurrentTicketService CurrentTicketService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="TicketService"/> instance.
        /// </summary>
        [Inject]
        public required TicketService TicketService { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            CurrentTicketService.OnTicketChange += OnTicketSelected;
            _tickets = new List<Ticket>();

            _tickets = await TicketService.GetTicketsAsync();
        }

        /// <summary>
        /// Adds a ticket to the database.
        /// </summary>
        /// <param name="ticket">The details of the new ticket.</param>
        private async Task OnNewTicketAdded(Ticket ticket)
        {
            _newTicketDialogShowing = true;
            //using var client = ClientFactory.CreateClient("Api");

            //var response = await client.PostAsJsonAsync("ticket", ticket);
            //if (response != null)
            //{
            //    ticket.Id = 0;
            //}
        }

        /// <summary>
        /// Handles an update to the currently selected ticket.
        /// </summary>
        /// <param name="id">The id of the selected ticket.</param>
        private void OnTicketSelected(int? id)
        {
            _selectedTicket = _tickets.FirstOrDefault(t => t.Id == id);
            StateHasChanged();
        }

        #endregion
    }
}
