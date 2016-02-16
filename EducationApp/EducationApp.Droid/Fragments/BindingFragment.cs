using System.Collections.Generic;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public abstract class BindingFragment : FragmentBase
    {
        private readonly List<Binding> _bindings;
        protected bool DelayedBinding;

        protected BindingFragment()
        {
            _bindings = new List<Binding>();
        }

        protected virtual IEnumerable<Binding> SetBindings() => new List<Binding>();

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