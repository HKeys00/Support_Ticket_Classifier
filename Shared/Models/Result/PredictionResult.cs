using System.Net;

namespace Shared.Models.Result
{
    /// <summary>
    /// Data class for storing information regarding the result of a prediction
    /// request api call.
    /// </summary>
    public class PredictionResult
    {
        #region Properties
        /// <summary>
        /// Gets or sets the prediction information fetched from the controller.
        /// </summary>
        public Prediction? Prediction { get; set; }

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
        /// Creates a new prediction result object when the prediction was succesfully fetched.
        /// </summary>
        /// <param name="prediction"></param>
        /// <returns>A successful prediction result.</returns>
        public static PredictionResult FromSuccess(Prediction prediction) =>
            new() { Prediction = prediction, Success = true };

        /// <summary>
        /// Creates a new prediction result object when the prediction resulted in an error.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The error status code.</param>
        /// <returns>A failed prediction result.</returns>
        public static PredictionResult FromError(string message, HttpStatusCode? statusCode = null) =>
            new() { Success = false, ErrorMessage = message, StatusCode = statusCode };

        #endregion
    }
}
