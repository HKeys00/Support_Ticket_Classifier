using Microsoft.AspNetCore.Components;

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
        /// Gets or sets the ticket data.
        /// </summary>
        [Parameter]
        public required Shared.Models.Ticket Data { get; set; }

        /// <summary>
        /// Gets or sets the data updated callback.
        /// </summary>
        [Parameter]
        public required EventCallback<Shared.Models.Ticket> DataChanged { get; set; }

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

        #endregion
    }
}
