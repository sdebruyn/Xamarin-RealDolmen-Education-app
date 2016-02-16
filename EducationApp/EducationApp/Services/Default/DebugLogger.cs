using System;
using System.Diagnostics;
using System.Security;

namespace EducationApp.Services.Default
{
    public class DebugLogger : ILoggingService
    {
        /// <exception cref="SecurityException">
        ///     The <see cref="T:System.Security.Permissions.UIPermission" /> is not set to break
        ///     into the debugger.
        /// </exception>
        public void Report(Exception exc, object caller)
        {
            Debug.WriteLine($"{exc.Message} from {caller.GetType().Name}");
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        public void Report(Exception exc, object caller, string extraInfo)
        {
            Debug.WriteLine($"{exc.Message} from {caller.GetType().Name}. Extra info: {extraInfo}");
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}