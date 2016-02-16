using System;
using System.Collections.Generic;
using CoreGraphics;
using EducationApp.iOS.Controls;
using EducationApp.iOS.Utilities;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels.Utilities;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;
using NavigationService = EducationApp.iOS.Services.NavigationService;

namespace EducationApp.iOS.ViewControllers
{
    public abstract class BaseViewController : ControllerBase, INavigationPage
    {
        private readonly List<Binding> _bindings;
        private readonly LoadingOverlay _loader;
        protected readonly UIBarButtonItem LoginButton;
        protected readonly UIBarButtonItem LogoutButton;

        private bool _isLoading;
        private UITextView _tvPleaseLogin;

        protected UINavigationItem CurrentNavigationItem;
        protected bool DelayedActivation;
        protected bool DelayedParameter;

        protected BaseViewController(IntPtr handle) : base(handle)
        {
            _bindings = new List<Binding>();
            _loader = new LoadingOverlay(UIScreen.MainScreen.Bounds);

            LoginButton = new UIBarButtonItem();
            LogoutButton = new UIBarButtonItem();
        }

        protected virtual void IdentityChanged()
        {
            CurrentNavigationItem?.SetRightBarButtonItem(
                Application.Locator.MainViewModel.Identity == null ? LoginButton : LogoutButton, false);
        }

        protected abstract ViewModelBase GetViewModel();

        protected void LoadingChanged()
        {
            if (_isLoading == GetViewModel().IsLoading)
            {
                return;
            }
            _isLoading = GetViewModel().IsLoading;

            if (_isLoading)
            {
                View.Add(_loader);
            }
            else
            {
                _loader.Hide();
            }
        }

        public void ShowLoginText()
        {
            if (Application.Locator.MainViewModel.Identity == null)
            {
                _tvPleaseLogin = new UITextView(new CGRect(0, 0, View.Frame.Width, View.Frame.Height - 50))
                {
                    Text =
                        ServiceLocator.Current.GetInstance<ILocalizedStringProvider>()
                            .GetLocalizedString(Localized.PleaseLogin_Text),
                    TextAlignment = UITextAlignment.Center
                };
                View.AddSubview(_tvPleaseLogin);
            }
            else
            {
                _tvPleaseLogin?.RemoveFromSuperview();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (NavigationController?.InteractivePopGestureRecognizer != null)
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;

            var loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            LogoutButton.Title = loc.GetLocalizedString(Localized.Logout_Label);
            LoginButton.Title = loc.GetLocalizedString(Localized.Login_Label);

            if (!DelayedParameter)
            {
                (GetViewModel() as IAcceptParameterViewModel)?.SetParameter(NavigationParameter);
            }

            if (!DelayedActivation)
            {
                var toActivate = GetViewModel() as IActivationEnabledViewModel;
                toActivate?.ActivateAsync().ConfigureAwait(true);
            }

            LoginButton.Clicked += LoginButtonClick;
            LogoutButton.SetCommand("Clicked", GetViewModel().LogoutCommand);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _bindings.AddRange(SetBindings());
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _bindings.ForEach(b => b.ForceUpdateValueFromSourceToTarget());
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _bindings?.ForEach(b => b?.Detach());
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            _loader.Dispose();
            LoginButton.Clicked -= LoginButtonClick;
        }

        protected virtual IEnumerable<Binding> SetBindings() => new List<Binding>();

        internal static UITableViewCell CreateDefaultTableCell(NSString reuseIdentifier)
            => new UITableViewCell(UITableViewCellStyle.Default, reuseIdentifier)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator
            };

        protected static void BindCategoryCell(UITableViewCell uiTableViewCell, Category category, NSIndexPath arg3)
        {
            uiTableViewCell.TextLabel.Text = category.Name;
        }

        protected static void BindCourseCell(UITableViewCell arg1, Course arg2, NSIndexPath arg3)
        {
            arg1.TextLabel.Text = arg2.Title;
        }

        protected void NavigateOnTabBar(object sender, UITabBarItemEventArgs args)
            => ServiceLocator.Current.GetInstance<NavigationService>().NavigateTo(GetTabBarItemType(args.Item));

        protected virtual TabBarItemType GetTabBarItemType(UITabBarItem item) => TabBarItemType.Unknown;

        protected void LoginButtonClick(object sender, EventArgs e)
        {
            GetViewModel().LoginCommand.Execute(this);
        }
    }
}