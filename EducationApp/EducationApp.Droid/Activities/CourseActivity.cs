using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using EducationApp.Droid.Adapters;
using EducationApp.Droid.Fragments;
using EducationApp.Services;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Droid.Activities
{
    [Activity]
    public sealed partial class CourseActivity : ActivityBase
    {
        internal CourseViewModel Vm => App.Locator.CourseViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        private void SetupViewPager(ViewPager viewPager)
        {
            var loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            var adapter = new ViewPagerAdapter(SupportFragmentManager);

            adapter.AddFragment(new InformationFragment(), loc.GetLocalizedString(Localized.InformationPivotItem_Header));
            adapter.AddFragment(new DescriptionFragment(), loc.GetLocalizedString(Localized.DescriptionPivotItem_Header));
            adapter.AddFragment(new InstructorFragment(), loc.GetLocalizedString(Localized.InstructorPivotItem_Header));

            viewPager.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Course, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            AppCompatDialogFragment frag;
            switch (item.ItemId)
            {
                case Resource.Id.ActionShowSessions:
                    Vm.UpdateSessionsCommand.Execute(null);
                    frag = new SessionsFragment();
                    frag.Show(SupportFragmentManager, nameof(SessionsFragment));
                    break;
                case Resource.Id.ActionChangeLanguage:
                    frag = new LanguageFragment();
                    frag.Show(SupportFragmentManager, nameof(LanguageFragment));
                    break;
                default:
                    ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
                    break;
            }
            return true;
        }

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.CourseActivity);

            CourseToolbar.Title = Vm.Course.Title;
            SetSupportActionBar(CourseToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var viewPager = FindViewById<ViewPager>(Resource.Id.CourseViewPager);
            SetupViewPager(viewPager);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.CourseTabs);
            tabLayout.SetupWithViewPager(viewPager);
        }
    }
}