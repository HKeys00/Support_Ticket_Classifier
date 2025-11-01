namespace Shared
{
    /// <summary>
    /// Data class for containing the api endpoints.
    /// </summary>
    public static class ApiEndpoints
    {
        public const string Client = "Api";

        /// <summary>
        /// The endpoints for the model controller.
        /// </summary>
        public class Model
        {
            public const string Endpoint = "model";
            public const string ModelPrediction = "model-prediction";
            public const string LLMPrediction = "llm-prediction";
            public const string Retrain = "retrain";
        }

        /// <summary>
        /// The endpoints for the ticket controller.
        /// </summary>
        public class Ticket
        {
            public const string Endpoint = "ticket";
            public const string Corrections = "corrections";
        }
    }
}
