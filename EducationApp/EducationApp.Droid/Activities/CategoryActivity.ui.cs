using Android.Widget;
using AnimatedLoadingViews;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace EducationApp.Droid.Activities
{
    public sealed partial class CategoryActivity
    {
        private Toolbar _categoryToolbar;
        private AnimatedCircleLoadingView _loadingView;
        private ListView _subcategoryListView;

        public ListView SubcategoryListView
            => _subcategoryListView ?? (_subcategoryListView = FindViewById<ListView>(Resource.Id.SubcategoryList));

        public AnimatedCircleLoadingView LoadingView
            =>
                _loadingView ??
                (_loadingView = FindViewById<AnimatedCircleLoadingView>(Resource.Id.CategoryCircleLoadingView));

        public Toolbar CategoryToolbar
            => _categoryToolbar ?? (_categoryToolbar = FindViewById<Toolbar>(Resource.Id.CategoryToolbar));
    }
}