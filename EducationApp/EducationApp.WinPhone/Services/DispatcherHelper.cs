using System;
using EducationApp.Services;

namespace EducationApp.WinPhone.Services
{
    public sealed class DispatcherHelper : IDispatcherHelper
    {
        public void ExecuteOnUiThread(Action action)
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(action);
        }
    }
}