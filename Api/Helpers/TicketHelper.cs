using System.ComponentModel;
using System.Reflection;
using Api.Models;
using Shared.Enums.Ticket;
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
        public static TicketPredictionDto TicketToPredictionDto(this Ticket ticket)
        {
            return new TicketPredictionDto
            {
                DateOfPurchase = ticket.DateOfPurchase.ToString("yyyy-mm-dd"),
                TicketType = ticket.Type.GetTicketTypeDisplay(),
                TicketSubject = ticket.Subject,
                TicketDescription = ticket.Description,
                TicketChannel = ticket.Channel.GetTicketChannelDisplay(),
            };
        }

        /// <summary>
        /// Converts a ticket to a correction dto record.
        /// </summary>
        /// <param name="ticket">The ticket data to convert.</param>
        /// <returns>A dto record.</returns>
        public static TicketCorrectionDto TicketToCorrectionDto(this Ticket ticket)
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

        /// <summary>
        /// Gets the description of a ticket type value.
        /// </summary>
        /// <param name="type">The ticket type.</param>
        /// <returns>The description.</returns>
        public static string GetTicketTypeDisplay(this TicketType type)
        {
            return GetDescription(type);
        }

        /// <summary>
        /// Gets the description of a ticket channel value.
        /// </summary>
        /// <param name="channel">The ticket channel.</param>
        /// <returns>The description.</returns>
        public static string GetTicketChannelDisplay(this TicketChannel channel)
        {
            return GetDescription(channel);
        }

        /// <summary>
        /// Gets the description attribute from a value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value with the possible description attribute.</param>
        /// <returns>The description.</returns>
        public static string GetDescription<T>(T value)
        {
            var field = typeof(T).GetField(value.ToString());
            var display = field.IsDefined(typeof(DescriptionAttribute), false);

            if (display)
            {
                DescriptionAttribute? description = (DescriptionAttribute?)field.GetCustomAttribute(typeof(DescriptionAttribute));
                return description?.Description ?? nameof(value);
            }

            return nameof(value);
        }
    }
}
