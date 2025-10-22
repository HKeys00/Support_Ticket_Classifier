using System;

namespace Shared.Models
{
    /// <summary>
    /// Data class for capturing a request.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Gets or sets the id of the request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the endpoint url the request was targeted for.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ip address the request is coming from.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date time the request was made.
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
