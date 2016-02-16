using System;
using EducationApp.iOS.Utilities;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    partial class CourseTabViewController : UITabBarController, INavigationPage
    {
        private UIBarButtonItem _button;
        private Course _course;
        private ILocalizedStringProvider _loc;


        public CourseTabViewController(IntPtr handle) : base(handle)
        {
        }

        private CourseViewModel Vm => Application.Locator.CourseViewModel;

        public object NavigationParameter
        {
            get { return _course; }
            set
            {
                _course = value as Course;
                Title = _course.Title;
                Application.Locator.CourseViewModel.Course = _course;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            _button = new UIBarButtonItem {Title = _loc.GetLocalizedString(Localized.ChangeLanguage_Label)};
            _button.Clicked += ChangeLanguageAlert;

            NavigationItem.SetRightBarButtonItem(_button, false);
        }

        private void ChangeLanguageAlert(object sender, EventArgs e)
        {
            var controller = UIAlertController.Create(_loc.GetLocalizedString(Localized.ChangeLanguage_Label), null,
                UIAlertControllerStyle.ActionSheet);

            foreach (var language in Vm.Languages)
            {
                controller.AddAction(UIAlertAction.Create(_loc.GetLocalizedString(language), UIAlertActionStyle.Default,
                    a => Vm.ChangeLanguageCommand.Execute(language)));
            }

            PresentViewController(controller, true, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            _button.Clicked -= ChangeLanguageAlert;
        }
    }
}