using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    /// <summary>
    /// Data transfer object for reading the customer data from the excel spreadsheet.
    /// </summary>
    public class CustomerDto
    {
        /// <summary>
        /// Gets or sets the customers full name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customers email address.
        /// </summary>
        [MaxLength(50)]
        public string CustomerEmail { get; set; }
    }
}
