using Client.Model;
using Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Shared.Enums.Ticket;

namespace Client.Components.Kanban
{
    /// <summary>
    /// The column component of the kanban board
    /// </summary>
    public partial class Column
    {
        #region Fields

        private int _ticketCount;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="IJSRuntime"/>
        /// </summary>
        [Inject]
        public required IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets the injected <see cref="TicketDragService"/>
        /// </summary>
        [Inject]
        public required TicketDragService TicketDragService { get; set; }

        /// <summary>
        /// Gets or sets the name of the column to display in the header.
        /// </summary>
        [Parameter]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of tickets for the column.
        /// </summary>
        [Parameter]
        public required List<Shared.Models.Ticket> Tickets { get; set; }

        /// <summary>
        /// Gets or sets the status associated with the column.
        /// </summary>
        [Parameter] 
        public TicketStatus ColumnStatus { get; set; }

        /// <summary>
        /// Gets or sets whether or not tickets can be added to this column.
        /// </summary>
        [Parameter]
        public bool CanAddTickets { get; set; }

        /// <summary>
        /// Gets or sets the event to call when a new ticket is added to the column.
        /// </summary>
        [Parameter]
        public EventCallback AddTicketsEvent { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            _ticketCount = Tickets.Count;
        }

        /// <summary>
        /// Hanldes the on click even for the add ticket button.
        /// </summary>
        private async Task OnAddTicketClicked()
        {
            await AddTicketsEvent.InvokeAsync();
        }
        
        /// <summary>
        /// Handles the event when a ticket is dropped onto a column
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private async Task OnDrop(DragEventArgs e)
        {
            var ticketIdString = await JSRuntime.InvokeAsync<string>("dragDropHelper.getData", e, "text/plain");
            if (int.TryParse(ticketIdString, out var ticketId))
            {
                var args = new TicketDragEventArgs(ticketId, (int)ColumnStatus);
                TicketDragService.RaiseTicketDragged(args);
            }

            //TODO - Throw error
        }

        #endregion
    }
}
