using System;

namespace EducationApp.Services.Fakes
{
    public class FakeDispatcherHelper : IDispatcherHelper
    {
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public void ExecuteOnUiThread(Action action)
        {
            action.Invoke();
        }
    }
}