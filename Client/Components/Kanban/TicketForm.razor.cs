using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;

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
                Status = TicketStatus.Open,
                Type = TicketType.None
            };

            if (Ticket != null)
            {
                _isNew = false;
                _ticketModel = new Shared.Models.Ticket()
                {
                    Id = Ticket.Id,
                    Customer = Ticket.Customer,
                    ProductPurchased = Ticket.ProductPurchased,
                    DateOfPurchase = Ticket.DateOfPurchase,
                    Type = Ticket.Type,
                    Subject = Ticket.Subject,
                    Description = Ticket.Description,
                    Channel = Ticket.Channel,
                    Priority = Ticket.Priority,
                    Status = Ticket.Status,
                    DateResolved = Ticket.DateResolved,
                    Resolution = Ticket.Resolution
                };
            }
        }

        /// <summary>
        /// Submits the new ticket.
        /// </summary>
        private async void Submit()
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

        private async Task CreateTicket()
        {
            var priority = await ModelService.GetPriorityPrediction(Ticket);
            Ticket.Priority = priority;

            var response = await TicketService.PostTicketAsync(Ticket);
            if (response != -1)
            {
                await IsVisibleChanged.InvokeAsync(true);
            }
        }

        private async Task UpdateTicket()
        {
            var priority = await ModelService.GetPriorityPrediction(Ticket);
            Ticket.Priority = priority;

            var response = await TicketService.PostTicketAsync(Ticket);
            if (response != -1)
            {
                await IsVisibleChanged.InvokeAsync(true);
            }
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
