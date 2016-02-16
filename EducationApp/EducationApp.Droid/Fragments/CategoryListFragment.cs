using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using AnimatedLoadingViews;
using EducationApp.Droid.Extensions;
using EducationApp.Models;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class CategoryListFragment : FragmentBase, AdapterView.IOnItemClickListener
    {
        private ListView _categoryListView;

        private bool _isLoading;
        private AnimatedCircleLoadingView _loadingView;
        private ListView _searchResultsListView;
        private Dictionary<View, SearchStatus> _searchViews;
        private TextView _tvNoCoursesFound;
        private TextView _tvSearchFaulted;
        private TextView _tvSearching;

        private MainViewModel Vm => App.Locator.MainViewModel;

        private AnimatedCircleLoadingView LoadingView
            =>
                _loadingView ??
                (_loadingView = Activity.FindViewById<AnimatedCircleLoadingView>(Resource.Id.CategoryListLoadingView));

        private ListView CategoryListView
            => _categoryListView ?? (_categoryListView = Activity.FindViewById<ListView>(Resource.Id.CategoryList));

        private ListView SearchResultListView
            =>
                _searchResultsListView ??
                (_searchResultsListView = Activity.FindViewById<ListView>(Resource.Id.SearchResultList));

        private TextView TvSearchFaulted
            => _tvSearchFaulted ?? (_tvSearchFaulted = Activity.FindViewById<TextView>(Resource.Id.TvSearchFaulted));

        private TextView TvNoCoursesFound
            => _tvNoCoursesFound ?? (_tvNoCoursesFound = Activity.FindViewById<TextView>(Resource.Id.TvNoCoursesFound));

        private TextView TvSearching
            => _tvSearching ?? (_tvSearching = Activity.FindViewById<TextView>(Resource.Id.TvSearching));

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (parent.Id == CategoryListView.Id)
            {
                var category = Vm.Categories[position];
                Vm.ShowDetailsCommand.Execute(category);
            }
            if (parent.Id == SearchResultListView.Id)
            {
                var course = Vm.FoundCourses[position];
                Vm.ShowCourseDetailsCommand.Execute(course);
            }
        }

        public void LoadingChanged()
        {
            if (Vm.IsLoading == _isLoading)
            {
                return;
            }
            _isLoading = Vm.IsLoading;

            if (_isLoading)
            {
                LoadingView.Visibility = ViewStates.Visible;
                LoadingView.StartIndeterminate();
            }
            else
            {
                LoadingView.StopOk();
                LoadingView.Visibility = ViewStates.Gone;
            }
        }

        protected override ViewModelBase GetViewModel() => Vm;

        protected override IEnumerable<Binding> SetBindings()
        {
            _searchViews = new Dictionary<View, SearchStatus>
            {
                {TvSearching, SearchStatus.Searching},
                {TvSearchFaulted, SearchStatus.Faulted},
                {TvNoCoursesFound, SearchStatus.NoResults},
                {CategoryListView, SearchStatus.Inactive},
                {SearchResultListView, SearchStatus.ResultsAvailable}
            };

            var bindings = new List<Binding>
            {
                App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.IsLoading)
                    .WhenSourceChanges(LoadingChanged),
                App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.SearchStatus)
                    .WhenSourceChanges(SearchStatusUpdate)
            };
            return bindings;
        }

        private void SearchStatusUpdate()
        {
            var status = App.Locator.MainViewModel.SearchStatus;
            foreach (var pair in _searchViews)
            {
                pair.Key.Visibility = pair.Value == status ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            CategoryListView.Adapter =
                Vm.Categories.GetAdapter(
                    (i, cat, v) =>
                        this.GetObjectAdapter((c => c.Name), cat, v, Resource.Layout.CategoryListItem,
                            Resource.Id.CategoryListItemName));

            CategoryListView.OnItemClickListener = this;

            SearchResultListView.Adapter =
                Vm.FoundCourses.GetAdapter(
                    (i, course, v) =>
                        this.GetObjectAdapter((c => c.Title), course, v, Resource.Layout.CourseListItem,
                            Resource.Id.CourseListItemName));

            SearchResultListView.OnItemClickListener = this;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.CategoryListFragment, container, false);
    }
}