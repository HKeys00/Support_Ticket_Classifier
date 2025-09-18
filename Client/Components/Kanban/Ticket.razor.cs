using Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Client.Components.Kanban
{
    public partial class Ticket
    {
        #region Fields

        private string _ticketTypeIconPath;
        private string _ticketPriorityIconPath;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="CurrentTicketService"/> instance.
        /// </summary>
        [Inject]
        public required CurrentTicketService CurrentTicketService { get; set; }

        /// <summary>
        /// Gets or sets the ticket data.
        /// </summary>
        [Parameter]
        public required Shared.Models.Ticket Data { get; set; }

        /// <summary>
        /// Gets or sets the data updated callback.
        /// </summary>
        [Parameter]
        public required EventCallback<Shared.Models.Ticket> DataChanged { get; set; }

        /// <summary>
        /// Gets or sets the event callback to trigger when a ticket drag starts.
        /// </summary>
        [Parameter]
        public EventCallback<Shared.Models.Ticket> OnDragStartTicket { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            _ticketPriorityIconPath = string.Empty;
            _ticketTypeIconPath = string.Empty;
        }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            _ticketTypeIconPath = $"images/ticket icons/{Data.Type}.png";
            _ticketPriorityIconPath = $"images/ticket priority/{Data.Priority}.png";
        }

        /// <summary>
        /// Event handler for when the ticket is clicked.
        /// </summary>
        public void OnTicketClicked()
        {
            CurrentTicketService.Id = Data.Id;
        }

        /// <summary>
        /// Event handler for when the ticket drag starts.
        /// </summary>
        /// <param name="e">Drage event params</param>
        private async Task OnDragStart(DragEventArgs e)
        {
            await OnDragStartTicket.InvokeAsync(Data);
        }

        #endregion
    }
}
