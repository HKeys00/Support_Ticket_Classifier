using Client.Model;

namespace Client.Services
{
    /// <summary>
    /// Service for handling ticket dragged events.
    /// </summary>
    /// <remarks>Created to avoid having to pass the same method to each column.</remarks>
    public class TicketDragService
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the callback action when a ticket is dragged.
        /// </summary>
        public event Action<TicketDragEventArgs> TicketDragged;

        #endregion

        #region Methods

        /// <summary>
        /// Raises the ticket dragged event.
        /// </summary>
        /// <param name="e">The ticket dragged event arguments.</param>
        public void RaiseTicketDragged(TicketDragEventArgs e)
        {
            TicketDragged?.Invoke(e);
        }

        #endregion
    }
}
