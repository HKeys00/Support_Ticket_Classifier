using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Threading.Channels;

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
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(t => t.Id == id);
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
        public async Task<ActionResult<List<Ticket>>> GetAllTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Customer)
                .ToListAsync();

            return Ok(tickets);
        }

        /// <summary>
        /// Adds a new ticket to the database.
        /// </summary>
        /// <param name="ticket">The ticket details.</param>
        /// <returns>The id of the newly created ticket.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateTicket([FromBody] Ticket ticket)
        {
            try
            {
                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();
                return Ok(ticket.Id);
            } catch
            {
                return BadRequest(-1);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTicket([FromBody] Ticket ticket)
        {
            try
            {
                var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticket.Id);
                if (existingTicket == null)
                {
                    return BadRequest();
                }


                existingTicket.Status = ticket.Status;
                existingTicket.ProductPurchased = ticket.ProductPurchased;
                existingTicket.DateOfPurchase = ticket.DateOfPurchase;
                existingTicket.Type = ticket.Type;
                existingTicket.Subject = ticket.Subject;
                existingTicket.Description = ticket.Description;
                existingTicket.Channel = ticket.Channel;
                existingTicket.DateResolved = ticket.DateResolved;
                existingTicket.Resolution = ticket.Resolution;


                if (existingTicket.Priority != ticket.Priority)
                {
                    var existingCorrection = await _context.Corrections.FirstOrDefaultAsync(c => c.TicketId == ticket.Id);
                    if (existingCorrection != null)
                    {
                        existingCorrection.CorrectedPriority = (int)ticket.Priority;
                    } else
                    {
                        await _context.Corrections.AddAsync(new Correction()
                        {
                            TicketId = ticket.Id,
                            ModelPriority = (int)existingTicket.Priority,
                            CorrectedPriority = (int)ticket.Priority
                        });
                    }
                }

                existingTicket.Priority = ticket.Priority;
                await _context.SaveChangesAsync();
            return Ok();
            } catch {
                //TODO Hanndle API ERROR
                return BadRequest();
            }
        }
        #endregion
    }
}
