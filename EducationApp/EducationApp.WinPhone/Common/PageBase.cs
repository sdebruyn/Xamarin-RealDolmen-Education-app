using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.Security.Authentication.Web;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using EducationApp.Messaging;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using IdentityModel.Client;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.WinPhone.Common
{
    public class PageBase : Page, IWebAuthenticationContinuable
    {
        private readonly IDialogService _dialogService;
        protected readonly ILocalizedStringProvider Loc;
        protected bool DelayedActivation;
        protected bool DelayedParameter;
        private IDispatcherHelper _helper;

        protected PageBase()
        {
            HardwareButtons.BackPressed -= HardwareButtonsOnBackPressed;
            HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;

            Loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            _helper = ServiceLocator.Current.GetInstance<IDispatcherHelper>();

            var statusBar = StatusBar.GetForCurrentView();
            statusBar.BackgroundOpacity = 0;
            statusBar.ForegroundColor = Color.FromArgb(0, 1, 137, 180);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            statusBar.ShowAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Messenger.Default.Register<PropertyChangedMessage<bool>>(this,
                async message => await UpdateLoadingStatusAsync(message).ConfigureAwait(true));

            Messenger.Default.Register<SubmittingMessage>(this, message => _helper.ExecuteOnUiThread(async () => await UpdateSubmittingStatusAsync(message).ConfigureAwait(true)));

            Unloaded += (sender, args) => Messenger.Default.Unregister(sender);

            _dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
        }

        private async Task UpdateSubmittingStatusAsync(SubmittingMessage message)
        {
            var bar = StatusBar.GetForCurrentView();
            if (message.Submitting)
            {
                bar.ProgressIndicator.Text = Loc.GetLocalizedString(Localized.Submitting);
                await bar.ProgressIndicator.ShowAsync();
            }
            else
            {
                await bar.ProgressIndicator.HideAsync();
            }
        }

        public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            var result = args.WebAuthenticationResult;

            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:

                    var response = new AuthorizeResponse(args.WebAuthenticationResult.ResponseData);

                    var identity = new Identity();
                    identity.AddClaims(new Claim(Claim.AccessTokenName, response.AccessToken));
                    identity.ExtractFromIdentityToken(response.IdentityToken);

                    var authService = ServiceLocator.Current.GetInstance<IAuthenticationService>();
                    await authService.StoreAndUpdateIdentityAsync(identity).ConfigureAwait(false);

                    break;
                default:
                    await
                        _dialogService.ShowError(Loc.GetLocalizedString(Localized.AuthenticationError), null, null, null)
                            .ConfigureAwait(false);
                    return;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!DelayedParameter)
            {
                var vmp = DataContext as IAcceptParameterViewModel;
                if (vmp != null && e.Parameter != null)
                {
                    vmp.SetParameter(e.Parameter);
                }
            }

            if (!DelayedActivation)
            {
                var vma = DataContext as IActivationEnabledViewModel;
                if (vma != null)
                {
                    await vma.ActivateAsync().ConfigureAwait(true);
                }
            }
        }

        private static void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null && frame.CanGoBack)
            {
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.GoBack();
                e.Handled = true;
            }
        }

        private async Task UpdateLoadingStatusAsync(PropertyChangedMessage<bool> message)
        {
            if (message.PropertyName != nameof(ViewModelBase.IsLoading))
            {
                return;
            }

            var bar = StatusBar.GetForCurrentView();
            if (message.NewValue)
            {
                bar.ProgressIndicator.Text = Loc.GetLocalizedString(Localized.Loading);
                await bar.ProgressIndicator.ShowAsync();
            }
            else
            {
                await bar.ProgressIndicator.HideAsync();
            }
        }
    }
}