using System.Net;

namespace Shared.Models.Result
{
    /// <summary>
    /// Data class for storing information regarding the result of a retrain
    /// request api call.
    /// </summary>
    public class RetrainResult
    {
        #region Properties
        
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

        #region Static Methods

        /// <summary>
        /// Creates a new retrain result object when the call was successful.
        /// </summary>
        /// <returns>A successful retrain result.</returns>
        public static RetrainResult FromSuccess() =>
            new() { Success = true };

        /// <summary>
        /// Creates a new retrain result object when the call was successful.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The error status code.</param>
        /// <returns>A failed retrain result.</returns>
        public static RetrainResult FromError(string message, HttpStatusCode? statusCode = null) =>
            new() { Success = false, ErrorMessage = message, StatusCode = statusCode };

        #endregion
    }
}
