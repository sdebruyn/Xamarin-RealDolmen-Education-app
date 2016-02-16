using System;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Default;
using GalaSoft.MvvmLight.Views;
using UIKit;
using Xamarin.Auth;

namespace EducationApp.iOS.Services
{
    public class AuthenticationService : BaseAuthenticationService
    {
        private readonly IDispatcherHelper _dispatcherHelper;
        private WebRedirectAuthenticator _authenticator;
        private UIViewController _viewController;

        public AuthenticationService(IDialogService dialogService, ILocalizedStringProvider loc,
            IDispatcherHelper dispatcherHelper)
            : base(dialogService, loc)
        {
            _dispatcherHelper = dispatcherHelper;
        }

        public override Uri GetRedirectUri() => new Uri(Configuration.iOSRedirectUri);

        protected override void StartLoginUi(object viewReference)
        {
            _authenticator = new WebRedirectAuthenticator(GetAuthenticationUri(), GetRedirectUri());
            _authenticator.Error += AuthenticatorOnError;
            _authenticator.Completed += AuthenticatorOnCompleted;

            _dispatcherHelper.ExecuteOnUiThread(() =>
            {
                _viewController = (UIViewController) viewReference;
                _viewController.PresentViewController(_authenticator.GetUI(), false, null);
            });
        }

        private async void AuthenticatorOnCompleted(object sender,
            AuthenticatorCompletedEventArgs authenticatorCompletedEventArgs)
        {
            await _viewController.DismissViewControllerAsync(false).ConfigureAwait(true);
            if (authenticatorCompletedEventArgs == null || !authenticatorCompletedEventArgs.IsAuthenticated ||
                authenticatorCompletedEventArgs.Account?.Properties == null ||
                authenticatorCompletedEventArgs.Account.Properties.Count < 2)
            {
                AuthenticatorOnError(sender, authenticatorCompletedEventArgs);
                return;
            }

            var properties = authenticatorCompletedEventArgs.Account.Properties;
            var identity = new Identity();
            identity.AddClaims(new Claim(Claim.AccessTokenName, properties["access_token"]));
            identity.ExtractFromIdentityToken(properties["id_token"]);

            await StoreAndUpdateIdentityAsync(identity).ConfigureAwait(true);
        }
    }
}