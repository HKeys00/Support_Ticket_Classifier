using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Components.Kanban
{
    public partial class Ticket
    {
        #region Fields

        private string _iconPath;

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
        protected override void OnParametersSet()
        {
            _iconPath = $"images/ticket icons/{Data.Type.ToString().ToLower()}.png";
        }

        #endregion
    }
}
