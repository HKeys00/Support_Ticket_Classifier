using System.ComponentModel;

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

        [Description("Billing Inquiry")]
        BillingInquiry = 1,

        [Description("Cancellation Request")]
        CancellationRequest = 2,

        [Description("Product Inquiry")]
        ProductInquiry = 3,

        [Description("Refund Request")]
        RefundRequest = 4,

        [Description("Technical Issue")]
        TechnicalIssue = 5
    }
}
