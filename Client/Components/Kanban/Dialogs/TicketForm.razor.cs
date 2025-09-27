using Microsoft.AspNetCore.Components;
namespace Client.Components.Kanban.Dialogs
{
    public partial class TicketForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the ticket form.
        /// </summary>
        [Parameter]
        public required string FormName { get; set; }

        /// <summary>
        /// Gets or sets the callback for the valid submit.
        /// </summary>
        [Parameter]
        public required EventCallback OnValidSubmit { get; set; }

        /// <summary>
        /// Gets or sets the ticket model data to be supplied by the form.
        /// </summary>
        [Parameter]
        public required Shared.Models.Ticket Model { get; set; }

        #endregion
    }
}
