using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared;

namespace Api.Controllers
{
    /// <summary>
    /// Controller class that handles CRUD operations for support tickets.
    /// </summary>
    [ApiController]
    [Route(ApiEndpoints.Ticket.Endpoint)]
    public class TicketController : ControllerBase
    {
        #region Fields

        private readonly ILogger<TicketController> _logger;
        private readonly ApplicationDbContext _context;

        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="context">The database context.</param>
        public TicketController(ILogger<TicketController> logger, ApplicationDbContext context)
        {
            _logger = logger;
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
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.Customer)
                    .FirstOrDefaultAsync(t => t.Id == id);
                if (ticket == null)
                {
                    _logger.LogWarning($"Ticket with id {id} not found");
                    return NotFound(new { message = $"Ticket with id {id} not found." });
                }

                _logger.LogInformation($"Fetched ticket with id {id}");
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching ticket with id {id}");
                return Problem(
                    detail: $"An unexpected error occurred while fetching a ticket with id {id}: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves all support tickets from the database.
        /// </summary>
        /// <returns>A list of tickets.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Ticket>>> GetAllTickets()
        {
            try
            {
                var tickets = await _context.Tickets
                    .Include(t => t.Customer)
                    .ToListAsync();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tickets");
                return Problem(
                    detail: $"An unexpected error occured while fetching all tickets: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adds a new ticket to the database.
        /// </summary>
        /// <param name="ticket">The ticket details.</param>
        /// <returns>The id of the newly created ticket.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CreateTicket([FromBody] Ticket ticket)
        {
            try
            {
                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();
                return Ok(ticket.Id);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket ticket");
                return Problem(
                    detail: $"An unexpected error occured while creating a ticket: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a ticket in the database.
        /// </summary>
        /// <param name="ticket">The new ticket data.</param>
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
                    return NotFound(new { message = $"Ticket with id {ticket.Id} not found." });
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
            } catch (Exception ex) {
                _logger.LogError(ex, "Error updating ticket.");
                return Problem(
                    detail: $"An unexpected error occurred while updating the ticket: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        /// <summary>
        /// Checks if corrections exist in the database.
        /// </summary>
        [HttpGet(ApiEndpoints.Ticket.Corrections)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> GetCorrectionsExist()
        {
            try
            {
                int count = await _context.Corrections.CountAsync();
                return Ok(count > 0);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching corrections.");
                return Problem(
                    detail: $"An unexpected error occurred while fetching corrections: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            
        }

        #endregion
    }
}
