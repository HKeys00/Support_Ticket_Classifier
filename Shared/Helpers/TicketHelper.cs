using Shared.Models;

namespace Shared.Helpers
{
    /// <summary>
    /// Collection of helper methods that involve tickets.
    /// </summary>
    public static class TicketHelper
    {
        /// <summary>
        /// Perform a deep copy of a ticket.
        /// </summary>
        /// <param name="to">The ticket to copy to.</param>
        /// <param name="from">The ticket to copy from.</param>
        public static void CopyTicket(Ticket to,  Ticket from)
        {
            to.Id = from.Id;
            to.Customer = from.Customer;
            to.ProductPurchased = from.ProductPurchased;
            to.DateOfPurchase = from.DateOfPurchase;
            to.Type = from.Type;
            to.Subject = from.Subject;
            to.Description = from.Description;
            to.Channel = from.Channel;
            to.Priority = from.Priority;
            to.Status = from.Status;
            to.DateResolved = from.DateResolved;
            to.Resolution = from.Resolution;
        }

    }
}
