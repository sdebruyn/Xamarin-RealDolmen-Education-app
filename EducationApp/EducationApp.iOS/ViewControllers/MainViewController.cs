using System;
using System.Collections.Generic;
using EducationApp.iOS.Utilities;
using EducationApp.Models;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    internal partial class MainViewController : BaseViewController
    {
        private ObservableTableViewController<Category> _categoryListController;
        private ObservableTableViewController<Course> _foundCoursesListController;

        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        public MainViewModel Vm => Application.Locator.MainViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void ViewDidLoad()
        {
            CurrentNavigationItem = NavigationItem;
            base.ViewDidLoad();

            TabBar.ItemSelected += NavigateOnTabBar;
            SearchBar.TextChanged += UpdateSearchValue;

            _categoryListController = Vm.Categories.GetController(CreateDefaultTableCell, BindCategoryCell);
            _categoryListController.TableView = CategoryTable;
            _categoryListController.SelectionChanged += SelectCategory;

            _foundCoursesListController = Vm.FoundCourses.GetController(CreateDefaultTableCell, BindCourseCell);
            _foundCoursesListController.TableView = SearchResultsTable;
            _foundCoursesListController.SelectionChanged += SelectCourse;

            var gestureRecognizer = new UITapGestureRecognizer
            {
                CancelsTouchesInView = false
            };
            gestureRecognizer.AddTarget(() => View.EndEditing(true));
            gestureRecognizer.ShouldReceiveTouch += (rec, t) => !(t.View is UIControl);
            View.AddGestureRecognizer(gestureRecognizer);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TabBar.SelectedItem = CoursesTabBarItem;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            TabBar.ItemSelected -= NavigateOnTabBar;
            SearchBar.TextChanged -= UpdateSearchValue;
            _categoryListController.SelectionChanged -= SelectCategory;
            _foundCoursesListController.SelectionChanged -= SelectCourse;

            _categoryListController.Dispose();
            _categoryListController = null;
            _foundCoursesListController.Dispose();
            _foundCoursesListController = null;

            ReleaseDesignerOutlets();
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.SearchStatus, this,
                () => SearchResultsTable.Hidden, BindingMode.OneWay)
                .ConvertSourceToTarget(status => status != SearchStatus.ResultsAvailable),
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.SearchStatus, this,
                () => CategoryTable.Hidden, BindingMode.OneWay)
                .ConvertSourceToTarget(status => status != SearchStatus.Inactive),
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.SearchStatus, this,
                () => TvSearchFaulted.Hidden, BindingMode.OneWay)
                .ConvertSourceToTarget(status => status != SearchStatus.Faulted),
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.SearchStatus, this,
                () => TvSearching.Hidden, BindingMode.OneWay)
                .ConvertSourceToTarget(status => status != SearchStatus.Searching),
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.SearchStatus, this,
                () => TvNoCoursesFound.Hidden, BindingMode.OneWay)
                .ConvertSourceToTarget(status => status != SearchStatus.NoResults),
            Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.Identity)
                .WhenSourceChanges(IdentityChanged)
        };

        private void UpdateSearchValue(object sender, EventArgs e)
        {
            var sb = sender as UISearchBar;
            Vm.SearchValue = sb?.Text;
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            Vm.ShowDetailsCommand.Execute(_categoryListController.SelectedItem);
        }

        protected override TabBarItemType GetTabBarItemType(UITabBarItem item)
        {
            if (item == CoursesTabBarItem)
            {
                return TabBarItemType.Courses;
            }
            if (item == ProfileTabBarItem)
            {
                return TabBarItemType.Profile;
            }
            return base.GetTabBarItemType(item);
        }

        private void SelectCourse(object sender, EventArgs e)
        {
            Vm.ShowCourseDetailsCommand.Execute(_foundCoursesListController.SelectedItem);
        }
    }
}