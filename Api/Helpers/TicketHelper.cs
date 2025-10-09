using Api.Models;
using Shared.Models;

namespace Api.Helpers
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class TicketHelper
    {
        /// <summary>
        /// Converts a ticket to a dto record.
        /// </summary>
        /// <param name="ticket">The ticket data to convert.</param>
        /// <returns>A dto record.</returns>
        public static TicketDto TicketToDto(Ticket ticket)
        {
            return new TicketDto
            {
                ProductPurchased = ticket.ProductPurchased,
                DateOfPurchase = ticket.DateOfPurchase.ToShortDateString(),
                TicketType = ticket.Type,
                TicketSubject = ticket.Subject,
                TicketDescription = ticket.Description,
                TicketChannel = ticket.Channel,
                TicketPriority = ticket.Priority.ToString(),
            };
        }
    }
}
