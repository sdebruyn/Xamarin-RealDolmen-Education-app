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

namespace EducationApp.ViewModels
{
    public class CategoryViewModel : ViewModelBase, IActivationEnabledViewModel, IAcceptParameterViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly ICourseService _courseService;
        private readonly object _lockObject;
        private Category _category;

        private RelayCommand<Category> _showDetailsCommand;

        public CategoryViewModel(ICategoryService categoryService, ICourseService courseService,
            ILoggingService loggingService,
            IDialogService dialogService, ILocalizedStringProvider loc, IDispatcherHelper dispatcherHelper,
            IAuthenticationService authService, INavigationService navigationService)
            : base(dialogService, loc, loggingService, authService, dispatcherHelper, navigationService)
        {
            _categoryService = categoryService;
            _courseService = courseService;
            _lockObject = new object();

            if (IsInDesignMode)
            {
                Category = FakeCategoryService.GetFakeCategory(1);
            }
        }

        /// <summary>
        ///     Gets the ShowDetailsCommand.
        /// </summary>
        public RelayCommand<Category> ShowDetailsCommand
            => _showDetailsCommand ?? (_showDetailsCommand = new RelayCommand<Category>(
                cat =>
                {
                    if (_showDetailsCommand.CanExecute(cat))
                    {
                        NavigationService.NavigateTo(Constants.Pages.SubcategoryDetailsKey, cat);
                    }
                },
                cat => cat != null));

        /// <summary>
        ///     Sets and gets the Category property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Category Category
        {
            get { return _category; }
            set { Set(ref _category, value); }
        }

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
                var category = await _categoryService.GetCategoryDetailsAsync(Category.Id).ConfigureAwait(true);
                _courseService.FetchCoursesSpeculativelyAsync(category.Subcategories.ToArray()).ConfigureAwait(true);
                lock (_lockObject)
                {
                    DispatcherHelper.ExecuteOnUiThread(() => { Category.Subcategories.Fill(category.Subcategories); });
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