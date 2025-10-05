using System.Net;

namespace Shared.Models.Result
{
    /// <summary>
    /// Data class for storing information regarding the result of a ticket
    /// request api call.
    /// </summary>
    public class TicketResult<T>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the ticket data fetched from the controller.
        /// </summary>
        public T? TicketData { get; set; }

        /// <summary>
        /// Gets or sets whether the prediction was fetched successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets any error messages.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the status code of the request.
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        #endregion
    }

    /// <summary>
    /// Helper class for the generic TicketResult class.
    /// </summary>
    public static class TicketResult
    {
        /// <summary>
        /// Creates a new prediction result object when the prediction was succesfully fetched.
        /// </summary>
        /// <param name="prediction"></param>
        /// <returns>A successful prediction result.</returns>
        public static TicketResult<T> FromSuccess<T>(T TicketData) =>
            new() { TicketData = TicketData, Success = true };

        /// <summary>
        /// Creates a new prediction result object when the prediction resulted in an error.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The error status code.</param>
        /// <returns>A failed prediction result.</returns>
        public static TicketResult<T> FromError<T>(string message, HttpStatusCode? statusCode = null) =>
            new() { Success = false, ErrorMessage = message, StatusCode = statusCode };
    }
}
