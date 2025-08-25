namespace Shared.Enums.Ticket
{
    /// <summary>
    /// Represents the channel through which the ticket was raised.
    /// Maps to the encoded ticket channel values in the dataset.
    /// </summary>
    public enum TicketChannel
    {
        SocialMedia = 0,
        Chat = 1,
        Email = 2,
        Phone = 3,
    }
}
