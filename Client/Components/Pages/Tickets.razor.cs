using Client.Model;
using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;
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

        private bool _existingTicketDialogShowing;
        private bool _newTicketDialogShowing;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="TicketService"/> instance.
        /// </summary>
        [Inject]
        public required TicketService TicketService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="CurrentTicketService"/> instance.
        /// </summary>
        [Inject]
        public required CurrentTicketService CurrentTicketService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="TicketDragService"/> instance.
        /// </summary>
        [Inject]
        public required TicketDragService TicketDragService { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            CurrentTicketService.OnTicketChange += OnTicketSelected;
            TicketDragService.TicketDragged += OnTicketDragged;
            _tickets = new List<Ticket>();

            _tickets = await TicketService.GetTicketsAsync();
        }

        /// <summary>
        /// Adds a ticket to the database.
        /// </summary>
        /// <param name="ticket">The details of the new ticket.</param>
        private void OnNewTicketAdded()
        {
            _newTicketDialogShowing = true;
        }

        /// <summary>
        /// Handles a ticket dragged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private async void OnTicketDragged(TicketDragEventArgs e)
        {
            _selectedTicket = _tickets.FirstOrDefault(t => t.Id == e.Id);
            if (_selectedTicket == null)
            {
                //TODO Hadnle Error
                return;
            }

            _selectedTicket.Status = (TicketStatus)e.TicketStatus;

            try
            {
                await TicketService.UpdateTicketAsync(_selectedTicket);
            }
            catch (Exception ex)
            {
                //TODO Errors
            }
            
            StateHasChanged();
        }

        /// <summary>
        /// Handles an update to the currently selected ticket.
        /// </summary>
        /// <param name="id">The id of the selected ticket.</param>
        private void OnTicketSelected(int? id)
        {
            _selectedTicket = _tickets.FirstOrDefault(t => t.Id == id);
            _existingTicketDialogShowing = true;
            StateHasChanged();
        }

        #endregion
    }
}
