using System;
using System.Collections.Generic;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.iOS.ViewControllers
{
    partial class InstructorViewController : BaseViewController
    {
        public InstructorViewController(IntPtr handle) : base(handle)
        {
            DelayedParameter = true;
        }

        protected override ViewModelBase GetViewModel() => Application.Locator.CourseViewModel;

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Instructor.FirstName, this, () => TvFirstName.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Instructor.LastName, this, () => TvLastName.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Instructor.Employer, this, () => TvEmployer.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Instructor.EmployerDepartment, this,
                () => TvEmployerDepartment.Text),
            Application.Locator.CourseViewModel.SetBinding(() => Application.Locator.CourseViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged)
        };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BtSendInstructorEmail.SetCommand("TouchUpInside",
                Application.Locator.CourseViewModel.SendInstructorEmailCommand);
        }
    }
}