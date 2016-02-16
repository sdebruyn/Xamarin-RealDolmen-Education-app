using System.Collections.Generic;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using EducationApp.Droid.Adapters;
using EducationApp.Droid.Fragments;
using EducationApp.Services;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Droid.Activities
{
    [Activity]
    public class SessionActivity : ActivityBase
    {
        private Toolbar _toolbar;
        public Toolbar SessionToolbar => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.SessionToolbar));
        internal SessionViewModel Vm => App.Locator.SessionViewModel;

        public IMenuItem LoginMenuItem { get; private set; }
        public IMenuItem LogoutMenuItem { get; private set; }

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.SessionActivity);

            SetSupportActionBar(SessionToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var viewPager = FindViewById<ViewPager>(Resource.Id.SessionViewPager);
            SetupViewPager(viewPager);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.SessionTabs);
            tabLayout.SetupWithViewPager(viewPager);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);

            LoginMenuItem = menu.FindItem(Resource.Id.ActionLogIn);
            LogoutMenuItem = menu.FindItem(Resource.Id.ActionLogOut);

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

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.CourseViewModel.SetBinding(() => App.Locator.CourseViewModel.Course.Title, this,
                () => SessionToolbar.Title, BindingMode.OneWay),
            App.Locator.SessionViewModel.SetBinding(() => App.Locator.SessionViewModel.Identity)
                .WhenSourceChanges(IdentityChanged)
        };

        private void IdentityChanged()
        {
            LoginMenuItem?.SetVisible(Vm.Identity == null);
            LogoutMenuItem?.SetVisible(Vm.Identity != null);
        }
    }
}