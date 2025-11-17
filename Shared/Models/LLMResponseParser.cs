using Shared.Enums.Ticket;

namespace Client.Helpers
{
    /// <summary>
    /// Helper class to parse responses from LLM's
    /// </summary>
    public static class LLMResponseParser
    {
        /// <summary>
        /// Parses the ticket priority from a LLM response string.
        /// </summary>
        /// <param name="response">The response string.</param>
        /// <returns>The ticket priority.</returns>
        public static TicketPriority ParsePriority(this string response)
        {
            if (response.Contains(TicketPriority.Medium.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return TicketPriority.Medium;
            }
            else if (response.Contains(TicketPriority.High.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return TicketPriority.High;
            }
            else if (response.Contains(TicketPriority.Critical.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return TicketPriority.Critical;
            }

            return TicketPriority.Low;
        }
    }
}
