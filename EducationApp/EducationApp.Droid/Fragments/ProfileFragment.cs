using System.ComponentModel;
using Android.OS;
using Android.Views;
using EducationApp.Droid.Activities;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;

namespace EducationApp.Droid.Fragments
{
    public sealed partial class ProfileFragment : FragmentBase
    {
        private MainViewModel Vm => App.Locator.MainViewModel;

        protected override ViewModelBase GetViewModel() => App.Locator.MainViewModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.ProfileFragment, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            Vm.PropertyChanged += VmPropertyChanged;
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vm.Identity))
            {
                TvPleaseLogin.Visibility = ActivityBase.NullToVisibilityConverter(Vm.Identity);
                LProfileData.Visibility = ActivityBase.NotNullToVisibilityConverter(Vm.Identity);
                TvFirstName.Text = Vm.Identity?.FirstName;
                TvLastName.Text = Vm.Identity?.LastName;
                TvEmail.Text = Vm.Identity?.Email;
            }
        }
    }
}