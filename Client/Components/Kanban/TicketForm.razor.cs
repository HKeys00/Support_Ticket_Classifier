using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;
using Shared.Helpers;

namespace Client.Components.Kanban
{
    public partial class TicketForm
    {
        #region Fields

        private Shared.Models.Ticket _ticketModel;
        private bool _isNew;

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
        public required Shared.Models.Ticket? Ticket { get; set; }

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
        public required EventCallback<Shared.Models.Ticket?> TicketChanged { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (!IsVisible)
            {
                return;
            }

            _isNew = true;
            _ticketModel = new()
            {
                Customer = new Shared.Models.Customer(),
                DateOfPurchase = DateTime.Today,
                Type = TicketType.BillingInquiry,
                Status = TicketStatus.Open,
            };

            if (Ticket != null)
            {
                _isNew = false;
                TicketHelper.CopyTicket(_ticketModel, Ticket);
            }
        }

        /// <summary>
        /// Submits the ticket.
        /// </summary>
        private async Task Submit()
        {
            if (_isNew)
            {
                await CreateTicket();
            }
            else
            {
                await UpdateTicket();
            }
        }

        /// <summary>
        /// Requests for a new ticket to be created in the database.
        /// </summary>
        private async Task CreateTicket()
        {
            var priority = await ModelService.GetPriorityPrediction(_ticketModel);
            _ticketModel.Priority = priority;

            var response = await TicketService.CreateTicketAsync(_ticketModel);
            if (response != -1)
            {
                StateHasChanged();
                await IsVisibleChanged.InvokeAsync(false);
            }
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
