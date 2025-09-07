namespace Client.Services
{
    /// <summary>
    /// Service used to update the current selected ticket.
    /// Avoids the need to pass the same callback to each column and then to each ticket.
    /// </summary>
    public class CurrentTicketService
    {
        #region Fields

        private int? _id;

        #endregion

        #region Properties

        /// <summary>
        /// The event to raised when the id of the current ticket
        /// is updated.
        /// </summary>
        public event Action<int?> OnTicketChange;

        /// <summary>
        /// The id of the current selected ticket.
        /// </summary>
        public int? Id { 
            get => _id; 
            set 
            {
                _id = value;
                OnTicketChange?.Invoke(value);
            } 
        }

        #endregion
    }
}
