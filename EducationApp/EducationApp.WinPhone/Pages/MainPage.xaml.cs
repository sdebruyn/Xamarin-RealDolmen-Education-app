using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using EducationApp.Models;
using EducationApp.Services;
using GalaSoft.MvvmLight.Messaging;

namespace EducationApp.WinPhone.Pages
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            Messenger.Default.Register<PropertyChangedMessage<SearchStatus>>(this,
                async message => await UpdateSearchStatusAsync(message.NewValue).ConfigureAwait(true));
        }

        private async Task UpdateSearchStatusAsync(SearchStatus searchStatus)
        {
            var bar = StatusBar.GetForCurrentView();
            switch (searchStatus)
            {
                case SearchStatus.Searching:
                    bar.ProgressIndicator.Text = Loc.GetLocalizedString(Localized.Searching);
                    await bar.ProgressIndicator.ShowAsync();
                    break;
                default:
                    await bar.ProgressIndicator.HideAsync();
                    break;
            }
        }
    }
}