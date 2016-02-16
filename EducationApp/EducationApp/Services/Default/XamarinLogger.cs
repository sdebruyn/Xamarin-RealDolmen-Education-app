using System;
using System.Collections.Generic;
using Xamarin;

namespace EducationApp.Services.Default
{
    public class XamarinLogger : ILoggingService
    {
        public void Report(Exception exc, object caller)
        {
            Insights.Report(exc, Constants.Logging.LogType, caller.GetType().FullName);
        }

        public void Report(Exception exc, object caller, string extraInfo)
        {
            var extraData = new Dictionary<string, string>
            {
                {Constants.Logging.LogType, caller.GetType().FullName},
                {Constants.Logging.ExtraInfo, extraInfo}
            };
            Insights.Report(exc, extraData);
        }
    }
}