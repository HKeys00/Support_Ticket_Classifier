using Shared.Attributes;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
                                //Disabling this as this class is only used as an Entity Framework model, but visual studio doesn't seem to be able to detect that across projects.
namespace Shared.Models
{
    /// <summary>
    /// Represents a customer entity in the database.
    /// Contains basic customer information.
    /// </summary>
    [ObjectPrefix(nameof(Customer))]
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="Customer"/>.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customers full name.
        /// </summary>
        [Required, MaxLength(50)]        
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the customers email address.
        /// </summary>
        [Required, MaxLength(50)]        
        public string Email { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.