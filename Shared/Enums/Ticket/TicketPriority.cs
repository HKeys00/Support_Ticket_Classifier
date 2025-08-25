namespace Shared.Enums.Ticket
{
    /// <summary>
    /// Represents the priority level assigned to the ticket.
    /// Maps to the encoded ticket priority values in the dataset.
    /// </summary>
    public enum TicketPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 4,
    }
}
