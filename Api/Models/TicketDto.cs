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
        /// Gets or sets the product purchased by the customer.
        /// </summary>
        public string ProductPurchased { get; set; }

        /// <summary>
        /// Gets or sets the date when the product was purchased.
        /// </summary>
        public string DateOfPurchase { get; set; }

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
        public string TicketPriority { get; set; }
    }
}
