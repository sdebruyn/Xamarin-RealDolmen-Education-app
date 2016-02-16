using System;

namespace EducationApp.Services
{
    public interface IDispatcherHelper
    {
        void ExecuteOnUiThread(Action action);
    }
}