using System.Collections.Specialized;
using System.ComponentModel;
using Android.App;
using Android.Views;
using Android.Widget;
using EducationApp.Droid.Extensions;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Droid.Activities
{
    [Activity]
    public sealed partial class SubcategoryActivity : ActivityBase, AdapterView.IOnItemClickListener
    {
        private SubcategoryViewModel Vm => App.Locator.SubcategoryViewModel;

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            var course = Vm.Category.Courses[position];
            Vm.ShowCourseDetailsCommand.Execute(course);
        }

        private void LoadingChanged()
        {
            if (Vm.IsLoading)
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

        private void SwitchVisibility(object sender = null, NotifyCollectionChangedEventArgs e = null)
        {
            TvNoCoursesInCategory.Visibility = NullToVisibilityConverter(Vm?.Category?.Courses);
            CourseListView.Visibility = NotNullToVisibilityConverter(Vm?.Category?.Courses);
        }

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.SubcategoryActivity);

            SubcategoryToolbar.Title = Vm.Category.Name;
            SetSupportActionBar(SubcategoryToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            CourseListView.Adapter =
                Vm.Category.Courses.GetAdapter(
                    (i, course, v) =>
                        this.GetObjectAdapter((c => c.Title), course, v, Resource.Layout.CourseListItem,
                            Resource.Id.CourseListItemName));
            CourseListView.OnItemClickListener = this;

            Vm.Category.Courses.CollectionChanged += SwitchVisibility;
            SwitchVisibility();

            Vm.PropertyChanged += VmPropertyChanged;
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vm.IsLoading))
            {
                LoadingChanged();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
            return true;
        }
    }
}