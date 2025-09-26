namespace Client.Model
{
    /// <summary>
    /// Data class for passing drag event data around.
    /// </summary>
    public struct TicketDragEventArgs
    {
        #region Properties

        public int Id { get; }
        public int TicketStatus { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketDragEvent"/> struct.
        /// </summary>
        /// <param name="id">The id of the ticket that has beend dragged.</param>
        /// <param name="ticketStatus">The new ticket status</param>
        public TicketDragEventArgs(int id, int ticketStatus)
        {
            Id = id;
            TicketStatus = ticketStatus;
        }

        #endregion
    }
}
