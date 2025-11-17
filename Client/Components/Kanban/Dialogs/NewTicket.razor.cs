using Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Enums.Ticket;
using Shared.Models;

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
        /// Gets or sets the event callback to trigger when a ticket has been created.
        /// </summary>
        [Parameter]
        public required EventCallback<Shared.Models.Ticket> TicketCreated { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NewTicket"/> component.
        /// </summary>
        public NewTicket()
        {
            _ticketModel = new()
            {
                Customer = new Customer(),
                DateOfPurchase = DateTime.Today,
                Type = TicketType.BillingInquiry,
                Status = TicketStatus.Open,
            };
        }

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
                Customer = new Customer(),
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
            _ticketModel.Priority = TicketPriority.Low;
            var predictionResult = await ModelService.GetPriorityPrediction(_ticketModel);

            if (predictionResult.Success)
            {
                _ticketModel.Priority = predictionResult.Prediction!.Value;
            } 

            var response = await TicketService.CreateTicketAsync(_ticketModel);
            if (!response.Success)
            {
                ToastService.ShowToast(new Model.ToastMessage()
                {
                    Title = "Failed to create ticket!",
                    Message = $"An error occured: {response.ErrorMessage}",
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });

                return;
            }

            if (predictionResult.Success)
            {
                var prediction = predictionResult.Prediction!;
                string confidence = (prediction.Confidence * 100).ToString("F0");
                ToastService.ShowToast(new Model.ToastMessage()
                {
                    Title = "New Ticket Created!",
                    Message = $"This ticket has been assigned a priority of {(TicketPriority)prediction.Value} with a confidence level of {confidence}%",
                    Level = Enums.ToastLevel.Success,
                    DurationMs = 5000,
                });
            } else
            {
                ToastService.ShowToast(new Model.ToastMessage()
                {
                    Title = "New Ticket Created!",
                    Message = "A priority prediction couldn't be made so it has been set to low priority",
                    Level = Enums.ToastLevel.Warning,
                    DurationMs = 5000,
                });
            }
            await TicketCreated.InvokeAsync(_ticketModel);
            await IsVisibleChanged.InvokeAsync(false);

            StateHasChanged();
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
