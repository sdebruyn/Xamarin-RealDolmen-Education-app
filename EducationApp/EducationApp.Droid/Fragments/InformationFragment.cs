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
    public class InformationFragment : BindingFragment
    {
        private bool _isLoading;

        public TextView TvActiveDescription => Activity.FindViewById<TextView>(Resource.Id.TvActiveDescription);
        public TextView TvTapDescription => Activity.FindViewById<TextView>(Resource.Id.TvTapDescription);
        public TextView TvDuration => Activity.FindViewById<TextView>(Resource.Id.TvDuration);
        public TextView TvDurationDesc => Activity.FindViewById<TextView>(Resource.Id.TvDurationDesc);
        public TextView TvPrice => Activity.FindViewById<TextView>(Resource.Id.TvPrice);
        public TextView TvPriceDesc => Activity.FindViewById<TextView>(Resource.Id.TvPriceDesc);
        public TextView TvPublisher => Activity.FindViewById<TextView>(Resource.Id.TvPublisher);
        public TextView TvPublisherDesc => Activity.FindViewById<TextView>(Resource.Id.TvPublisherDesc);
        public TextView TvDays => Activity.FindViewById<TextView>(Resource.Id.TvDays);
        public TextView TvStartDate => Activity.FindViewById<TextView>(Resource.Id.TvStartDate);
        public TextView TvStartDateDesc => Activity.FindViewById<TextView>(Resource.Id.TvStartDateDesc);

        public AnimatedCircleLoadingView LoadingView
            => Activity.FindViewById<AnimatedCircleLoadingView>(Resource.Id.CourseInformationLoadingView);

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
            => inflater.Inflate(Resource.Layout.InformationFragment, container, false);

        protected override IEnumerable<Binding> SetBindings()
        {
            TvTapDescription.SetCommand("Click", App.Locator.CourseViewModel.SwitchDescriptionCommand);
            TvActiveDescription.SetCommand("Click", App.Locator.CourseViewModel.SwitchDescriptionCommand);

            return new List<Binding>
            {
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.IsLoading)
                    .WhenSourceChanges(LoadingChanged),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.ActiveDescription, this,
                    () => TvActiveDescription.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.ActiveDescription, this,
                    () => TvActiveDescription.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.ActiveDescription, this,
                    () => TvTapDescription.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.StartDate, this,
                    () => TvStartDate.Text, BindingMode.OneWay)
                    .ConvertSourceToTarget(d => d?.ToString("d")),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.StartDate, this,
                    () => TvStartDate.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.StartDate, this,
                    () => TvStartDateDesc.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Duration, this,
                    () => TvDuration.Text, BindingMode.OneWay)
                    .ConvertSourceToTarget(d => d?.ToString()),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Duration, this,
                    () => TvDays.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Duration, this,
                    () => TvDuration.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Duration, this,
                    () => TvDurationDesc.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Price, this,
                    () => TvPrice.Text, BindingMode.OneWay)
                    .ConvertSourceToTarget(p => $"{p:F}"),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Price, this,
                    () => TvPrice.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Price, this,
                    () => TvPriceDesc.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Publisher, this,
                    () => TvPublisher.Text, BindingMode.OneWay),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Publisher, this,
                    () => TvPublisher.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Publisher, this,
                    () => TvPublisherDesc.Visibility, BindingMode.OneWay)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter)
            };
        }
    }
}