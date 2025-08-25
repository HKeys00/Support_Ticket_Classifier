namespace Shared.Enums.Ticket
{
    /// <summary>
    /// Represents a the status of a ticket.
    /// Used for storing or displaying ticket status related information.
    /// Maps to the encoded ticket status values in the dataset.
    /// </summary>
    public enum TicketStatus
    {
        Open = 0,
        PendingCustomerResponse = 1,
        Closed = 2,        
    }
}
