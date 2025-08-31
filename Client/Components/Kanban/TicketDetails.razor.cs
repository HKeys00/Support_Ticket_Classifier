using Microsoft.AspNetCore.Components;

namespace Client.Components.Kanban
{
    public partial class TicketDetails
    {
        #region Fields

        private bool _isVisible;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ticket data to display details for.
        /// </summary>
        [Parameter]
        public Shared.Models.Ticket Ticket { get; set; }

        #endregion
    }
}
