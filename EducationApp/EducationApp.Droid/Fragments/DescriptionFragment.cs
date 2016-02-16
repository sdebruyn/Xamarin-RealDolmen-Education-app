using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using AnimatedLoadingViews;
using EducationApp.Droid.Activities;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class DescriptionFragment : BindingFragment
    {
        private bool _isLoading;

        public Button OpenBrowserButton => Activity.FindViewById<Button>(Resource.Id.BtOpenBrowser);
        public TextView TvCourseContent => Activity.FindViewById<TextView>(Resource.Id.TvCourseContent);
        public TextView TvCourseContentDesc => Activity.FindViewById<TextView>(Resource.Id.TvCourseContentDesc);
        public TextView TvAudience => Activity.FindViewById<TextView>(Resource.Id.TvAudience);
        public TextView TvAudienceDesc => Activity.FindViewById<TextView>(Resource.Id.TvAudienceDesc);
        public TextView TvMaterials => Activity.FindViewById<TextView>(Resource.Id.TvMaterials);
        public TextView TvMaterialsDesc => Activity.FindViewById<TextView>(Resource.Id.TvMaterialsDesc);
        public TextView TvMethods => Activity.FindViewById<TextView>(Resource.Id.TvMethods);
        public TextView TvMethodsDesc => Activity.FindViewById<TextView>(Resource.Id.TvMethodsDesc);
        public TextView TvPlatforms => Activity.FindViewById<TextView>(Resource.Id.TvPlatforms);
        public TextView TvPlatformsDesc => Activity.FindViewById<TextView>(Resource.Id.TvPlatformsDesc);
        public TextView TvPrerequisites => Activity.FindViewById<TextView>(Resource.Id.TvPrerequisites);
        public TextView TvPrerequisitesDesc => Activity.FindViewById<TextView>(Resource.Id.TvPrerequisitesDesc);
        public TextView TvObjectives => Activity.FindViewById<TextView>(Resource.Id.TvObjectives);
        public TextView TvObjectivesDesc => Activity.FindViewById<TextView>(Resource.Id.TvObjectivesDesc);

        public AnimatedCircleLoadingView LoadingView
            => Activity.FindViewById<AnimatedCircleLoadingView>(Resource.Id.CourseDescriptionLoadingView);

        private CourseViewModel Vm => App.Locator.CourseViewModel;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.DescriptionFragment, container, false);

        protected override IEnumerable<Binding> SetBindings()
        {
            OpenBrowserButton.SetCommand("Click", App.Locator.CourseViewModel.OpenBrowserCommand);

            return new List<Binding>
            {
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.IsLoading)
                    .WhenSourceChanges(LoadingChanged),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Description.CourseContent, this, () => TvCourseContent.Text,
                    BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Description.CourseContent, this,
                    () => TvCourseContent.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Description.CourseContent, this,
                    () => TvCourseContentDesc.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Audience,
                    this, () => TvAudience.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Audience,
                    this, () => TvAudience.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Audience,
                    this, () => TvAudienceDesc.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Materials,
                    this, () => TvMaterials.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Materials,
                    this, () => TvMaterials.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Materials,
                    this, () => TvMaterialsDesc.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Methods,
                    this, () => TvMethods.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Methods,
                    this, () => TvMethods.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Methods,
                    this, () => TvMethodsDesc.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Platforms,
                    this, () => TvPlatforms.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Platforms,
                    this, () => TvPlatforms.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Platforms,
                    this, () => TvPlatformsDesc.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Objectives,
                    this, () => TvObjectives.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Objectives,
                    this, () => TvObjectives.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Description.Objectives,
                    this, () => TvObjectivesDesc.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Description.Prerequisites, this, () => TvPrerequisites.Text,
                    BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Description.Prerequisites, this,
                    () => TvPrerequisites.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Description.Prerequisites, this,
                    () => TvPrerequisitesDesc.Visibility,
                    BindingMode.OneWay).ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter)
            };
        }
    }
}