using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers
{
    /// <summary>
    /// Controller class that handles CRUD operations for support tickets.
    /// </summary>
    [ApiController]
    [Route("ticket")]
    public class TicketController : ControllerBase
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a specific ticket from the database.
        /// </summary>
        /// <param name="id">The id of the ticket to retrieve.</param>
        /// <returns>A ticket.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Ticket> GetTicket(int id)
        {
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        /// <summary>
        /// Retrieves all support tickets from the database.
        /// </summary>
        /// <returns>A list of tickets.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Ticket>> GetAllTickets()
        {
            var tickets = _context.Tickets.ToList();
            return Ok(tickets);
        }

        #endregion
    }
}
