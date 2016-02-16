using System.Collections.Generic;
using System.Collections.Specialized;
using Android.App;
using Android.Views;
using Android.Widget;
using AnimatedLoadingViews;
using EducationApp.Droid.Extensions;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace EducationApp.Droid.Activities
{
    [Activity]
    public class SubcategoryActivity : ActivityBase, AdapterView.IOnItemClickListener
    {
        private ListView _courseListView;


        private bool _isLoading;
        private AnimatedCircleLoadingView _loadingView;
        private Toolbar _toolbar;
        private TextView _tvNoCoursesInCategory;

        public Toolbar SubcategoryToolbar
            => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.SubcategoryToolbar));

        public SubcategoryViewModel Vm => App.Locator.SubcategoryViewModel;

        public ListView CourseListView
            => _courseListView ?? (_courseListView = FindViewById<ListView>(Resource.Id.CourseList));

        public TextView TvNoCoursesInCategory
            =>
                _tvNoCoursesInCategory ??
                (_tvNoCoursesInCategory = FindViewById<TextView>(Resource.Id.TvNoCoursesInCategory));

        public AnimatedCircleLoadingView LoadingView
            =>
                _loadingView ??
                (_loadingView = FindViewById<AnimatedCircleLoadingView>(Resource.Id.SubcategoryCircleLoadingView));

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            var course = Vm.Category.Courses[position];
            Vm.ShowCourseDetailsCommand.Execute(course);
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

        private void SetAdapter()
        {
            CourseListView.Adapter =
                Vm.Category.Courses.GetAdapter(
                    (i, course, v) =>
                        this.GetObjectAdapter((c => c.Title), course, v, Resource.Layout.CourseListItem,
                            Resource.Id.CourseListItemName));
            CourseListView.OnItemClickListener = this;

            Vm.Category.Courses.CollectionChanged += SwitchVisibility;
        }

        private void SwitchVisibility(object sender = null, NotifyCollectionChangedEventArgs e = null)
        {
            TvNoCoursesInCategory.Visibility = NullToVisibilityConverter(Vm?.Category?.Courses);
            CourseListView.Visibility = NotNullToVisibilityConverter(Vm?.Category?.Courses);
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.SubcategoryViewModel.SetBinding(() => App.Locator.SubcategoryViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            App.Locator.SubcategoryViewModel.SetBinding(() => App.Locator.SubcategoryViewModel.Category.Courses, this,
                () => TvNoCoursesInCategory.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget(NullToVisibilityConverter),
            App.Locator.SubcategoryViewModel.SetBinding(() => App.Locator.SubcategoryViewModel.Category.Name, this,
                () => SubcategoryToolbar.Title,
                BindingMode.OneWay),
            App.Locator.SubcategoryViewModel.SetBinding(() => App.Locator.SubcategoryViewModel.Category.Courses)
                .WhenSourceChanges(SetAdapter)
        };

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.SubcategoryActivity);
            SetSupportActionBar(SubcategoryToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SwitchVisibility();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
            return true;
        }
    }
}