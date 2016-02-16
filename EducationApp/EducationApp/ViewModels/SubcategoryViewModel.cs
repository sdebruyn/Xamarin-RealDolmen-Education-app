using System.Threading.Tasks;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace EducationApp.ViewModels
{
    public class SubcategoryViewModel : ViewModelBase, IActivationEnabledViewModel, IAcceptParameterViewModel
    {
        private readonly ICourseService _courseService;
        private readonly object _lockObject;

        private Category _category;
        private RelayCommand<Course> _showCourseDetailsCommand;

        public SubcategoryViewModel(IDialogService dialogService, ILocalizedStringProvider loc,
            ILoggingService loggingService, IAuthenticationService authService, IDispatcherHelper dispatcherHelper,
            ICourseService courseService, INavigationService navigationService)
            : base(dialogService, loc, loggingService, authService, dispatcherHelper, navigationService)
        {
            _courseService = courseService;
            _lockObject = new object();

            if (IsInDesignMode)
            {
                Category = FakeCategoryService.GetFakeCategory(1);
            }
        }

        /// <summary>
        ///     Sets and gets the Category property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Category Category
        {
            get { return _category; }
            set { Set(ref _category, value); }
        }

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

        public void SetParameter(object obj)
        {
            Category = obj as Category;
        }

        public async Task ActivateAsync()
        {
            lock (_lockObject)
            {
                if (IsLoading || Category == null)
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
                var courses = await _courseService.GetCategoryCoursesAsync(Category.Id).ConfigureAwait(true);
                lock (_lockObject)
                {
                    DispatcherHelper.ExecuteOnUiThread(() => { Category.Courses.Fill(courses); });
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
            }
        }
    }
}