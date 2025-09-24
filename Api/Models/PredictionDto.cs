namespace Api.Models
{
    /// <summary>
    /// Data transfer object for reading prediction value from the model.
    /// </summary>
    public record PredictionDto
    {
        /// <summary>
        /// Gets or sets the prediction.
        /// </summary>
        public int Prediction { get; set; }
    }
}
