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
        private bool _showRetrainModel;

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
        /// Gets or sets the injected <see cref="CurrentTicketService"/> instance.
        /// </summary>
        [Inject]
        public required CurrentTicketService CurrentTicketService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="TicketDragService"/> instance.
        /// </summary>
        [Inject]
        public required TicketDragService TicketDragService { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="ToastService"/> instance.
        /// </summary>
        [Inject]
        public required ToastService ToastService { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Tickets"/> page.
        /// </summary>
        public Tickets()
        {
            _tickets = [];
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            CurrentTicketService.OnTicketChange += OnTicketSelected;
            TicketDragService.TicketDragged += OnTicketDragged;
            _tickets = [];

            var fetch = await TicketService.GetTicketsAsync();
            if (fetch.Success)
            {
                _tickets = fetch.TicketData!;
            } else
            {
                await ToastService.ShowToast(new ToastMessage()
                {
                    Title = "Could not fetch existing tickets!",
                    Message = $"An error occured fetching tickets from the database: {fetch.ErrorMessage}",
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });
            }

            _showRetrainModel = await TicketService.GetCorrectionsExist();
        }

        /// <summary>
        /// Adds a ticket to the database.
        /// </summary>
        /// <param name="ticket">The details of the new ticket.</param>
        private void OnNewTicketAdded(Ticket Ticket)
        {
            _tickets.Add(Ticket);
        }

        /// <summary>
        /// Handles the request to retrain the model.
        /// </summary>
        private async void OnModelRetrainRequested()
        {
            var result = await ModelService.RetrainModel();
            if (!result.Success)
            {
                await ToastService.ShowToast(new ToastMessage()
                {
                    Title = "An error occured!",
                    Message = $"{result.ErrorMessage}.",
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });

                return;
            }

            _showRetrainModel = false;
            await ToastService.ShowToast(new ToastMessage()
            {
                Title = "Submitted corrections to model!",
                Message = string.Empty,
                Level = Enums.ToastLevel.Success,
                DurationMs = 5000,
            });
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
                await ToastService.ShowToast(new ToastMessage()
                {
                    Title = "An error occured!",
                    Message = $"Unable to drag ticket because it doesn't exist.",
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });
                return;
            }

            _selectedTicket.Status = (TicketStatus)e.TicketStatus;

            var response = await TicketService.UpdateTicketAsync(_selectedTicket);
            if (!response.Success)
            {
                await ToastService.ShowToast(new ToastMessage()
                {
                    Title = "An error occured!",
                    Message = response.ErrorMessage!,
                    Level = Enums.ToastLevel.Error,
                    DurationMs = 5000,
                });

                return;
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
