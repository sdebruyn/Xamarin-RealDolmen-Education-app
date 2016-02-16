using System.ComponentModel;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using EducationApp.Droid.Adapters;
using EducationApp.Droid.Fragments;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;

namespace EducationApp.Droid.Activities
{
    [Activity]
    [IntentFilter(new[] {"android.intent.action.VIEW"},
        Categories = new[] {"android.intent.category.DEFAULT", "android.intent.category.BROWSABLE"},
        DataScheme = Constants.Authentication.DefaultRedirectUriScheme)]
    public sealed class MainActivity : ActivityBase, SearchView.IOnQueryTextListener
    {
        private IMenuItem _loginMenuItem;
        private IMenuItem _logoutMenuItem;
        private SearchView _searchView;

        private MainViewModel Vm => App.Locator.MainViewModel;

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

            Vm.PropertyChanged += VmOnPropertyChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Vm.PropertyChanged -= VmOnPropertyChanged;
        }

        private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Vm.Identity):
                    _loginMenuItem?.SetVisible(Vm.Identity == null);
                    _logoutMenuItem?.SetVisible(Vm.Identity != null);
                    break;
            }
        }

        protected override ViewModelBase GetViewModel() => Vm;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);

            var searchMenuItem = menu.FindItem(Resource.Id.ActionSearch);
            _searchView = (SearchView) MenuItemCompat.GetActionView(searchMenuItem);
            _searchView.SetOnQueryTextListener(this);

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