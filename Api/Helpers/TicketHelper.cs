using Api.Models;
using Shared.Models;
using Shared.Helpers;

namespace Api.Helpers
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class TicketHelper
    {
        /// <summary>
        /// Converts a ticket to a prediction dto record.
        /// </summary>
        /// <param name="ticket">The ticket data to convert.</param>
        /// <returns>A dto record.</returns>
        public static TicketPredictionDto TicketToPredictionDto(Ticket ticket)
        {
            return new TicketPredictionDto
            {
                DateOfPurchase = ticket.DateOfPurchase.ToString("yyyy-mm-dd"),
                TicketType = ticket.Type,
                TicketSubject = ticket.Subject,
                TicketDescription = ticket.Description,
                TicketChannel = ticket.Channel,
            };
        }

        /// <summary>
        /// Converts a ticket to a correction dto record.
        /// </summary>
        /// <param name="ticket">The ticket data to convert.</param>
        /// <returns>A dto record.</returns>
        public static TicketCorrectionDto TicketToCorrectionDto(Ticket ticket)
        {
            return new TicketCorrectionDto
            {
                DateOfPurchase = ticket.DateOfPurchase.ToString("yyyy-mm-dd"),
                TicketType = ticket.Type.GetDescription(),
                TicketSubject = ticket.Subject,
                TicketDescription = ticket.Description,
                TicketChannel = ticket.Channel.GetDescription(),
                TicketPriority = ticket.Priority.GetDescription()
            };
        }
    }
}
