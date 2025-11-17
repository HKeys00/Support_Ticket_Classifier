namespace Shared.Models
{
    /// <summary>
    /// Data transfer object for reading prediction value from the model.
    /// </summary>
    public record ModelPrediction
    {
        /// <summary>
        /// Gets or sets the list of confidence % values.
        /// </summary>
        public required float[] Confidence { get; set; }

        /// <summary>
        /// Gets or sets the value of the prediction.
        /// </summary>
        public int Value { get; set; }
    }
}
