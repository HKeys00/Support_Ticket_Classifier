using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
    /// <summary>
    /// Represents a customer entity in the database.
    /// Contains basic customer information.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Customer"/>.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customers full name.
        /// </summary>
        [MaxLength(50)]        
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the customers email address.
        /// </summary>
        [MaxLength(50)]        
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the customers age.
        /// </summary>
        public int Age { get; set; }
    }
}
