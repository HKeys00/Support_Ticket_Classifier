namespace Shared.Enums.Ticket
{
    /// <summary>
    /// Represents the type of a ticket.
    /// Used for storing or displaying ticket type related information.
    /// Maps to the encoded ticket type values in the dataset.
    /// </summary>
    public enum TicketType
    {
        None = 0,
        BillingInquiry = 1,
        CancellationRequest = 2,
        ProductInquiry = 3,
        RefundRequest = 4,
        TechnicalIssue = 5
    }
}
