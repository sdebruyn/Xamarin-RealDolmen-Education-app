using System;
using EducationApp.Services;

namespace EducationApp.Exceptions
{
    public class LocalizationException : Exception
    {
        public LocalizationException(Localized identifier) : base($"No translation found for ${identifier}.")
        {
        }
    }
}