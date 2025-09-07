using Microsoft.AspNetCore.Components;

namespace Client.Components.Kanban
{
    public partial class TicketForm
    {
        #region Properties

        [Parameter]
        public required Shared.Models.Ticket Ticket { get; set; }

        #endregion
    }
}
