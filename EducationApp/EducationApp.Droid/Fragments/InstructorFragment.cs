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
    public class InstructorFragment : BindingFragment
    {
        private bool _isLoading;

        public TextView TvInstructorName => Activity.FindViewById<TextView>(Resource.Id.TvInstructorName);
        public TextView TvNoInstructor => Activity.FindViewById<TextView>(Resource.Id.TvNoInstructor);
        public TextView TvInstructorFirstName => Activity.FindViewById<TextView>(Resource.Id.TvInstructorFirstName);
        public TextView TvInstructorLastName => Activity.FindViewById<TextView>(Resource.Id.TvInstructorLastName);
        public TextView TvEmployer => Activity.FindViewById<TextView>(Resource.Id.TvEmployer);
        public TextView TvEmployerDesc => Activity.FindViewById<TextView>(Resource.Id.TvEmployerDesc);
        public TextView TvEmployerDepartment => Activity.FindViewById<TextView>(Resource.Id.TvEmployerDepartment);
        public TextView TvEmployerSeparator => Activity.FindViewById<TextView>(Resource.Id.TvEmployerSeparator);
        public Button SendInstructorEmailButton => Activity.FindViewById<Button>(Resource.Id.BtSendInstructorMail);

        public AnimatedCircleLoadingView LoadingView
            => Activity.FindViewById<AnimatedCircleLoadingView>(Resource.Id.CourseInstructorLoadingView);

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
            => inflater.Inflate(Resource.Layout.InstructorFragment, container, false);

        protected override IEnumerable<Binding> SetBindings()
        {
            SendInstructorEmailButton.SetCommand("Click", App.Locator.CourseViewModel.SendInstructorEmailCommand);
            return new List<Binding>
            {
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.IsLoading)
                    .WhenSourceChanges(LoadingChanged),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor, this,
                    () => TvNoInstructor.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.FirstName,
                    this,
                    () => TvInstructorFirstName.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.FirstName,
                    this,
                    () => TvInstructorFirstName.Text),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.LastName,
                    this,
                    () => TvInstructorLastName.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.LastName,
                    this,
                    () => TvInstructorLastName.Text),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.Employer,
                    this,
                    () => TvEmployer.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.Employer,
                    this,
                    () => TvEmployer.Text),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Instructor.EmployerDepartment, this,
                    () => TvEmployerDepartment.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Instructor.EmployerDepartment, this,
                    () => TvEmployerDepartment.Text),
                App.Locator.CourseViewModel.SetBinding(
                    () => App.Locator.CourseViewModel.Course.Instructor.EmployerDepartment, this,
                    () => TvEmployerSeparator.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.Employer,
                    this,
                    () => TvEmployerDesc.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter),
                App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Instructor.LastName,
                    this,
                    () => TvInstructorName.Visibility)
                    .ConvertSourceToTarget(ActivityBase.NotNullToVisibilityConverter)
            };
        }
    }
}