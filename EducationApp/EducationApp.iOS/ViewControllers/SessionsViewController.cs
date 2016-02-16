using System;
using EducationApp.Models;
using EducationApp.ViewModels;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    partial class SessionsViewController : UIViewController
    {
        private ObservableTableViewController<Session> _sessionListController;

        public SessionsViewController(IntPtr handle) : base(handle)
        {
        }

        public CourseViewModel Vm => Application.Locator.CourseViewModel;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Vm.UpdateSessionsCommand.Execute(null);

            _sessionListController = Vm.Course.Sessions.GetController(BaseViewController.CreateDefaultTableCell,
                BindSessionTableCell);
            _sessionListController.TableView = SessionsTable;
            _sessionListController.SelectionChanged += SelectSession;
        }

        private void BindSessionTableCell(UITableViewCell arg1, Session arg2, NSIndexPath arg3)
        {
            arg1.TextLabel.Text = arg2.Description;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            _sessionListController.SelectionChanged -= SelectSession;
            _sessionListController = null;
        }

        private void SelectSession(object sender, EventArgs e)
        {
            Vm.ViewSessionCommand.Execute(_sessionListController.SelectedItem);
        }
    }
}