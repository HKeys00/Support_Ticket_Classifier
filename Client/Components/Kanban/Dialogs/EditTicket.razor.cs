using Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Enums.Ticket;
using Shared.Helpers;

namespace Client.Components.Kanban.Dialogs
{
    public partial class EditTicket
    {
        #region Fields

        private Shared.Models.Ticket _ticketModel;
        private bool _isLoading;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="TicketService"/> instance.
        /// </summary>
        [Inject]
        public required TicketService TicketService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="ModelService"/> instance.
        /// </summary>
        [Inject]
        public required ModelService ModelService { get; set; }

        /// <summary>
        /// Gets or sets the ticket model data to be supplied by the form.
        /// </summary>
        [Parameter]
        public required Shared.Models.Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets if the ticket form dialog is currently visible.
        /// </summary>
        [Parameter]
        public required bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the event callback to trigger when visibility changes.
        /// </summary>
        [Parameter]
        public required EventCallback<bool> IsVisibleChanged { get; set; }

        /// <summary>
        /// Gets or sets the event callback to trigger when a ticket has been updated.
        /// </summary>
        [Parameter]
        public required EventCallback<Shared.Models.Ticket> TicketChanged { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (!IsVisible)
            {
                return;
            }

            _isLoading = false;
            _ticketModel = new()
            {
                Customer = new Shared.Models.Customer(),
                DateOfPurchase = DateTime.Today,
                Type = TicketType.BillingInquiry,
                Status = TicketStatus.Open,
            };
            TicketHelper.CopyTicket(_ticketModel, Ticket);
        }

        /// <summary>
        /// Submits the ticket.
        /// </summary>
        private async Task Submit()
        {
            _isLoading = true;
            await UpdateTicket();            
            _isLoading = false;
        }

        /// <summary>
        /// Requests the update of a specific existing ticket.
        /// </summary>
        private async Task UpdateTicket()
        {
            await TicketService.UpdateTicketAsync(_ticketModel);
            TicketHelper.CopyTicket(Ticket, _ticketModel);

            await TicketChanged.InvokeAsync(Ticket);
            await IsVisibleChanged.InvokeAsync(false);
        }

        /// <summary>
        /// Cancels the creation of the ticket.
        /// </summary>
        private void Cancel()
        {
            Ticket = null;
            IsVisibleChanged.InvokeAsync(false);
        }

        #endregion
    }
}
