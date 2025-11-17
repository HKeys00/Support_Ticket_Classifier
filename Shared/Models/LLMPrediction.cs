namespace Shared.Models
{
    /// <summary>
    /// Data transfer object for reading prediction value from an llm.
    /// </summary>
    public record LLMPrediction
    {
        /// <summary>
        /// Gets or sets the list of confidence value.
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the value of the prediction.
        /// </summary>
        public required string Value { get; set; }
    }
}
