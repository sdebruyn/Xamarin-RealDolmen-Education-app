using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

// ReSharper disable ConsiderUsingAsyncSuffix

namespace EducationApp.Services.Fakes
{
    public class FakeDialogService : IDialogService
    {
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
            => Task.Run(() => Debug.WriteLine(message));

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
            => Task.Run(() => Debug.WriteLine(error));

        public Task ShowMessage(string message, string title) => Task.Run(() => Debug.WriteLine(message));

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
            => Task.Run(() => Debug.WriteLine(message));

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText,
            Action<bool> afterHideCallback)
        {
            Debug.WriteLine(message);
            return Task.FromResult(true);
        }

        public Task ShowMessageBox(string message, string title) => Task.Run(() => Debug.WriteLine(message));
    }
}