using Shared.Enums.Ticket;
using Shared.Models;

namespace Api.Models
{
    /// <summary>
    /// Data transfer object for reading the data from the excel spreadsheet.
    /// </summary>
    public record TicketDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Ticket"/>.
        /// </summary>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the product purchased by the customer.
        /// </summary>
        public string ProductPurchased { get; set; }

        /// <summary>
        /// Gets or sets the date when the product was purchased.
        /// </summary>
        public DateTime DateOfPurchase { get; set; }

        /// <summary>
        /// Gets or sets the type of ticket.
        /// </summary>
        public TicketType TicketType { get; set; }

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
        public TicketChannel TicketChannel { get; set; }

        /// <summary>
        /// Gets or sets the priority level assigned to the ticket.
        /// </summary>
        public TicketPriority TicketPriority { get; set; }

        /// <summary>
        /// Gets or sets that current status of the ticket.
        /// </summary>
        public TicketStatus TicketStatus { get; set; }

        /// <summary>
        /// Gets or sets the date time this ticket was resolved (if it has been).
        /// </summary>
        public DateTime? TimeToResolution { get; set; }

        /// <summary>
        /// Gets or sets the resolution or solution provided for the ticket.
        /// </summary>
        public string? Resolution { get; set; }
    }
}
