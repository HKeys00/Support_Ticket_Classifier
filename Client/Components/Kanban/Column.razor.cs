using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

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

        #endregion
    }
}
