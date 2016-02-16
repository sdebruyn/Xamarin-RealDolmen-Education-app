using Android.Support.V7.App;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Default;
using GalaSoft.MvvmLight.Views;
using Xamarin.Auth;

namespace EducationApp.Droid.Services
{
    public class AuthenticationService : BaseAuthenticationService
    {
        private WebRedirectAuthenticator _authenticator;

        public AuthenticationService(IDialogService dialogService, ILocalizedStringProvider loc)
            : base(dialogService, loc)
        {
        }

        protected override void StartLoginUi(object viewReference)
        {
            _authenticator = new WebRedirectAuthenticator(GetAuthenticationUri(), GetRedirectUri());
            _authenticator.Error += AuthenticatorOnError;
            _authenticator.Completed += AuthenticatorOnCompleted;

            var activity = (AppCompatActivity) viewReference;
            var intent = _authenticator.GetUI(activity);
            activity.StartActivity(intent);
        }

        private async void AuthenticatorOnCompleted(object sender,
            AuthenticatorCompletedEventArgs authenticatorCompletedEventArgs)
        {
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

            await StoreAndUpdateIdentityAsync(identity).ConfigureAwait(false);
        }
    }
}