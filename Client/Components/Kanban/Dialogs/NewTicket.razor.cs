using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;

namespace Client.Components.Kanban.Dialogs
{
    public partial class NewTicket
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
        /// Gets or sets the injected <see cref="ToastService"/> instance.
        /// </summary>
        [Inject]
        public required ToastService ToastService { get; set; }

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

            _isLoading = false;
            _ticketModel = new()
            {
                Customer = new Shared.Models.Customer(),
                DateOfPurchase = DateTime.Today,
                Type = TicketType.BillingInquiry,
                Status = TicketStatus.Open,
            };
        }

        /// <summary>
        /// Submits the ticket.
        /// </summary>
        private async Task Submit()
        {
            _isLoading = true;
            await CreateTicket();
            _isLoading = false;
        }

        /// <summary>
        /// Requests for a new ticket to be created in the database.
        /// </summary>
        private async Task CreateTicket()
        {
            var prediction = await ModelService.GetPriorityPrediction(_ticketModel);

            if (prediction == null)
            {
                _ticketModel.Priority = TicketPriority.Low;
                //TODO Handle lack of prediction,
            }
            else
            {
                _ticketModel.Priority = (TicketPriority)prediction.Value;
            }

            var response = await TicketService.CreateTicketAsync(_ticketModel);
            if (response != -1)
            {
                await ToastService.ShowToast(new Model.ToastMessage()
                {
                    Title= "New Ticket Created!",
                    Message = $"This ticket has been assigned a priority of {(TicketPriority)prediction.Value} with a confidence level of {prediction.Confidence[prediction.Value]}%",
                    Level = Enums.ToastLevel.Success,
                    DurationMs = 3000,
                });

                await IsVisibleChanged.InvokeAsync(false);
                StateHasChanged();
                return;
            }

            //TODO: Handle Error
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
