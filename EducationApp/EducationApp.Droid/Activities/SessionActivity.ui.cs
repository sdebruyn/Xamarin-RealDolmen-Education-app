using Android.Support.V7.Widget;

namespace EducationApp.Droid.Activities
{
    public sealed partial class SessionActivity
    {
        private Toolbar _toolbar;
        private Toolbar SessionToolbar => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.SessionToolbar));
    }
}