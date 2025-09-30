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
            Prediction prediction = null;
            try
            {
                prediction = await ModelService.GetPriorityPrediction(_ticketModel); 
                _ticketModel.Priority = (TicketPriority)prediction.Value;
            }
            catch
            {
                _ticketModel.Priority = TicketPriority.Low;
            }

            try
            {
                var response = await TicketService.CreateTicketAsync(_ticketModel); 
                await IsVisibleChanged.InvokeAsync(false);
                
                if (prediction == null)
                {
                    await ToastService.ShowToast(new Model.ToastMessage()
                    {
                        Title = "New Ticket Created!",
                        Message = "A priority prediction couldn't be made so it has been set to low priority",
                        Level = Enums.ToastLevel.Warning,
                        DurationMs = 5000,
                    });
                }
                else
                {
                    string confidence = (prediction.Confidence[prediction.Value] * 100).ToString("F0");
                    await ToastService.ShowToast(new Model.ToastMessage()
                    {
                        Title = "New Ticket Created!",
                        Message = $"This ticket has been assigned a priority of {(TicketPriority)prediction.Value} with a confidence level of {confidence}%",
                        Level = Enums.ToastLevel.Success,
                        DurationMs = 5000,
                    });
                }
            }
            catch (Exception ex)
            {
                await ToastService.ShowToast(new Model.ToastMessage()
                {
                    Title = "Failed to create ticket!",
                    Message = $"An error occured: {ex.Message}",
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });

                await IsVisibleChanged.InvokeAsync(false);
            }

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
