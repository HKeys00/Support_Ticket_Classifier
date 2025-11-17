using Client.Helpers;
using Shared.Enums.Ticket;
using Shared.Models.Result;
using System;

namespace Shared.Models
{
    /// <summary>
    /// Data transfer object for reading prediction value.
    /// </summary>
    public record Prediction
    {
        /// <summary>
        /// Gets or sets the list of confidence % values.
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the value of the prediction.
        /// </summary>
        public TicketPriority Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prediction"/> record.
        /// </summary>
        /// <param name="prediction">The prediction data.</param>
        public Prediction(LLMPrediction prediction)
        {
            Confidence = prediction.Confidence;
            Value = LLMResponseParser.ParsePriority(prediction.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prediction"/> record.
        /// </summary>
        /// <param name="prediction">The prediction data.</param>
        public Prediction(ModelPrediction prediction)
        {
            Confidence = prediction.Confidence[prediction.Value];
            Value = (TicketPriority)prediction.Value;
        }
    }
}
