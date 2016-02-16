using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using EducationApp.Extensions;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using JimBobBennett.MvvmLight.AppCompat;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Droid.Activities
{
    /// <summary>
    ///     Activity required to inherit from.
    /// </summary>
    public abstract class ActivityBase : AppCompatActivityBase
    {
        private readonly List<Binding> _bindings;
        private bool _isUpdatingBindings;
        protected bool DelayedActivation;
        protected bool DelayedBinding;
        protected bool DelayedParameter;

        protected ActivityBase()
        {
            _bindings = new List<Binding>();
        }

        protected virtual IEnumerable<Binding> SetBindings() => new List<Binding>();

        protected abstract void SetupView();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetupView();
            if (!DelayedParameter)
            {
                SetNavigationParameter();
            }
            if (!DelayedBinding)
            {
                ActivateBindings();
            }
            if (!DelayedActivation)
            {
                ActivateViewModelAsync().ConfigureAwait(true);
            }
        }

        protected void ActivateBindings()
        {
            _bindings.AddRange(SetBindings());
        }

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _bindings?.ForEach(binding => binding?.Detach());
        }

        protected override void OnResume()
        {
            base.OnResume();
            ForceBindingUpdates();
        }

        public void ForceBindingUpdates()
        {
            if (_isUpdatingBindings)
            {
                return;
            }
            _isUpdatingBindings = true;
            foreach (var binding in _bindings)
            {
                binding.ForceUpdateValueFromSourceToTarget();
            }
            _isUpdatingBindings = false;
        }

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

        internal static ViewStates NullToVisibilityConverter(double? arg)
            => arg.HasValue ? ViewStates.Gone : ViewStates.Visible;

        internal static ViewStates NotNullToVisibilityConverter(decimal? arg)
            => arg.HasValue ? ViewStates.Visible : ViewStates.Gone;

        internal static ViewStates NullToVisibilityConverter(decimal? arg)
            => arg.HasValue ? ViewStates.Gone : ViewStates.Visible;

        internal static ViewStates NotNullToVisibilityConverter(DateTime? arg)
            => arg.HasValue ? ViewStates.Visible : ViewStates.Gone;

        internal static ViewStates NullToVisibilityConverter(DateTime? arg)
            => arg.HasValue ? ViewStates.Gone : ViewStates.Visible;
    }
}