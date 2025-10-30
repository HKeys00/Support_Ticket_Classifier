using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    /// <summary>
    /// Helper class for api endpoints.
    /// </summary>
    public static class EndpointHelper
    {
        /// <summary>
        /// Combines the path arguments into a readable endpoint.
        /// </summary>
        /// <param name="args">The path arguments</param>
        /// <returns>An endpoint as a string.</returns>
        public static string Combine(params string[] args)
        {
            return $"/{String.Join("/", args)}";
        }
    }
}
