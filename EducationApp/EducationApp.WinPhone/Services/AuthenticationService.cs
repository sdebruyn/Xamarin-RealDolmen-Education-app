using System;
using Windows.Security.Authentication.Web;
using EducationApp.Services;
using EducationApp.Services.Default;
using GalaSoft.MvvmLight.Views;

namespace EducationApp.WinPhone.Services
{
    public sealed class AuthenticationService : BaseAuthenticationService
    {
        public AuthenticationService(IDialogService dialogService, ILocalizedStringProvider loc)
            : base(dialogService, loc)
        {
        }

        public override Uri GetRedirectUri() => WebAuthenticationBroker.GetCurrentApplicationCallbackUri();

        protected override void StartLoginUi(object viewReference)
        {
            WebAuthenticationBroker.AuthenticateAndContinue(GetAuthenticationUri(), GetRedirectUri());
        }
    }
}