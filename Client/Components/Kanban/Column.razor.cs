using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        /// Gets or sets whether or not tickets can be added to this column.
        /// </summary>
        [Parameter]
        public bool CanAddTickets { get; set; }

        /// <summary>
        /// Gets or sets the event to call when a new ticket is added to the column.
        /// </summary>
        [Parameter]
        public EventCallback<Shared.Models.Ticket> AddTicketsEvent { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            _ticketCount = Tickets.Count;
        }

        /// <summary>
        /// Hanldes the on click even for the add ticket button.
        /// </summary>
        private async Task OnAddTicketClicked()
        {
            Shared.Models.Ticket ticket = new()
            {
                Subject = "New Ticket",
                Description = string.Empty,
                Type = TicketType.None,
                Priority = TicketPriority.Low,
                Status = TicketStatus.Open
            };

            await AddTicketsEvent.InvokeAsync(ticket);
        }

        #endregion
    }
}
