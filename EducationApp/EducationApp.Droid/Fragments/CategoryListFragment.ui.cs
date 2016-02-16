using Android.Widget;
using AnimatedLoadingViews;

namespace EducationApp.Droid.Fragments
{
    public sealed partial class CategoryListFragment
    {
        private ListView _categoryListView;
        private AnimatedCircleLoadingView _loadingView;
        private ListView _searchResultsListView;
        private TextView _tvNoCoursesFound;
        private TextView _tvSearchFaulted;
        private TextView _tvSearching;

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
    }
}