using System.Collections.Generic;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using EducationApp.Droid.Adapters;
using EducationApp.Droid.Fragments;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Activities
{
    [Activity]
    [IntentFilter(new[] {"android.intent.action.VIEW"},
        Categories = new[] {"android.intent.category.DEFAULT", "android.intent.category.BROWSABLE"},
        DataScheme = Constants.Authentication.DefaultRedirectUriScheme)]
    public class MainActivity : ActivityBase, SearchView.IOnQueryTextListener
    {
        public SearchView SearchView { get; private set; }
        public IMenuItem LoginMenuItem { get; private set; }
        public IMenuItem LogoutMenuItem { get; private set; }
        public MainViewModel Vm => App.Locator.MainViewModel;

        public bool OnQueryTextChange(string newText)
        {
            Vm.SearchValue = newText;
            return true;
        }

        public bool OnQueryTextSubmit(string query) => true;

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.MainActivity);

            var toolbar = FindViewById<Toolbar>(Resource.Id.MainToolbar);
            SetSupportActionBar(toolbar);

            var viewPager = FindViewById<ViewPager>(Resource.Id.MainViewPager);
            SetupViewPager(viewPager);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.MainTabs);
            tabLayout.SetupWithViewPager(viewPager);
        }

        protected override ViewModelBase GetViewModel() => Vm;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);

            var searchMenuItem = menu.FindItem(Resource.Id.ActionSearch);
            SearchView = (SearchView) MenuItemCompat.GetActionView(searchMenuItem);
            SearchView.SetOnQueryTextListener(this);

            LoginMenuItem = menu.FindItem(Resource.Id.ActionLogIn);
            LogoutMenuItem = menu.FindItem(Resource.Id.ActionLogOut);

            return true;
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.MainViewModel.SetBinding(() => App.Locator.MainViewModel.Identity)
                .WhenSourceChanges(IdentityChanged)
        };

        private void IdentityChanged()
        {
            LoginMenuItem?.SetVisible(Vm.Identity == null);
            LogoutMenuItem?.SetVisible(Vm.Identity != null);
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
            }

            return true;
        }

        private void SetupViewPager(ViewPager viewPager)
        {
            var adapter = new ViewPagerAdapter(SupportFragmentManager);

            adapter.AddFragment(new CategoryListFragment(), Resources.GetText(Resource.String.CoursesPivotItem_Header));
            adapter.AddFragment(new ProfileFragment(), Resources.GetText(Resource.String.ProfilePivotItem_Header));

            viewPager.Adapter = adapter;
        }
    }
}