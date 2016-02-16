using System.ComponentModel;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
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
    public sealed partial class SessionActivity : ActivityBase
    {
        private IMenuItem _loginMenuItem;
        private IMenuItem _logoutMenuItem;
        internal SessionViewModel Vm => App.Locator.SessionViewModel;
        private CourseViewModel ParentVm => App.Locator.CourseViewModel;

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.SessionActivity);

            SessionToolbar.Title = ParentVm.Course.Title;
            SetSupportActionBar(SessionToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var viewPager = FindViewById<ViewPager>(Resource.Id.SessionViewPager);
            SetupViewPager(viewPager);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.SessionTabs);
            tabLayout.SetupWithViewPager(viewPager);

            Vm.PropertyChanged += VmPropertyChanged;
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vm.Identity))
            {
                _loginMenuItem?.SetVisible(Vm.Identity == null);
                _logoutMenuItem?.SetVisible(Vm.Identity != null);
            }
        }

        protected override void OnDestroy()
        {
            Vm.PropertyChanged -= VmPropertyChanged;
            base.OnDestroy();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);

            _loginMenuItem = menu.FindItem(Resource.Id.ActionLogIn);
            _logoutMenuItem = menu.FindItem(Resource.Id.ActionLogOut);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ActionLogIn:
                    Vm.LoginCommand.Execute(this);
                    break;
                case Resource.Id.ActionLogOut:
                    Vm.LogoutCommand.Execute(null);
                    break;
                default:
                    ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
                    break;
            }

            return true;
        }

        private void SetupViewPager(ViewPager viewPager)
        {
            var loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            var adapter = new ViewPagerAdapter(SupportFragmentManager);

            adapter.AddFragment(new SessionDetailsFragment(), loc.GetLocalizedString(Localized.SchedulePivotItem_Header));
            adapter.AddFragment(new SubscribeFragment(), loc.GetLocalizedString(Localized.SubscribePivotItem_Header));
            adapter.AddFragment(new FeedbackFragment(), loc.GetLocalizedString(Localized.FeedbackPivotItem_Header));
            adapter.AddFragment(new ContactFragment(), loc.GetLocalizedString(Localized.ContactPivotItem_Header));

            viewPager.Adapter = adapter;
        }

        protected override ViewModelBase GetViewModel() => Vm;
    }
}