using Shared.Enums.Ticket;

namespace Api.Models
{
    /// <summary>
    /// Data transfer object for making a priority prediction.
    /// </summary>
    public record TicketPredictionDto
    {
        /// <summary>
        /// Gets or sets the date when the product was purchased.
        /// </summary>
        public required string DateOfPurchase { get; set; }

        /// <summary>
        /// Gets or sets the type of ticket.
        /// </summary>
        public TicketType TicketType { get; set; }

        /// <summary>
        /// Gets or sets the subject/topic of the ticket.
        /// </summary>
        public required string TicketSubject { get; set; }

        /// <summary>
        /// Gets or sets the description of the customer's issue or inquiry.
        /// </summary>
        public required string TicketDescription { get; set; }

        /// <summary>
        /// Gets or sets the channel through which the ticket was raised.
        /// </summary>
        public TicketChannel TicketChannel { get; set; }
    }
}
