using System;

namespace EducationApp.Services
{
    public interface ILoggingService
    {
        void Report(Exception exc, object caller);
        void Report(Exception exc, object caller, string extraInfo);
    }
}