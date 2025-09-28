using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    /// <summary>
    /// Data class for tracking corrections to a ticket priority.
    /// This data will be used to retrain the model.
    /// </summary>
    public class Correction
    {
        /// <summary>
        /// Gets or sets the id of the correction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Ticket"/> that has been corrected.
        /// </summary>
        [Required, ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the the <see cref="Ticket"/> that has been corrected.
        /// </summary>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the priority the model predicted.
        /// </summary>
        public int ModelPriority { get; set; }

        /// <summary>
        /// Gets or sets the correct priority.
        /// </summary>
        public int CorrectedPriority { get; set; }

    }
}
