using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Messaging;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace EducationApp.ViewModels
{
    public class MainViewModel : ViewModelBase, IActivationEnabledViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly ICourseService _courseService;
        private readonly IDispatcherHelper _dispatcher;
        private readonly object _lockObject;

        private Identity _identity;
        private bool _isLoaded;
        private SearchStatus _searchStatus;
        private CancellationTokenSource _searchTaskCancellationSource;
        private string _searchValue;
        private RelayCommand<Course> _showCourseDetailsCommand;
        private RelayCommand<Category> _showDetailsCommand;

        public MainViewModel(ICategoryService categoryService, IDialogService dialogService,
            ILocalizedStringProvider localizedStringProvider, ILoggingService loggingService,
            IAuthenticationService authService, INavigationService navigationService, ICourseService courseService,
            IDispatcherHelper dispatcher)
            : base(dialogService, localizedStringProvider, loggingService, authService, dispatcher, navigationService)
        {
            _categoryService = categoryService;
            _courseService = courseService;
            _dispatcher = dispatcher;

            Categories = new ObservableCollection<Category>();
            _lockObject = new object();
            FoundCourses = new ObservableCollection<Course>();

            Messenger.Default.Register<PropertyChangedMessage<string>>(this,
                message =>
                {
                    if (message.PropertyName == nameof(SearchValue))
                    {
                        UpdateSearchResultsAsync(message.NewValue);
                    }
                });
            Messenger.Default.Register<AuthenticationChangedMessage>(this,
                m => _dispatcher.ExecuteOnUiThread(() => Identity = m.NewIdentity));

            if (IsInDesignModeStatic)
            {
                var identity = new Identity();
                identity.AddClaims(new Claim(Claim.FamilyNameName, Claim.FamilyNameName),
                    new Claim(Claim.EmailName, Claim.EmailName), new Claim(Claim.GivenNameName, Claim.GivenNameName));
                Identity = identity;
            }
        }

        /// <summary>
        ///     Sets and gets the Identity property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Identity Identity
        {
            get { return _identity; }
            set { Set(ref _identity, value); }
        }

        /// <summary>
        ///     Sets and gets the SearchStatus property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public SearchStatus SearchStatus
        {
            get { return _searchStatus; }
            set { Set(ref _searchStatus, value, true); }
        }

        /// <summary>
        ///     Sets and gets the SearchValue property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string SearchValue
        {
            get { return _searchValue; }
            set { Set(ref _searchValue, value, true); }
        }

        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Course> FoundCourses { get; }

        /// <summary>
        ///     Gets the ShowDetailsCommand.
        /// </summary>
        public RelayCommand<Category> ShowDetailsCommand
            => _showDetailsCommand ?? (_showDetailsCommand = new RelayCommand<Category>(category =>
            {
                if (_showDetailsCommand.CanExecute(category))
                {
                    NavigationService.NavigateTo(Constants.Pages.CategoryDetailsKey, category);
                }
            }, category => category != null));

        /// <summary>
        ///     Gets the ShowCourseDetailsCommand.
        /// </summary>
        public RelayCommand<Course> ShowCourseDetailsCommand => _showCourseDetailsCommand
                                                                ??
                                                                (_showCourseDetailsCommand = new RelayCommand<Course>(
                                                                    c =>
                                                                    {
                                                                        if (ShowCourseDetailsCommand.CanExecute(c))
                                                                        {
                                                                            NavigationService.NavigateTo(
                                                                                Constants.Pages.CourseDetailsKey, c);
                                                                        }
                                                                    },
                                                                    c => c != null && c.GetDetailLevel() >= 1));

        public async Task ActivateAsync()
        {
            lock (_lockObject)
            {
                if (IsLoading || _isLoaded)
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
                var categories = await _categoryService.GetAllCategoriesAsync().ConfigureAwait(true);
                Categories.Fill(categories);
                var identity = await AuthService.GetIdentityAsync().ConfigureAwait(true);
                Identity = identity;
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
                        _isLoaded = true;
                    }
                });
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            Messenger.Default.Unregister(this);
        }

        private async Task UpdateSearchResultsAsync(string searchQuery)
        {
            if (SearchStatus == SearchStatus.Searching)
            {
                _searchTaskCancellationSource.Cancel();
            }

            if (SearchValue.IsNullOrWhiteSpace())
            {
                SetSearchResults();
                return;
            }

            SearchStatus = SearchStatus.Searching;
            _searchTaskCancellationSource?.Dispose();
            _searchTaskCancellationSource = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                try
                {
                    var result = await _courseService.SearchCoursesAsync(searchQuery).ConfigureAwait(true);
                    SetSearchResults(result);
                }
                catch (DataSourceException e)
                {
                    LoggingService.Report(e.InnerException ?? e, this);
                    SetSearchResults(null, true);
                }
            }, _searchTaskCancellationSource.Token).ConfigureAwait(true);
        }

        private void SetSearchResults(ICollection<Course> results = null, bool faulted = false)
        {
            _dispatcher.ExecuteOnUiThread(() =>
            {
                FoundCourses.Clear();

                if (faulted)
                {
                    SearchStatus = SearchStatus.Faulted;
                    return;
                }

                if (SearchValue.IsNullOrEmpty())
                {
                    SearchStatus = SearchStatus.Inactive;
                    return;
                }

                if (results == null || !results.Any())
                {
                    SearchStatus = SearchStatus.NoResults;
                    return;
                }

                SearchStatus = SearchStatus.ResultsAvailable;
                results.ForEach(c => FoundCourses.Add(c));
            });
        }
    }
}