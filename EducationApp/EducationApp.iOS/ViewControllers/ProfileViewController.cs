using System;
using System.Collections.Generic;
using EducationApp.iOS.Utilities;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    partial class ProfileViewController : BaseViewController
    {
        public ProfileViewController(IntPtr handle) : base(handle)
        {
        }

        public MainViewModel Vm => Application.Locator.MainViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void ViewDidLoad()
        {
            CurrentNavigationItem = NavigationItem;
            base.ViewDidLoad();
            TabBar.ItemSelected += NavigateOnTabBar;
            NavigationItem.HidesBackButton = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TabBar.SelectedItem = ProfileTabBarItem;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            TabBar.ItemSelected -= NavigateOnTabBar;
        }

        protected override TabBarItemType GetTabBarItemType(UITabBarItem item)
        {
            if (item == CoursesTabBarItem)
            {
                return TabBarItemType.Courses;
            }
            if (item == ProfileTabBarItem)
            {
                return TabBarItemType.Profile;
            }
            return base.GetTabBarItemType(item);
        }

        protected override IEnumerable<Binding> SetBindings() =>
            new List<Binding>
            {
                Application.Locator.MainViewModel.SetBinding(() => Application.Locator.MainViewModel.Identity)
                    .WhenSourceChanges(IdentityChanged)
            };

        protected override void IdentityChanged()
        {
            base.IdentityChanged();
            TvFirstName.Text = Vm.Identity?.FirstName;
            TvLastName.Text = Vm.Identity?.LastName;
            TvEmail.Text = Vm.Identity?.Email;
            PleaseLogin.Hidden = Vm.Identity != null;
            ProfileData.Hidden = Vm.Identity == null;
        }
    }
}