using Android.Widget;
using AnimatedLoadingViews;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace EducationApp.Droid.Activities
{
    public sealed partial class SubcategoryActivity
    {
        private ListView _courseListView;
        private AnimatedCircleLoadingView _loadingView;
        private Toolbar _toolbar;
        private TextView _tvNoCoursesInCategory;

        private Toolbar SubcategoryToolbar
            => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.SubcategoryToolbar));

        private ListView CourseListView
            => _courseListView ?? (_courseListView = FindViewById<ListView>(Resource.Id.CourseList));

        private TextView TvNoCoursesInCategory
            =>
                _tvNoCoursesInCategory ??
                (_tvNoCoursesInCategory = FindViewById<TextView>(Resource.Id.TvNoCoursesInCategory));

        private AnimatedCircleLoadingView LoadingView
            =>
                _loadingView ??
                (_loadingView = FindViewById<AnimatedCircleLoadingView>(Resource.Id.SubcategoryCircleLoadingView));
    }
}