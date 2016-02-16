using System;
using System.Threading.Tasks;
using EducationApp.Exceptions;
using EducationApp.Messaging;
using EducationApp.Models;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Plugin.Connectivity;

namespace EducationApp.Services.Web
{
    public class WebContactService : IContactService
    {
        private readonly IEducationApi _apiService;
        private readonly IDialogService _dialogService;
        private readonly IDispatcherHelper _dispatcherHelper;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalizedStringProvider _loc;
        private readonly ILoggingService _loggingService;

        public WebContactService(IUserInitiatedClient apiService, ILoggingService loggingService,
            IDialogService dialogService, ILocalizedStringProvider loc, IDispatcherHelper dispatcherHelper, IAuthenticationService authenticationService)
        {
            _loggingService = loggingService;
            _dialogService = dialogService;
            _loc = loc;
            _dispatcherHelper = dispatcherHelper;
            _authenticationService = authenticationService;
            _apiService = apiService.Client;
        }

        /// <exception cref="ConnectionException">Could not send feedback because the device is not connected to the web.</exception>
        public async Task SendFeedbackAsync(int sessionId, ScoredFeedback feedback)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                throw new ConnectionException();
            }

            try
            {
                await _apiService.SendFeedbackAsync(sessionId, feedback).ConfigureAwait(false);
                _dispatcherHelper.ExecuteOnUiThread(
                    () =>
                        _dialogService.ShowMessageBox(_loc.GetLocalizedString(Localized.FeedbackSent),
                            _loc.GetLocalizedString(Localized.Success)));
            }
            catch (Exception e)
            {
                await CatchExceptionAsync(e);
            }
            finally
            {
                Messenger.Default.Send(new SubmittingMessage(false));
            }
        }

        /// <exception cref="ConnectionException">Could not send subscription because the device is not connected to the web.</exception>
        public async Task SendSubscriptionAsync(int sessionId, Subscription subscription)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                throw new ConnectionException();
            }

            try
            {
                await _apiService.SendSubscriptionAsync(sessionId, subscription).ConfigureAwait(false);
                _dispatcherHelper.ExecuteOnUiThread(
                    () =>
                        _dialogService.ShowMessageBox(_loc.GetLocalizedString(Localized.SubscriptionSent),
                            _loc.GetLocalizedString(Localized.Success)));
            }
            catch (Exception e)
            {
                await CatchExceptionAsync(e);
            }
            finally
            {
                Messenger.Default.Send(new SubmittingMessage(false));
            }
        }

        /// <exception cref="ConnectionException">Could not send contact form because the device is not connected to the web.</exception>
        public async Task SendContactFormAsync(int sessionId, ContactForm contactForm)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                throw new ConnectionException();
            }

            try
            {
                await _apiService.SendContactFormAsync(sessionId, contactForm).ConfigureAwait(false);
                _dispatcherHelper.ExecuteOnUiThread(
                    () =>
                        _dialogService.ShowMessageBox(_loc.GetLocalizedString(Localized.MessageSent),
                            _loc.GetLocalizedString(Localized.Success)));
            }
            catch (Exception e)
            {
                await CatchExceptionAsync(e);
            }
            finally
            {
                Messenger.Default.Send(new SubmittingMessage(false));
            }
        }

        private async Task CatchExceptionAsync(Exception exc)
        {
            _loggingService.Report(exc, this);
            _dispatcherHelper.ExecuteOnUiThread(async () => await _dialogService.ShowMessageBox(_loc.GetLocalizedString(Localized.ContactError),
                    _loc.GetLocalizedString(Localized.SomethingWrong)).ConfigureAwait(false));
            await _authenticationService.LogoutAsync().ConfigureAwait(false);

        }
    }
}