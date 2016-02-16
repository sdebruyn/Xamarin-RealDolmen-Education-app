using System;

namespace EducationApp.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException() : base("There was a problem connecting to the given data source.")
        {
        }
    }
}