using System;
using System.Collections;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using EducationApp.Extensions;
using EducationApp.ViewModels.Utilities;
using JimBobBennett.MvvmLight.AppCompat;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Droid.Activities
{
    public abstract class ActivityBase : AppCompatActivityBase
    {
        protected bool DelayedActivation;
        protected bool DelayedParameter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (!DelayedParameter)
            {
                SetNavigationParameter();
            }
            SetupView();
            if (!DelayedActivation)
            {
                ActivateViewModelAsync().ConfigureAwait(true);
            }
        }

        protected abstract void SetupView();

        protected async Task ActivateViewModelAsync()
        {
            var vma = GetViewModel() as IActivationEnabledViewModel;
            if (vma != null)
            {
                await vma.ActivateAsync().ConfigureAwait(true);
            }
        }

        protected void SetNavigationParameter()
        {
            var vmp = GetViewModel() as IAcceptParameterViewModel;
            vmp?.SetParameter(
                ServiceLocator.Current.GetInstance<AppCompatNavigationService>().GetAndRemoveParameter(Intent));
        }

        protected abstract ViewModelBase GetViewModel();

        internal static ViewStates NullToVisibilityConverter(object arg)
        {
            var visibility = arg == null || (arg is string && ((string) arg).IsNullOrWhiteSpace()) ||
                             (arg is ICollection && ((ICollection) arg).Count == 0)
                ? ViewStates.Visible
                : ViewStates.Gone;
            return visibility;
        }

        internal static ViewStates NotNullToVisibilityConverter(object arg)
            =>
                arg == null || (arg is string && ((string) arg).IsNullOrWhiteSpace()) ||
                (arg is ICollection && ((ICollection) arg).Count == 0)
                    ? ViewStates.Gone
                    : ViewStates.Visible;

        internal static ViewStates NotNullToVisibilityConverter(double? arg)
            => arg.HasValue ? ViewStates.Visible : ViewStates.Gone;

        internal static ViewStates NotNullToVisibilityConverter(decimal? arg)
            => arg.HasValue ? ViewStates.Visible : ViewStates.Gone;

        internal static ViewStates NotNullToVisibilityConverter(DateTime? arg)
            => arg.HasValue ? ViewStates.Visible : ViewStates.Gone;
    }
}