using System.Collections.Generic;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    /// <summary>
    ///     Fragment required to inherit from.
    /// </summary>
    public abstract class FragmentBase : Fragment
    {
        private readonly List<Binding> _bindings;
        private bool _isUpdatingBindings;
        protected bool DelayedActivation;
        protected bool DelayedBinding;

        protected FragmentBase()
        {
            _bindings = new List<Binding>();
        }

        protected abstract ViewModelBase GetViewModel();

        protected virtual IEnumerable<Binding> SetBindings() => new List<Binding>();

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (!DelayedActivation)
            {
                await ActivateViewModelAsync().ConfigureAwait(true);
            }
        }

        protected async Task ActivateViewModelAsync()
        {
            var vm = GetViewModel() as IActivationEnabledViewModel;
            if (vm != null)
            {
                await vm.ActivateAsync().ConfigureAwait(true);
            }
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

        protected void ActivateBindings()
        {
            _bindings.AddRange(SetBindings());
        }

        public override void OnPause()
        {
            base.OnPause();
            DeactivateBindings();
        }

        protected void DeactivateBindings()
        {
            _bindings?.ForEach(binding => binding?.Detach());
            _bindings?.Clear();
        }

        public override void OnResume()
        {
            base.OnResume();
            if (!DelayedBinding)
            {
                ActivateBindings();
            }
        }
    }
}