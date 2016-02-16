using System;
using EducationApp.Services;

namespace EducationApp.iOS.Services
{
    public class DispatcherHelper : IDispatcherHelper
    {
        public void ExecuteOnUiThread(Action action)
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(action);
        }
    }
}