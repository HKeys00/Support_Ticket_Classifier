using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Api.Data
{
    /// <summary>
    /// The Entity Framework database context for the application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collection of all entities in the Ticket database table.
        /// </summary>
        public DbSet<Ticket> Tickets { get; set; }

        /// <summary>
        /// Gets or sets the collection of all entities in the Customers database table.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Gets or sets the collection of all entities in the Corrections database table.
        /// </summary>
        public DbSet<Correction> Corrections { get; set; }

        /// <summary>
        /// Gets or sets the collection of all entities in the Requests database table.
        /// </summary>
        public DbSet<Request> Requests { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        #endregion
    }
}
