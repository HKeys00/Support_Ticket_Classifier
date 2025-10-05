using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;
using Shared.Helpers;
using Shared.Models;

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
        /// Gets or sets the injected <see cref="ToastService"/> instance.
        /// </summary>
        [Inject]
        public required ToastService ToastService { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="EditTicket"/> component.
        /// </summary>
        public EditTicket()
        {
            _ticketModel = new()
            {
                Customer = new Customer(),
                DateOfPurchase = DateTime.Today,
                Type = TicketType.BillingInquiry,
                Status = TicketStatus.Open,
            };
        }

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
                Customer = new Customer(),
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
            var response = await TicketService.UpdateTicketAsync(_ticketModel);
            if (!response.Success)
            {
                await ToastService.ShowToast(new Model.ToastMessage()
                {
                    Title = "Failed to update ticket.",
                    Message = $"{response.ErrorMessage}",
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });
                return;
            }

            TicketHelper.CopyTicket(Ticket, _ticketModel);

            await TicketChanged.InvokeAsync(Ticket);
            await IsVisibleChanged.InvokeAsync(false);

            await ToastService.ShowToast(new Model.ToastMessage()
            {
                Title = "Ticket Updated!",
                Message = "",
                Level = Enums.ToastLevel.Success,
                DurationMs = 5000,
            });
        }

        /// <summary>
        /// Cancels the creation of the ticket.
        /// </summary>
        private void Cancel()
        {
            IsVisibleChanged.InvokeAsync(false);
        }

        #endregion
    }
}
