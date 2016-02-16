using System;
using EducationApp.Services;
using JimBobBennett.MvvmLight.AppCompat;

namespace EducationApp.Droid.Services
{
    public class DispatcherHelper : IDispatcherHelper
    {
        public void ExecuteOnUiThread(Action action)
        {
            AppCompatActivityBase.CurrentActivity.RunOnUiThread(action);
        }
    }
}