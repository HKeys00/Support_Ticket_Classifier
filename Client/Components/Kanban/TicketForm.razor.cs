using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;

namespace Client.Components.Kanban
{
    public partial class TicketForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ticket model data to be supplied by the form.
        /// </summary>
        [SupplyParameterFromForm]
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

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (IsVisible)
            {
                Ticket = new()
                {
                    Customer = new Shared.Models.Customer(),
                    DateOfPurchase = DateTime.Today,
                    Status = TicketStatus.Open,
                    Type = TicketType.None
                };
            }
        }

        /// <summary>
        /// Submits the new ticket.
        /// </summary>
        private void Submit()
        {

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
