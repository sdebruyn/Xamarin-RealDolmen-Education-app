using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using RelayCommand = EducationApp.ViewModels.Utilities.RelayCommand;

namespace EducationApp.ViewModels
{
    public class CourseViewModel : ViewModelBase, IActivationEnabledViewModel, IAcceptParameterViewModel
    {
        private readonly ICourseService _courseService;
        private readonly object _lockObject;
        private readonly IPlatformActionService _platformService;

        private string _activeDescription;
        private bool _activeDescriptionIsShort;

        private RelayCommand<LanguageCode> _changeLanguageCommand;
        private Course _course;
        private RelayCommand _openBrowserCommand;
        private RelayCommand _sendInstructorEmailCommand;
        private RelayCommand _switchDescriptionCommand;

        private RelayCommand _updateSessionsCommand;

        private RelayCommand<Session> _viewSessionCommand;

        private RelayCommand _viewSessionsCommand;
        private bool _refreshInNewLanguage;

        public CourseViewModel(ICourseService courseService, IDialogService dialogService, ILocalizedStringProvider loc,
            ILoggingService loggingService, IAuthenticationService authService, IDispatcherHelper dispatcherHelper,
            INavigationService navigationService, IPlatformActionService platformService)
            : base(dialogService, loc, loggingService, authService, dispatcherHelper, navigationService)
        {
            _courseService = courseService;
            _platformService = platformService;
            _lockObject = new object();

            if (IsInDesignMode)
            {
                Course = FakeCourseService.GenerateFakeCourse();
            }

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(IsLoading))
                {
                    ChangeLanguageCommand.RaiseCanExecuteChanged();
                }
            };
        }

        public IList<LanguageCode> Languages
            => Enum.GetValues(typeof (LanguageCode)).Cast<LanguageCode>().ToList();

        /// <summary>
        ///     Gets the SwitchDescriptionCommand.
        /// </summary>
        public RelayCommand SwitchDescriptionCommand
            => _switchDescriptionCommand ?? (_switchDescriptionCommand = new RelayCommand(() =>
            {
                if (SwitchDescriptionCommand.CanExecute(null))
                {
                    _activeDescriptionIsShort = !_activeDescriptionIsShort;
                    ActiveDescription = _activeDescriptionIsShort
                        ? Course?.Description?.ShortDescription
                        : Course?.Description?.LongDescription;
                }
            }, () => Course?.Description != null).DependsOn(() => Course.Description));

        /// <summary>
        ///     Gets the ChangeLanguageCommand.
        /// </summary>
        public RelayCommand<LanguageCode> ChangeLanguageCommand
            => _changeLanguageCommand ?? (_changeLanguageCommand = new RelayCommand<LanguageCode>(async lc =>
            {
                _refreshInNewLanguage = true;
                await _platformService.SetLanguageAsync(lc).ConfigureAwait(true);
                await ActivateAsync().ConfigureAwait(true);
            }, lc => IsLoading == false));

        /// <summary>
        ///     Sets and gets the ActiveDescription property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ActiveDescription
        {
            get { return _activeDescription; }
            set { Set(ref _activeDescription, value); }
        }

        /// <summary>
        ///     Gets the SendInstructorEmailCommand.
        /// </summary>
        public RelayCommand SendInstructorEmailCommand
            => _sendInstructorEmailCommand ?? (_sendInstructorEmailCommand = new RelayCommand((()
                =>
            {
                if (SendInstructorEmailCommand.CanExecute(null))
                {
                    _platformService.SendEmail(Course.Instructor.Email, Course.Title);
                }
            }),
                () =>
                    _platformService.CanSendMail() && Course?.Instructor?.Email != null &&
                    Course.Instructor.Email.IsNotNullOrWhiteSpace()).DependsOn(() => Course.Instructor.Email));

        /// <summary>
        ///     Sets and gets the Course property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Course Course
        {
            get { return _course; }
            set
            {
                Set(ref _course, value);
                _activeDescriptionIsShort = false;
                ActiveDescription = null;
                SwitchDescriptionCommand.Execute(null);
            }
        }

        /// <summary>
        ///     Gets the ViewSessionsCommand.
        /// </summary>
        public RelayCommand ViewSessionsCommand => _viewSessionsCommand ?? (_viewSessionsCommand = new RelayCommand(
            () =>
            {
                if (ViewSessionsCommand.CanExecute(null))
                {
                    NavigationService.NavigateTo(Constants.Pages.SessionsKey);
                }
            }, () => Course != null)).DependsOn(() => Course);

        /// <summary>
        ///     Gets the ViewSessionCommand.
        /// </summary>
        public RelayCommand<Session> ViewSessionCommand
            => _viewSessionCommand ?? (_viewSessionCommand = new RelayCommand<Session>(
                s =>
                {
                    if (ViewSessionCommand.CanExecute(s))
                    {
                        NavigationService.NavigateTo(Constants.Pages.SessionKey, s);
                    }
                }, s => s != null));

        /// <summary>
        ///     Gets the UpdateSessionsCommand.
        /// </summary>
        public RelayCommand UpdateSessionsCommand
            =>
                _updateSessionsCommand ??
                (_updateSessionsCommand =
                    new RelayCommand(async () => await ExecuteUpdateSessionsCommandAsync().ConfigureAwait(true),
                        () => Course != null)).DependsOn(() => Course);

        /// <summary>
        ///     Gets the OpenBrowserCommand.
        /// </summary>
        public RelayCommand OpenBrowserCommand
            =>
                _openBrowserCommand ??
                (_openBrowserCommand =
                    new RelayCommand(
                        async () => await _platformService.OpenBrowserAsync(Course.ExternalUrl).ConfigureAwait(true),
                        () => Course?.ExternalUrl != null && Course.ExternalUrl.IsNotNullOrWhiteSpace()).DependsOn(
                            () => Course.ExternalUrl));

        public void SetParameter(object obj)
        {
            Course = obj as Course;
        }

        public async Task ActivateAsync()
        {
            lock (_lockObject)
            {
                if (IsLoading || Course == null)
                {
                    return;
                }
            }
            DispatcherHelper.ExecuteOnUiThread(() =>
            {
                lock (_lockObject)
                {
                    IsLoading = true;
                }
            });
            try
            {
                if (Course.GetDetailLevel() < 2 || _refreshInNewLanguage)
                {
                    var detailed = await _courseService.GetCourseDetailsAsync(Course.Id).ConfigureAwait(true);
                    DispatcherHelper.ExecuteOnUiThread(() =>
                    {
                        Course = detailed;
#if DEBUG
                        // API doesn't work, so we add fake data
                        if (Course.Instructor == null)
                        {
                            Course.Instructor = new Instructor
                            {
                                Email = "samuel.debruyn@realdolmen.com",
                                Employer = "RealDolmen NV",
                                EmployerDepartment = "Professional Services",
                                FirstName = "Samuel",
                                LastName = "Debruyn",
                                Gender = "m",
                                Id = 1,
                                IsActive = true,
                                Language = "nl"
                            };
                        }
                        if (Course.Description == null)
                        {
                            Course.Descriptions.Add(new Description
                            {
                                Audience = "Padawans",
                                CourseContent = "You will learn the ways of the Force",
                                CourseId = Course.Id,
                                ExternalUrl = "https://education.realdolmen.com",
                                Language = "nl",
                                LongDescription = "This is a longer description.",
                                ShortDescription = "This is a short description.",
                                Materials = "Lightsaber",
                                Methods = "Mind control",
                                Objectives = "You can feel the Force around you",
                                Platforms = "Light Side",
                                Prerequisites = "You'll need some midi-chlorians"
                            });
                        }
                        if (Course.ContentUrl.IsNullOrWhiteSpace())
                        {
                            Course.ContentUrl = "https://education.realdolmen.com";
                        }
                        if (Course.Publisher.IsNullOrWhiteSpace())
                        {
                            Course.Publisher = "RealDolmen NV";
                        }
                        if (Course.StartDate == null)
                        {
                            Course.StartDate = DateTime.Now.AddDays(1);
                        }
#endif
                        SwitchDescriptionCommand.Execute(null);
                    });
                }
            }
            catch (ConnectionException)
            {
                await DialogService.ShowMessageBox(Loc.GetLocalizedString(Localized.NoInternetConnection),
                    Loc.GetLocalizedString(Localized.SomethingWrong)).ConfigureAwait(true);
            }
            catch (DataSourceException exc)
            {
                LoggingService.Report(exc.InnerException ?? exc, this);
                await DialogService.ShowMessageBox(Loc.GetLocalizedString(Localized.ConnectionProblem),
                    Loc.GetLocalizedString(Localized.SomethingWrong)).ConfigureAwait(true);
            }
            finally
            {
                DispatcherHelper.ExecuteOnUiThread(() =>
                {
                    lock (_lockObject)
                    {
                        IsLoading = false;
                    }
                });
                _refreshInNewLanguage = false;
            }
        }

        private async Task ExecuteUpdateSessionsCommandAsync()
        {
            lock (_lockObject)
            {
                if (IsLoading || !UpdateSessionsCommand.CanExecute(null))
                {
                    return;
                }
            }

            lock (_lockObject)
            {
                IsLoading = true;
            }
            try
            {
                await _courseService.UpdateCourseSessionsAsync(Course);
            }
            catch (ConnectionException)
            {
                await DialogService.ShowMessageBox(Loc.GetLocalizedString(Localized.NoInternetConnection),
                    Loc.GetLocalizedString(Localized.SomethingWrong)).ConfigureAwait(true);
            }
            catch (DataSourceException exc)
            {
                LoggingService.Report(exc.InnerException ?? exc, this);
                await DialogService.ShowMessageBox(Loc.GetLocalizedString(Localized.ConnectionProblem),
                    Loc.GetLocalizedString(Localized.SomethingWrong)).ConfigureAwait(true);
            }
            lock (_lockObject)
            {
                IsLoading = false;
            }
        }
    }
}