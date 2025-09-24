using Shared.Attributes;
using Shared.Enums.Ticket;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
                                //Disabling this as this class is only used as an Entity Framework model, but visual studio doesn't seem to be able to detect that across projects.
namespace Shared.Models
{
    /// <summary>
    /// Represents a ticket entity in the database.
    /// </summary>
    [ObjectPrefix(nameof(Ticket))]
    public class Ticket
    {
        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Ticket"/>.
        /// </summary>
        [Key]
        [Required]
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
        [NestedObject]
        [Required]
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the product purchased by the customer.
        /// </summary>
        [Required]
        public string ProductPurchased { get; set; }

        /// <summary>
        /// Gets or sets the date when the product was purchased.
        /// </summary>
        [Required]
        public DateTime DateOfPurchase { get; set; }

        /// <summary>
        /// Gets or sets the type of ticket.
        /// </summary>
        [Required]
        public TicketType Type { get; set; }

        /// <summary>
        /// Gets or sets the subject/topic of the ticket.
        /// </summary>
        [Required, StringLength(200)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the description of the customer's issue or inquiry.
        /// </summary>
        [Required, StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the channel through which the ticket was raised.
        /// </summary>
        [Required]
        public TicketChannel Channel { get; set; }

        /// <summary>
        /// Gets or sets the priority level assigned to the ticket.
        /// </summary>
        public TicketPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets that current status of the ticket.
        /// </summary>
        public TicketStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date time this ticket was resolved (if it has been).
        /// </summary>
        [ColumnName("TimeToResolution")]
        public DateTime? DateResolved { get; set; }

        /// <summary>
        /// Gets or sets the resolution or solution provided for the ticket.
        /// </summary>
        public string? Resolution { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.