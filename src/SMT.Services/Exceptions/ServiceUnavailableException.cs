using System;

namespace SMT.Services.Exceptions
{
    /// <summary>An external dependency (e.g. the AI data assistant) is unavailable or not configured - mapped to HTTP 503.</summary>
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string message) : base(message)
        {

        }
    }
}
