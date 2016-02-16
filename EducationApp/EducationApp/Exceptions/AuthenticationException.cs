using System;
using System.Net;

namespace EducationApp.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(HttpStatusCode statusCode, string reasonPhrase)
            : base($"Authentication failed with status code ${statusCode} and reason ${reasonPhrase}")
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public string ReasonPhrase { get; }
        public HttpStatusCode StatusCode { get; }
    }
}