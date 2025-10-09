using Shared.Enums.Ticket;

namespace Api.Models
{
    /// <summary>
    /// Data transfer object for retraining the model.
    /// </summary>
    public record TicketCorrectionDto
    {
        /// <summary>
        /// Gets or sets the date when the product was purchased.
        /// </summary>
        public string DateOfPurchase { get; set; }

        /// <summary>
        /// Gets or sets the type of ticket.
        /// </summary>
        public string TicketType { get; set; }

        /// <summary>
        /// Gets or sets the subject/topic of the ticket.
        /// </summary>
        public string TicketSubject { get; set; }

        /// <summary>
        /// Gets or sets the description of the customer's issue or inquiry.
        /// </summary>
        public string TicketDescription { get; set; }

        /// <summary>
        /// Gets or sets the channel through which the ticket was raised.
        /// </summary>
        public string TicketChannel { get; set; }

        /// <summary>
        /// Gets or sets the priority level assigned to the ticket.
        /// </summary>
        public string TicketPriority { get; set; }
    }
}
