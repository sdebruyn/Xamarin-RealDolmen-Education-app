using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class ProfileFragment : FragmentBase
    {
        private RelativeLayout _lProfileData;
        private TextView _tvEmail;
        private TextView _tvFirstName;
        private TextView _tvLastName;
        private TextView _tvPleaseLogin;

        public TextView TvFirstName
            => _tvFirstName ?? (_tvFirstName = Activity.FindViewById<TextView>(Resource.Id.TvFirstName));

        public TextView TvLastName
            => _tvLastName ?? (_tvLastName = Activity.FindViewById<TextView>(Resource.Id.TvLastName));

        public TextView TvEmail => _tvEmail ?? (_tvEmail = Activity.FindViewById<TextView>(Resource.Id.TvEmail));

        public TextView TvPleaseLogin
            => _tvPleaseLogin ?? (_tvPleaseLogin = Activity.FindViewById<TextView>(Resource.Id.TvPleaseLogin));

        public RelativeLayout LProfileData
            => _lProfileData ?? (_lProfileData = Activity.FindViewById<RelativeLayout>(Resource.Id.LProfileData));

        protected override ViewModelBase GetViewModel() => App.Locator.MainViewModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.ProfileFragment, container, false);

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.Identity, this,
                () => TvPleaseLogin.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget(id => id == null ? ViewStates.Visible : ViewStates.Invisible),
            App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.Identity, this,
                () => LProfileData.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget(id => id != null ? ViewStates.Visible : ViewStates.Invisible),
            App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.Identity, this, () => TvFirstName.Text,
                BindingMode.OneWay)
                .ConvertSourceToTarget(id => id?.FirstName),
            App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.Identity, this, () => TvLastName.Text,
                BindingMode.OneWay)
                .ConvertSourceToTarget(id => id?.LastName),
            App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.Identity, this, () => TvEmail.Text,
                BindingMode.OneWay)
                .ConvertSourceToTarget(id => id?.Email)
        };
    }
}