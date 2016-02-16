using System.Threading.Tasks;
using Android.Support.V4.App;
using EducationApp.ViewModels.Utilities;

namespace EducationApp.Droid.Fragments
{
    public abstract class FragmentBase : Fragment
    {
        protected abstract ViewModelBase GetViewModel();

        protected async Task ActivateViewModelAsync()
        {
            var vm = GetViewModel() as IActivationEnabledViewModel;
            if (vm != null)
            {
                await vm.ActivateAsync().ConfigureAwait(true);
            }
        }
    }
}