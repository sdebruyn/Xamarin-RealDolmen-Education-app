using System;
using System.Collections.Generic;
using EducationApp.Extensions;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.iOS.ViewControllers
{
    partial class InformationViewController : BaseViewController
    {
        public InformationViewController(IntPtr handle) : base(handle)
        {
            DelayedParameter = true;
        }

        protected override ViewModelBase GetViewModel() => Application.Locator.CourseViewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BtTapDescription.SetCommand("TouchUpInside", Application.Locator.CourseViewModel.SwitchDescriptionCommand);
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.CourseViewModel.SetBinding(() => Application.Locator.CourseViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.ActiveDescription, this, () => TvDescription.Text,
                BindingMode.OneWay),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.ActiveDescription, this, () => TvDescription.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(s => s.IsNullOrWhiteSpace()),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.StartDate, this, () => TvStartDate.Text,
                BindingMode.OneWay).ConvertSourceToTarget(s => s?.ToString("d") ?? ""),
            Application.Locator.CourseViewModel.SetBinding(() => Application.Locator.CourseViewModel.Course.Price,
                this, () => TvPrice.Text, BindingMode.OneWay).ConvertSourceToTarget(s => $"{s:F}"),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Duration, this, () => TvDuration.Text,
                BindingMode.OneWay).ConvertSourceToTarget(d => d.ToString()),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Publisher, this, () => TvPublisher.Text,
                BindingMode.OneWay),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.StartDate, this, () => TvStartDate.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(d => d == null),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.StartDate, this, () => TvStartDateDesc.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(d => d == null),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Publisher, this, () => TvPublisher.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(StringExtensions.IsNullOrWhiteSpace),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Publisher, this, () => TvPublisherDesc.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(StringExtensions.IsNullOrWhiteSpace),
            Application.Locator.CourseViewModel.SetBinding(() => Application.Locator.CourseViewModel.Course.Price,
                this, () => TvPrice.Hidden, BindingMode.OneWay).ConvertSourceToTarget(p => p == null),
            Application.Locator.CourseViewModel.SetBinding(() => Application.Locator.CourseViewModel.Course.Price,
                this, () => TvPriceDesc.Hidden, BindingMode.OneWay).ConvertSourceToTarget(p => p == null),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Duration, this, () => TvDuration.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(d => d == null),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Duration, this, () => TvDurationDesc1.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(d => d == null),
            Application.Locator.CourseViewModel.SetBinding(
                () => Application.Locator.CourseViewModel.Course.Duration, this, () => TvDurationDesc2.Hidden,
                BindingMode.OneWay).ConvertSourceToTarget(d => d == null)
        };
    }
}