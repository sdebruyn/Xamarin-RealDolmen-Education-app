using System;
using System.Threading.Tasks;
using EducationApp.Exceptions;
using EducationApp.Messaging;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using RelayCommand = EducationApp.ViewModels.Utilities.RelayCommand;

namespace EducationApp.ViewModels
{
    public class SessionViewModel : ViewModelBase, IActivationEnabledViewModel, IAcceptParameterViewModel
    {
        private readonly IContactService _contactService;

        private RelayCommand _addParticipantCommand;

        private ContactForm _contactForm;
        private Identity _identity;


        private Participant _participantToAdd;
        private RelayCommand<Participant> _removeParticipantCommand;
        private ScoredFeedback _scoredFeedback;
        private RelayCommand _sendContactFormCommand;
        private RelayCommand _sendFeedbackCommand;
        private RelayCommand _sendSubscriptionCommand;


        private Session _session;
        private Subscription _subscription;

        public SessionViewModel(IDialogService dialogService, ILocalizedStringProvider loc,
            ILoggingService loggingService, IAuthenticationService authService, IContactService contactService,
            IDispatcherHelper dispatcher, INavigationService navigationService)
            : base(dialogService, loc, loggingService, authService, dispatcher, navigationService)
        {
            _contactService = contactService;
            _contactForm = new ContactForm();
            _scoredFeedback = new ScoredFeedback();
            _subscription = new Subscription();
            ParticipantToAdd = new Participant();

            if (IsInDesignMode)
            {
                Session = FakeCourseService.GenerateFakeSession(123);
                Identity = new Identity();
                Subscription.Participants.Add(new Participant
                {
                    FirstName = "First",
                    LastName = "Last",
                    Email = "dummy.person@example.com"
                });
            }

            Messenger.Default.Register<AuthenticationChangedMessage>(this,
                m => DispatcherHelper.ExecuteOnUiThread(() => Identity = m.NewIdentity));
        }

        /// <summary>
        ///     Sets and gets the Session property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Session Session
        {
            get { return _session; }
            set { Set(ref _session, value, true); }
        }

        /// <summary>
        ///     Sets and gets the ParticipantToAdd property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Participant ParticipantToAdd
        {
            get { return _participantToAdd; }
            set { Set(ref _participantToAdd, value); }
        }

        /// <summary>
        ///     Gets the SendContactFormCommand.
        /// </summary>
        public RelayCommand SendContactFormCommand => _sendContactFormCommand
                                                      ??
                                                      (_sendContactFormCommand =
                                                          new RelayCommand(
                                                              async () =>
                                                                  await
                                                                      SendContactInfoAsync(ContactType.Form)
                                                                          .ConfigureAwait(true),
                                                              () =>
                                                                  Session != null && Identity != null &&
                                                                  ContactForm.IsValid).DependsOn(() => Session)
                                                              .DependsOn(() => Identity)
                                                              .DependsOn(() => ContactForm.IsValid));

        /// <summary>
        ///     Gets the SendSubscriptionCommand.
        /// </summary>
        public RelayCommand SendSubscriptionCommand
            =>
                _sendSubscriptionCommand ??
                (_sendSubscriptionCommand =
                    new RelayCommand(
                        async () => await SendContactInfoAsync(ContactType.Subscription).ConfigureAwait(true),
                        () => Session != null && Identity != null && Subscription.IsValid)).DependsOn(() => Session)
                    .DependsOn(() => Identity)
                    .DependsOn(() => Subscription.IsValid);

        public RelayCommand AddParticipantCommand
            => _addParticipantCommand ?? (_addParticipantCommand = new RelayCommand(
                () =>
                {
                    if (AddParticipantCommand.CanExecute(null))
                    {
                        Subscription.Participants.Add(ParticipantToAdd);
                        ParticipantToAdd = new Participant();
                    }
                },
                () =>
                    ParticipantToAdd != null && ParticipantToAdd.IsValid &&
                    !Subscription.Participants.Contains(ParticipantToAdd))).DependsOn(() => ParticipantToAdd)
                .DependsOn(() => ParticipantToAdd.IsValid);

        public RelayCommand<Participant> RemoveParticipantCommand
            => _removeParticipantCommand ?? (_removeParticipantCommand = new RelayCommand<Participant>(p =>
            {
                Subscription.Participants.Remove(p);
                ParticipantToAdd = p;
            }, p => p.IsValid && Subscription.Participants.Contains(p)));

        /// <summary>
        ///     Gets the SendFeedbackCommand.
        /// </summary>
        public RelayCommand SendFeedbackCommand
            =>
                _sendFeedbackCommand ??
                (_sendFeedbackCommand =
                    new RelayCommand(async () => await SendContactInfoAsync(ContactType.Feedback).ConfigureAwait(true),
                        () => Session != null && Identity != null && ScoredFeedback != null)).DependsOn(() => Session)
                    .DependsOn(() => Identity)
                    .DependsOn(() => ScoredFeedback);

        /// <summary>
        ///     Sets and gets the ScoredFeedback property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ScoredFeedback ScoredFeedback
        {
            get { return _scoredFeedback; }
            set { Set(ref _scoredFeedback, value); }
        }

        /// <summary>
        ///     Sets and gets the ContactForm property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ContactForm ContactForm
        {
            get { return _contactForm; }
            set { Set(ref _contactForm, value); }
        }

        /// <summary>
        ///     Sets and gets the Subscription property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Subscription Subscription
        {
            get { return _subscription; }
            set { Set(ref _subscription, value); }
        }

        /// <summary>
        ///     Sets and gets the Identity property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Identity Identity
        {
            get { return _identity; }
            set { Set(ref _identity, value, true); }
        }

        public void SetParameter(object obj)
        {
            var session = obj as Session;
            if (session == null)
                return;

            Session = session;
        }

        public async Task ActivateAsync()
        {
            Identity = await AuthService.GetIdentityAsync().ConfigureAwait(true);
        }

        private async Task SendContactInfoAsync(ContactType contactType)
        {
            Messenger.Default.Send(new SubmittingMessage(true));
            try
            {
                switch (contactType)
                {
                    case ContactType.Form:
                        await _contactService.SendContactFormAsync(Session.Id, ContactForm).ConfigureAwait(true);
                        ParticipantToAdd = new Participant();
                        break;
                    case ContactType.Subscription:
                        await _contactService.SendSubscriptionAsync(Session.Id, Subscription).ConfigureAwait(true);
                        break;
                    case ContactType.Feedback:
                        await _contactService.SendFeedbackAsync(Session.Id, ScoredFeedback).ConfigureAwait(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(contactType), contactType, null);
                }
            }
            catch (ConnectionException)
            {
                await
                    DialogService.ShowError(Loc.GetLocalizedString(Localized.ConnectionProblem),
                        Loc.GetLocalizedString(Localized.NoInternetConnection), null, null).ConfigureAwait(true);
            }
            finally
            {
                Messenger.Default.Send(new SubmittingMessage(false));
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            Messenger.Default.Unregister(this);
        }

        private enum ContactType
        {
            Form,
            Subscription,
            Feedback
        }
    }
}