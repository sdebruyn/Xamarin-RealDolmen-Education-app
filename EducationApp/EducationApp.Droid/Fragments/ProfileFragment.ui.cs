using Android.Widget;

namespace EducationApp.Droid.Fragments
{
    public sealed partial class ProfileFragment
    {
        private RelativeLayout _lProfileData;
        private TextView _tvEmail;
        private TextView _tvFirstName;
        private TextView _tvLastName;
        private TextView _tvPleaseLogin;

        private TextView TvFirstName
            => _tvFirstName ?? (_tvFirstName = Activity.FindViewById<TextView>(Resource.Id.TvFirstName));

        private TextView TvLastName
            => _tvLastName ?? (_tvLastName = Activity.FindViewById<TextView>(Resource.Id.TvLastName));

        private TextView TvEmail => _tvEmail ?? (_tvEmail = Activity.FindViewById<TextView>(Resource.Id.TvEmail));

        private TextView TvPleaseLogin
            => _tvPleaseLogin ?? (_tvPleaseLogin = Activity.FindViewById<TextView>(Resource.Id.TvPleaseLogin));

        private RelativeLayout LProfileData
            => _lProfileData ?? (_lProfileData = Activity.FindViewById<RelativeLayout>(Resource.Id.LProfileData));
    }
}