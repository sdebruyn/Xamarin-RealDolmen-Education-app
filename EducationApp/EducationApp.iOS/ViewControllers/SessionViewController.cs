using System;
using System.Collections.Generic;
using EducationApp.iOS.Utilities;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    public partial class SessionViewController : BaseViewController
    {
        private ObservableTableViewController<SessionSchedule> _scheduleListController;

        public SessionViewController(IntPtr handle) : base(handle)
        {
        }

        public SessionViewModel Vm => Application.Locator.SessionViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void ViewDidLoad()
        {
            CurrentNavigationItem = NavigationItem;
            base.ViewDidLoad();

            TabBar.ItemSelected += NavigateOnTabBar;
            Title = Application.Locator.CourseViewModel.Course.Title;
            IdentityChanged();

            _scheduleListController = Vm.Session.SessionSchedules.GetController(GetScheduleCellType,
                GetScheduleCellContent);
            _scheduleListController.TableView = SessionScheduleTable;

            TabBar.SelectedItem = ScheduleTabBarItem;
        }

        private static void GetScheduleCellContent(UITableViewCell cell, SessionSchedule schedule, NSIndexPath index)
        {
            var loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            cell.TextLabel.Text = schedule.StartTime.HasValue ? schedule.StartTime.Value.ToString("g") + " - " : "";
            cell.TextLabel.Text += schedule.EndTime?.ToString("t") ?? "...";
            cell.DetailTextLabel.Text =
                $"{schedule.ClassRoom.Name} ({loc.GetLocalizedString(Localized.Instructor_Text)} {schedule.Instructor.FirstName} {schedule.Instructor.LastName})";
        }

        private UITableViewCell GetScheduleCellType(NSString reuseIdentifier)
            => new UITableViewCell(UITableViewCellStyle.Subtitle, reuseIdentifier);

        protected override TabBarItemType GetTabBarItemType(UITabBarItem item)
        {
            if (item == ScheduleTabBarItem)
            {
                return TabBarItemType.Schedule;
            }
            if (item == ContactTabBarItem)
            {
                return TabBarItemType.Contact;
            }
            if (item == FeedbackTabBarItem)
            {
                return TabBarItemType.Feedback;
            }
            if (item == SubscribeTabBarItem)
            {
                return TabBarItemType.Subscribe;
            }

            return base.GetTabBarItemType(item);
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.SessionViewModel.SetBinding(() => Application.Locator.SessionViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            Application.Locator.SessionViewModel.SetBinding(
                () => Application.Locator.SessionViewModel.Session.Location.Name).WhenSourceChanges(LocationChanged),
            Application.Locator.SessionViewModel.SetBinding(
                () => Application.Locator.SessionViewModel.Session.Preparation, this, () => TvPreparation.Text)
        };

        private void LocationChanged()
        {
            TvLocation.Text = Vm.Session.Location.Name;
            TvLocation.Font = UIFont.FromDescriptor(UIFontDescriptor.PreferredSubheadline, 20);
            TvLocation.TextColor = Theme.RDBlue;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            TabBar.ItemSelected -= NavigateOnTabBar;

            _scheduleListController = null;
        }
    }
}