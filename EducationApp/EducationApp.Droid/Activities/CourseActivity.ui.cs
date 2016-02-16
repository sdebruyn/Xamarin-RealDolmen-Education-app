using Android.Support.V7.Widget;

namespace EducationApp.Droid.Activities
{
    public sealed partial class CourseActivity
    {
        private Toolbar _toolbar;
        private Toolbar CourseToolbar => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.CourseToolbar));
    }
}