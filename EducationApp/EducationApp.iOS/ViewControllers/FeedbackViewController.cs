using System;
using System.Collections.Generic;
using EducationApp.iOS.Utilities;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    partial class FeedbackViewController : BaseViewController
    {
        public FeedbackViewController(IntPtr handle) : base(handle)
        {
            DelayedActivation = true;
            DelayedParameter = true;
        }

        private SessionViewModel Vm => Application.Locator.SessionViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void ViewDidLoad()
        {
            CurrentNavigationItem = NavigationItem;
            base.ViewDidLoad();

            TabBar.ItemSelected += NavigateOnTabBar;
            Title = Application.Locator.CourseViewModel.Course.Title;
            IdentityChanged();

            TabBar.SelectedItem = FeedbackTabBarItem;
            SendFeedbackButton.SetCommand("TouchUpInside", Vm.SendFeedbackCommand);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ScrollView.SetUIScrollViewHeight(View.Frame.Width);
            var frame = ScrollView.Frame;
            frame.X = 0;
            ScrollView.Frame = frame;
        }

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

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            TabBar.ItemSelected -= NavigateOnTabBar;
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            this.SetBinding(() => SlClassRoom.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.Classroom)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlContent.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.Content)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlMaterials.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.CourseMaterials)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlGlobal.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.Global)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlGoalAttained.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.GoalAttained)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlConcepts.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.ConceptsAndPractice)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlDepth.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.InDepth)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlInstructorInteraction.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.InstructorInteraction)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlInstructorKnowledge.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.InstructorKnowledge)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlInstructorPresentation.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.InstructorPresentation)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlOrganization.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.Organization)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlWellPaced.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.WellPaced)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            this.SetBinding(() => SlWellStructured.Value, Application.Locator.SessionViewModel,
                () => Application.Locator.SessionViewModel.ScoredFeedback.WellStructured)
                .ConvertSourceToTarget(FloatToInt)
                .UpdateSourceTrigger("TouchUpInside"),
            Application.Locator.SessionViewModel.SetBinding(() => Application.Locator.SessionViewModel.Identity)
                .WhenSourceChanges(ShowLoginText),
            Application.Locator.SessionViewModel.SetBinding(() => Application.Locator.SessionViewModel.Identity, this,
                () => ScrollView.Hidden).ConvertSourceToTarget(identity => identity == null)
        };

        private static int? FloatToInt(float input) => (int) Math.Round((decimal) input, 0);
    }
}