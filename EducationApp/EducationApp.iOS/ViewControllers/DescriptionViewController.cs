using System;
using System.Collections.Generic;
using EducationApp.iOS.Utilities;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.iOS.ViewControllers
{
    partial class DescriptionViewController : BaseViewController
    {
        public DescriptionViewController(IntPtr handle) : base(handle)
        {
            DelayedParameter = true;
        }

        protected override ViewModelBase GetViewModel() => Application.Locator.CourseViewModel;

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.CourseContent, this,
                () => TvCourseContent.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.Audience, this, () => TvAudience.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.Objectives, this,
                () => TvObjectives.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.Prerequisites, this,
                () => TvPrerequisites.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.Methods, this, () => TvMethods.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.Materials, this, () => TvMaterials.Text),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Description.Platforms, this, () => TvPlatforms.Text),
            Application.Locator.CourseViewModel.SetBinding(() => Application.Locator.CourseViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged)
        };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BtOpenBrowser.SetCommand("TouchUpInside", Application.Locator.CourseViewModel.OpenBrowserCommand);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ScrollView.SetUIScrollViewHeight();
        }
    }
}