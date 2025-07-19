using Shared.Enums.Ticket;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    /// <summary>
    /// Represents a ticket entity in the database.
    /// </summary>
    public class Ticket
    {
        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Ticket"/>.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Customer"/> who raised
        /// this ticket.
        /// </summary>
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer who raised this ticket.
        /// </summary>
        public Customer Customer { get; set; }

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
        public TicketType Type { get; set; }

        /// <summary>
        /// Gets or sets the subject/topic of the ticket.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public string Description { get; set; }

        public TicketChannel Channel { get; set; }
        public TicketPriority Priority { get; set; }

        public DateTime FirstResponeTime { get; set; } 

        public TicketStatus Status { get; set; }

        public DateTime? DateResolved { get; set; }

        public string? Resolution { get; set; }

        public int? CustomerRating { get; set; }

    }
}
