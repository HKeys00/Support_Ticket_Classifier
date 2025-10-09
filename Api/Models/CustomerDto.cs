using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    /// <summary>
    /// Data transfer object for reading the customer data from the excel spreadsheet.
    /// </summary>
    public record CustomerDto
    {
        /// <summary>
        /// Gets or sets the customers full name.
        /// </summary>
        public required string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customers email address.
        /// </summary>
        [MaxLength(50)]
        public required string CustomerEmail { get; set; }
    }
}
