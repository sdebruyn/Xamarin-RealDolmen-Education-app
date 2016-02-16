using System;

namespace EducationApp.Exceptions
{
    public class DataSourceException : Exception
    {
        public DataSourceException(Exception inner)
            : base("There was a problem retrieving the requested objects.", inner)
        {
        }
    }
}