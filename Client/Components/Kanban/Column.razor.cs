using Microsoft.AspNetCore.Components;

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
        /// Gets or sets the list of tickets for the column
        /// </summary>
        [Parameter]
        public required List<Shared.Models.Ticket> Tickets { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            _ticketCount = Tickets.Count;
        }

        #endregion
    }
}
