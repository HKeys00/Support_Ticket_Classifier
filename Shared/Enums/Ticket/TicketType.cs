namespace Shared.Enums.Ticket
{
    /// <summary>
    /// Represents the type of a ticket.
    /// Used for storing or displaying ticket type related information.
    /// Maps to the encoded ticket type values in the dataset.
    /// </summary>
    public enum TicketType
    {
        BillingInquiry = 0,
        CancellationRequest = 1,
        ProductInquiry = 2,
        RefundRequest = 3,
        TechnicalIssue = 4
    }
}
